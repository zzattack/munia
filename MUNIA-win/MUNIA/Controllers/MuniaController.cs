using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using HidSharp;

namespace MUNIA.Controllers {
	public abstract class MuniaController : IController, IDisposable {
		/// <summary>
		/// Can be used to register for WM_INPUT messages and parse them.
		/// For testing purposes it can also be used to solely register for WM_INPUT messages.
		/// </summary>

		public HidDevice HidDevice;
		private HidStream _stream;
		public string DevicePath => HidDevice.DevicePath;
		public virtual string Name => HidDevice.ProductName;
		public abstract ControllerType Type { get; }

		protected readonly List<bool> _buttons = new List<bool>();
		protected readonly List<int> _axes = new List<int>();
		protected readonly List<Hat> _hats = new List<Hat>();
		public ControllerState GetState() => new ControllerState(_axes, _buttons, _hats);

		public event EventHandler StateUpdated;

		protected MuniaController(HidDevice hidDevice) {
			this.HidDevice = hidDevice;
		}
		public override string ToString() {
			return Name;
		}

		public static IEnumerable<MuniaController> ListDevices() {

			// MUNIA devices with 0x1209 VID
			foreach (var device in DeviceList.Local.GetHidDevices(0x1209, 0x8843)) {
				if (device.ProductName.Equals("NinHID NGC")) {
					yield return new MuniaNgc(device);
				}
				else if (device.ProductName.Equals("NinHID N64")) {
					yield return new MuniaN64(device);
				}
				else if (device.ProductName.Equals("NinHID SNES")) {
					yield return new MuniaSnes(device);
				}
			}

			// MUSIA devices
			foreach (var device in DeviceList.Local.GetHidDevices(0x1209, 0x8844)) {
				if (device.ProductName.Equals("MUSIA PS2 controller")) {
					yield return new MusiaPS2(device);
				}
			}

			
			// legacy VID/PID combo from microchip
			foreach (var device in DeviceList.Local.GetHidDevices(0x04d8, 0x0058)) {
				if (device.ProductName.Equals("NinHID NGC") || (device.GetMaxInputReportLength() == 9 && device.GetMaxOutputReportLength() == 0)) {
					yield return new MuniaNgc(device);
				}
				else if (device.ProductName.Equals("NinHID N64") || device.GetMaxInputReportLength() == 5) {
					yield return new MuniaN64(device);
				}
				else if (device.ProductName.Equals("NinHID SNES") || device.GetMaxInputReportLength() == 3) {
					yield return new MuniaSnes(device);
				}
			}

		}

		public static IEnumerable<ConfigInterface> GetConfigInterfaces() {
			return GetMuniaConfigInterfaces().OfType<ConfigInterface>()
				.Union(GetMusiaConfigInterfaces());
		}

		public static IEnumerable<MuniaConfigInterface> GetMuniaConfigInterfaces() {
			var candidates = DeviceList.Local.GetHidDevices(0x04d8, 0x0058)
				.Union(DeviceList.Local.GetHidDevices(0x1209, 0x8843));
			foreach (HidDevice dev in candidates.Where(device => device.ProductName.Equals("NinHID CFG") 
				|| (device.GetMaxInputReportLength() == device.GetMaxOutputReportLength()
					&& device.GetMaxOutputReportLength() == 9)))
				yield return new MuniaConfigInterface(dev);
		}

		public static IEnumerable<MusiaConfigInterface> GetMusiaConfigInterfaces() {
			foreach (HidDevice dev in DeviceList.Local.GetHidDevices(0x1209, 0x8844).Where(d => d.ProductName.Contains("config")))
				yield return new MusiaConfigInterface(dev);
		}

		class StreamAndBuffer {
			public byte[] buffer;
			public Stream stream;
		}

		/// <summary>
		/// Registers device for receiving inputs.
		/// </summary>
		public bool Activate() {
			try {
				_stream?.Dispose();
				_stream = HidDevice.Open();
				_stream.ReadTimeout = Timeout.Infinite;

				byte[] buffer = new byte[HidDevice.GetMaxInputReportLength()];
				var sb = new StreamAndBuffer { buffer = buffer, stream = _stream };

				_stream.BeginRead(buffer, 0, buffer.Length, Callback, sb);
				return true;
			}
			catch {
				return false;
			}
		}

		public void Deactivate() {
			_stream?.Dispose();
			_stream = null;
		}
		public bool IsActive => _stream != null && _stream.CanRead;
		public bool IsAvailable {
			get {
				HidStream s;
				var ret = HidDevice.TryOpen(out s);
				if (!ret) return false;
				ret = s.CanRead;
				s.Close();
				return ret;
			}
		}

		private void Callback(IAsyncResult ar) {
			var sb = (StreamAndBuffer)ar.AsyncState;
			try {
				int numBytes = sb.stream.EndRead(ar);
				if (numBytes > 0) {
					if (Parse(sb.buffer))
						StateUpdated?.Invoke(this, EventArgs.Empty);
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

		public void Dispose() {
			_stream?.Dispose();
		}

		protected abstract bool Parse(byte[] ev);

	}
}