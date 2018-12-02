using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using HidSharp;
using HidSharp.Reports;
using HidSharp.Reports.Input;
using MUNIA.Util;

namespace MUNIA.Controllers {
	public class RawInputController : GenericController {
		internal readonly HidDevice HidDevice;
		internal readonly DeviceItem DeviceItem;
		public readonly int DeviceItemIndex;
		public override string DevicePath => HidDevice.DevicePath;

		public override string Name => HidDevice.ProductName;

		public override ControllerType Type => ControllerType.Generic;

		public override bool RequiresPolling => false;

		public RawInputController() { }
		public RawInputController(HidDevice device, DeviceItem deviceItem) {
			HidDevice = device;
			DeviceItem = deviceItem;
			DeviceItemIndex = DeviceItem.ParentItem.ChildItems.IndexOf(deviceItem);
		}

		// stream, parser and report cache only available while device activated
		private HidStream _stream;
		private DeviceItemInputParser _inputParser;
		private readonly Dictionary<byte, Report> _reportCache = new Dictionary<byte, Report>();
		
		public override bool Activate() {
			_inputParser = DeviceItem.CreateDeviceItemInputParser();
			try {
				_stream?.Dispose();
				_stream = HidDevice.Open();
				_stream.ReadTimeout = Timeout.Infinite;

				// determine number of buttons, axes, hats
				DetermineCapabilities();

				byte[] buffer = new byte[HidDevice.GetMaxInputReportLength()];
				var sb = new MuniaController.StreamAndBuffer { buffer = buffer, stream = _stream };

				_stream.BeginRead(buffer, 0, buffer.Length, Callback, sb);
				return true;
			}
			catch {
				return false;
			}
		}

		private void DetermineCapabilities() {
			_hats.Clear();
			_buttons.Clear();
			_axes.Clear();
			int numHats = 0;
			int numButtons = 0;
			int numAxes = 0;
			foreach (var report in DeviceItem.InputReports) {
				foreach (var dataItem in report.DataItems) {
					foreach (Usage usage in dataItem.Usages.GetAllValues()) {
						if (Usage.Button1 <= usage && usage <= Usage.Button31) {
							int btnIdx = (int)(usage - (int)Usage.Button1);
							numButtons = Math.Max(numButtons, btnIdx + 1);
						}
						else if (usage == Usage.GenericDesktopHatSwitch) {
							numHats++;
						}
						else if (Usage.GenericDesktopX <= usage && usage <= Usage.GenericDesktopRz) {
							int axisIdx = (int)(usage - (int)Usage.GenericDesktopX);
							numAxes = Math.Max(numAxes, axisIdx + 1);
						}
						else {
							// unrecognized usage
							Debug.WriteLine("Unrecognized usage: " + usage);
						}
					}
				}
			}

			_hats.EnsureSize(numHats);
			numButtons += 4 * numHats; // also map each direction of hat as separate button
			_buttons.EnsureSize(numButtons);
			_axes.EnsureSize(numAxes);
		}

		public override void Deactivate() {
			_stream?.Dispose();
			_stream = null;
			_reportCache.Clear();
		}
		public override bool IsAxisTrigger(int axisNum) {
			Usage usage = Usage.GenericDesktopX + (uint)axisNum;
			return IsTriggerAxisByDefault(usage);
		}

		private void Callback(IAsyncResult ar) {
			var sb = (MuniaController.StreamAndBuffer)ar.AsyncState;
			try {
				int numBytes = sb.stream.EndRead(ar);
				if (numBytes > 0) {
					if (Parse(sb.buffer))
						OnStateUpdated(EventArgs.Empty);
					sb.stream.BeginRead(sb.buffer, 0, sb.buffer.Length, Callback, sb);
				}
			}
			catch (IOException exc) {
				_stream = null;
				Debug.WriteLine("IOException: " + exc.Message);
				sb.stream.Dispose();
			}
			catch (ObjectDisposedException) { }
			catch (NullReferenceException) { }
		}
		
		private bool Parse(byte[] reportBuffer) {
			try {
				byte reportId = reportBuffer[0];
				if (!_reportCache.TryGetValue(reportId, out Report report)) {
					_reportCache[reportId] = report = HidDevice.GetReportDescriptor().GetReport(ReportType.Input, reportId);
				}

				// Parse the report if possible.
				if (_inputParser.TryParseReport(reportBuffer, 0, report)) {
					while (_inputParser.HasChanged) {
						int changedIndex = _inputParser.GetNextChangedIndex();
						var dataValue = _inputParser.GetValue(changedIndex);

						Usage usage = (Usage)dataValue.Usages.FirstOrDefault();
						if (Usage.Button1 <= usage && usage <= Usage.Button31) {
							int btnIdx = (int)(usage - (int)Usage.Button1);
							_buttons.EnsureSize(btnIdx + 1);
							_buttons[btnIdx] = dataValue.GetLogicalValue() != 0;
							int val = dataValue.GetLogicalValue();
						}
						else if (usage == Usage.GenericDesktopHatSwitch) {
							// can only support 1 hat..
							_hats.EnsureSize(1);
							_hats[0] = ExtractHat(dataValue);
						}
						else if (Usage.GenericDesktopX <= usage && usage <= Usage.GenericDesktopRz) {
							int axisIdx = (int)(usage - (int)Usage.GenericDesktopX);
							_axes.EnsureSize(axisIdx + 1);
							_axes[axisIdx] = ScaleAxis(dataValue);
						}
						else {
							// unrecognized usage
							Debug.WriteLine("Unrecognized usage: " + usage);
						}
					}

					// for skinning simplicity sake, map hats also to buttons
					int btn = _buttons.Count - 4 * _hats.Count;
					for (int i = 0; i < _hats.Count; i++) {
						// UP DOWN LEFT RIGHT
						_buttons[btn++] = _hats[i].HasFlag(Hat.Up);
						_buttons[btn++] = _hats[i].HasFlag(Hat.Down);
						_buttons[btn++] = _hats[i].HasFlag(Hat.Left);
						_buttons[btn++] = _hats[i].HasFlag(Hat.Right);
					}

					return true;
				}
			}
			catch { }
			return false;
		}

		private double ScaleAxis(DataValue dataValue) {
			// first see if this item has a logical min/max defined
			var di = dataValue.DataItem;
			double val;
			if (di.LogicalMinimum < di.LogicalMaximum) {
				val = (dataValue.GetLogicalValue() - di.LogicalMinimum) / (double)di.LogicalRange;
			}
			else {
				int range = 1 << di.ElementBits;
				val = dataValue.GetLogicalValue() / (double)range;
			}

			if (!IsAxisTrigger((int)(dataValue.Usages.First() - Usage.GenericDesktopX))) {
				val -= 0.5;
				val *= 2.0;
			}

			return val;
		}
		private Hat ExtractHat(DataValue dataValue) {
			double phys = dataValue.GetPhysicalValue();
			if (double.IsNaN(phys))
				return Hat.None;

			// first see if this item has a logical min/max defined
			var di = dataValue.DataItem;
			int logical;
			if (di.LogicalMinimum < di.LogicalMaximum) {
				logical = dataValue.GetLogicalValue() - di.LogicalMinimum;
			}
			else {
				int range = 1 << di.ElementBits;
				logical = dataValue.GetLogicalValue() / range;
			}

			byte idx = (byte)(logical % 9);
			return ControllerState.HatLookup[idx];
		}

		private bool IsTriggerAxisByDefault(Usage usage) {
			return usage == Usage.GenericDesktopRz || usage == Usage.GenericDesktopZ;
		}

		public override bool IsAvailable {
			get {
				HidStream s;
				var ret = HidDevice.TryOpen(out s);
				if (!ret) return false;
				ret = s.CanRead;
				s.Close();
				return ret;
			}
		}

		private static Regex DevicePathRegex = new Regex(@"ig_(\d{1,2})[&#{}\b]", RegexOptions.Compiled);
		internal static IEnumerable<RawInputController> ListDevices() {
			// find all devices with a gamepad or joystick usage page
			foreach (var dev in DeviceList.Local.GetHidDevices()) {
				var matches = DevicePathRegex.Match(dev.DevicePath);
				if (matches.Success) {
					// according to Microsoft, this is a 'good' way to distinguish XInput controllers
					// from raw/directinput compatible ones. I have my doubts...
					// https://docs.microsoft.com/en-us/windows/desktop/xinput/xinput-and-directinput
					continue;
				}
				else {
					var reportDescriptor = dev.GetReportDescriptor();
					foreach (var deviceItem in reportDescriptor.DeviceItems) {
						bool isJoystickOrGamepad = deviceItem.Usages.GetAllValues().Contains((uint)Usage.GenericDesktopJoystick) ||
													deviceItem.Usages.GetAllValues().Contains((uint)Usage.GenericDesktopGamepad);
						if (isJoystickOrGamepad && deviceItem.InputReports.Any())
							yield return new RawInputController(dev, deviceItem);
					}
				}
			}
		}

		public override void Dispose() {
		}

	}
}
