using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using MUNIA.Annotations;
using MUNIA.Util;

namespace MUNIA.Controllers {
	public class ControllerMapping {
		public int VendorID { get; set; }
		public int ProductID { get; set; }
		public int DeviceIndex { get; set; }
		public uint ReportHash { get; set; }
		public Guid UUID { get; set; } = Guid.Empty;
		public bool IsBuiltIn { get; internal set; }

		public ControllerType MappedType { get; set; }
		public enum SourceType { RawInput, XInput }
		public SourceType Source { get; set; }

		public List<AxisMap> AxisMaps = new List<AxisMap>();
		public List<ButtonMap> ButtonMaps = new List<ButtonMap>();
		public List<AxisToButtonMap> AxisToButtonMaps = new List<AxisToButtonMap>();
		public List<ButtonToAxisMap> ButtonToAxisMaps = new List<ButtonToAxisMap>();

		internal ControllerMapping() { }

		public ControllerMapping(XmlNode xn) {
			LoadFrom(xn);
		}

		public ControllerMapping(GenericController controller, ControllerType type) {
			MappedType = type;
			UUID = Guid.NewGuid();
			if (controller is XInputController xi) {
				Source = SourceType.XInput;
				DeviceIndex = xi.Index;
			}
			else if (controller is RawInputController ri) {
				Source = SourceType.RawInput;
				VendorID = ri.HidDevice.VendorID;
				ProductID = ri.HidDevice.ProductID;
				DeviceIndex = ri.DeviceItemIndex;
				ReportHash = CRC32.Calc(ri.HidDevice.GetRawReportDescriptor());
			}
		}


		public override string ToString() {
			// see if the controller for this mapping exists, if so
			// use its name in the string representation
			var dev = GenericController.ListDevices().FirstOrDefault(AppliesTo);
			return (IsBuiltIn ? "[Built-in] " : "") + (dev != null ? dev.Name : "?") + " -> " + MappedType;
		}

		public ControllerState ApplyMap(ControllerState state) {
			var ret = new ControllerState();
			foreach (var item in ButtonMaps) {
				if (state.Buttons.Count > (int)item.Source) {
					if (item.Target != Button.Unmapped) {
						ret.Buttons.EnsureSize((int)(item.Target + 1));
						ret.Buttons[(int)item.Target] = state.Buttons[(int)item.Source];
					}
				}
			}

			foreach (var item in ButtonToAxisMaps) {
				if (state.Buttons.Count > (int)item.Source) {
					if (item.Target != Axis.Unmapped && state.Buttons[(int)item.Source]) {
						ret.Axes.EnsureSize((int)(item.Target + 1));
						ret.Axes[(int)item.Target] = item.AxisValue;
					}
				}
			}

			foreach (var item in AxisMaps) {
				if (state.Axes.Count > (int)item.Source) {
					if (item.Target != Axis.Unmapped) {
						ret.Axes.EnsureSize((int)(item.Target + 1));
						ret.Axes[(int)item.Target] = state.Axes[(int)item.Source];
					}
				}
			}

			foreach (var item in AxisToButtonMaps) {
				if (state.Axes.Count > (int)item.Source) {
					if (item.Target != Button.Unmapped) {
						ret.Buttons.EnsureSize((int)(item.Target + 1));
						double axisVal = state.Axes[(int)item.Source];
						if (item.Mode == AxisToButtonMapMode.WhenAboveThreshold && axisVal > item.Threshold)
							ret.Buttons[(int)item.Target] = true;
						else if (item.Mode == AxisToButtonMapMode.WhenBelowThreshold && axisVal < item.Threshold)
							ret.Buttons[(int)item.Target] = true;
						else
							ret.Buttons[(int)item.Target] = false;
					}
				}
			}


			return ret;
		}

		public bool AppliesTo(GenericController controller) {
			if (Source == SourceType.RawInput && controller is RawInputController ri) {
				return ri.HidDevice.ProductID == ProductID &&
						ri.HidDevice.VendorID == VendorID &&
						CRC32.Calc(ri.HidDevice.GetRawReportDescriptor()) == ReportHash;
			}
			else if (Source == SourceType.XInput && controller is XInputController xi) {
				// xinput devices are always compatible
				return true;
			}

			return false;
		}

		public void LoadFrom(XmlNode xn) {
			MappedType = (ControllerType)Enum.Parse(typeof(ControllerType), xn.Attributes["type"].Value, true);
			Source = (SourceType)Enum.Parse(typeof(SourceType), xn.Attributes["source"].Value, true);
			UUID = xn.Attributes["uuid"] != null ? Guid.Parse(xn.Attributes["uuid"].Value) : Guid.NewGuid();

			if (Source == SourceType.RawInput) {
				VendorID = int.Parse(xn.Attributes["vid"].Value, NumberStyles.HexNumber);
				ProductID = int.Parse(xn.Attributes["pid"].Value, NumberStyles.HexNumber);
				ReportHash = uint.Parse(xn.Attributes["rpt_hash"].Value);
			}
			DeviceIndex = int.Parse(xn.Attributes["idx"].Value);

			foreach (XmlNode n in xn["buttons"].ChildNodes) {
				var btn = new ButtonMap();
				btn.LoadFrom(n);
				ButtonMaps.Add(btn);
			}

			foreach (XmlNode n in xn["axes"].ChildNodes) {
				var axis = new AxisMap();
				axis.LoadFrom(n);
				AxisMaps.Add(axis);
			}

			foreach (XmlNode n in xn["buttons_to_axis"].ChildNodes) {
				var button = new ButtonToAxisMap();
				button.LoadFrom(n);
				ButtonToAxisMaps.Add(button);
			}

			foreach (XmlNode n in xn["axis_to_buttons"].ChildNodes) {
				var axis = new AxisToButtonMap();
				axis.LoadFrom(n);
				AxisToButtonMaps.Add(axis);
			}
		}

		public void SaveTo(XmlTextWriter xw) {
			xw.WriteAttributeString("type", MappedType.ToString());
			xw.WriteAttributeString("source", Source.ToString());
			xw.WriteAttributeString("uuid", UUID.ToString());

			if (Source == SourceType.RawInput) {
				xw.WriteAttributeString("vid", VendorID.ToString("X"));
				xw.WriteAttributeString("pid", ProductID.ToString("X"));
				xw.WriteAttributeString("rpt_hash", ReportHash.ToString());
			}
			xw.WriteAttributeString("idx", DeviceIndex.ToString());

			xw.WriteStartElement("buttons");
			foreach (var btn in ButtonMaps)
				btn.SaveTo(xw);
			xw.WriteEndElement();

			xw.WriteStartElement("axes");
			foreach (var axis in AxisMaps)
				axis.SaveTo(xw);
			xw.WriteEndElement();

			xw.WriteStartElement("buttons_to_axis");
			foreach (var btn in ButtonToAxisMaps)
				btn.SaveTo(xw);
			xw.WriteEndElement();

			xw.WriteStartElement("axis_to_buttons");
			foreach (var axis in AxisToButtonMaps)
				axis.SaveTo(xw);
			xw.WriteEndElement();
		}

		public class AxisMap {
			public AxisMap() { }
			public AxisMap(Axis source, Axis target, bool isTrigger) {
				Source = source;
				Target = target;
				IsTrigger = isTrigger;
			}
			public AxisMap(int source, int target, bool isTrigger) {
				Source = (Axis)source;
				Target = (Axis)target;
				IsTrigger = isTrigger;
			}

			public Axis Source { get; set; }
			public Axis Target { get; set; }
			public bool IsTrigger { get; set; }

			public void LoadFrom(XmlNode xn) {
				Source = (Axis)int.Parse(xn.Attributes["source"].Value);
				Target = (Axis)int.Parse(xn.Attributes["target"].Value);
				IsTrigger = bool.Parse(xn.Attributes["is_trigger"].Value);
			}

			public void SaveTo(XmlTextWriter xw) {
				xw.WriteStartElement("axis");
				xw.WriteAttributeString("source", ((int)Source).ToString());
				xw.WriteAttributeString("target", ((int)Target).ToString());
				xw.WriteAttributeString("is_trigger", IsTrigger.ToString());
				xw.WriteEndElement();
			}
			public override string ToString() => $"{Source} --> {Target}";
			public AxisMap Clone() { return (AxisMap)MemberwiseClone(); }
		}

		public class AxisToButtonMap : INotifyPropertyChanged {
			public AxisToButtonMap() { }
			public AxisToButtonMap(Axis source, Button target, int threshold, AxisToButtonMapMode mode) {
				Source = source;
				Target = target;
				Threshold = threshold;
				Mode = mode;
			}
			public AxisToButtonMap(int source, int target, double threshold) {
				Source = (Axis)source;
				Target = (Button)target;
				Threshold = threshold;
				Mode = threshold > 0.0 ? AxisToButtonMapMode.WhenAboveThreshold : AxisToButtonMapMode.WhenBelowThreshold;
			}

			private Axis _source;

			public Axis Source {
				get => _source;
				set {
					if (value == _source) return;
					_source = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(Name));
				}
			}

			private Button _target;

			public Button Target {
				get => _target;
				set {
					if (value == _target) return;
					_target = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(Name));
				}
			}

			public double Threshold { get; set; }
			public AxisToButtonMapMode Mode { get; set; }

			public void LoadFrom(XmlNode xn) {
				Source = (Axis)int.Parse(xn.Attributes["source"].Value);
				Target = (Button)int.Parse(xn.Attributes["target"].Value);
				Threshold = double.Parse(xn.Attributes["threshold"].Value);
				Mode = (AxisToButtonMapMode)Enum.Parse(typeof(AxisToButtonMapMode), xn.Attributes["mode"].Value);
			}

			public void SaveTo(XmlTextWriter xw) {
				xw.WriteStartElement("axis_to_button");
				xw.WriteAttributeString("source", ((int)Source).ToString());
				xw.WriteAttributeString("target", ((int)Target).ToString());
				xw.WriteAttributeString("threshold", Threshold.ToString(CultureInfo.InvariantCulture));
				xw.WriteAttributeString("mode", Mode.ToString());
				xw.WriteEndElement();
			}
			public override string ToString() => $"{Source} --> {Target}";
			public string Name => ToString();
			public AxisToButtonMap Clone() { return (AxisToButtonMap)MemberwiseClone(); }
			public event PropertyChangedEventHandler PropertyChanged;

			[NotifyPropertyChangedInvocator]
			protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public enum AxisToButtonMapMode {
			WhenAboveThreshold,
			WhenBelowThreshold
		};

		public class ButtonMap {
			public ButtonMap() { }
			public ButtonMap(Button source, Button target) {
				Source = source;
				Target = target;
			}
			public ButtonMap(int source, int target) {
				Source = (Button)source;
				Target = (Button)target;
			}

			public Button Source { get; set; }
			public Button Target { get; set; }

			public void LoadFrom(XmlNode xn) {
				Source = (Button)int.Parse(xn.Attributes["source"].Value);
				Target = (Button)int.Parse(xn.Attributes["target"].Value);
			}

			public void SaveTo(XmlTextWriter xw) {
				xw.WriteStartElement("button");
				xw.WriteAttributeString("source", ((int)Source).ToString());
				xw.WriteAttributeString("target", ((int)Target).ToString());
				xw.WriteEndElement();
			}

			public override string ToString() => $"{Source} --> {Target}";
			public ButtonMap Clone() { return (ButtonMap)MemberwiseClone(); }
		}

		public class ButtonToAxisMap : INotifyPropertyChanged {
			public ButtonToAxisMap() { }
			public ButtonToAxisMap(Button source, Axis target, double axisValue) {
				Source = source;
				Target = target;
				AxisValue = axisValue;
			}
			public ButtonToAxisMap(int source, int target, double axisValue) {
				Source = (Button)source;
				Target = (Axis)target;
				AxisValue = axisValue;
			}

			private Button _source;

			public Button Source {
				get => _source;
				set {
					if (value == _source) return;
					_source = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(Name));
				}
			}

			private Axis _target;

			public Axis Target {
				get => _target;
				set {
					if (value == _target) return;
					_target = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(Name));
				}
			}

			public double AxisValue { get; set; }

			public void LoadFrom(XmlNode xn) {
				Source = (Button)int.Parse(xn.Attributes["source"].Value);
				Target = (Axis)int.Parse(xn.Attributes["target"].Value);
				AxisValue = double.Parse(xn.Attributes["axisValue"].Value);
			}

			public void SaveTo(XmlTextWriter xw) {
				xw.WriteStartElement("button_to_axis");
				xw.WriteAttributeString("source", ((int)Source).ToString());
				xw.WriteAttributeString("target", ((int)Target).ToString());
				xw.WriteAttributeString("axisValue", AxisValue.ToString(CultureInfo.InvariantCulture));
				xw.WriteEndElement();
			}

			public string Name => ToString();
			public override string ToString() => $"{Source} --> {Target}";

			public ButtonToAxisMap Clone() { return (ButtonToAxisMap)MemberwiseClone(); }
			public event PropertyChangedEventHandler PropertyChanged;

			[NotifyPropertyChangedInvocator]
			protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public enum Button : int {
			Unmapped = -1,
			Button0 = 0, Button1, Button2, Button3,
			Button4, Button5, Button6, Button7,
			Button8, Button9, Button10, Button11,
			Button12, Button13, Button14, Button15,
			Button16, Button17, Button18, Button19,
			Button20, Button21, Button22, Button23,
			Button24, Button25, Button26, Button27,
			Button28, Button29, Button30, Button31
		}

		public enum Axis : int {
			Unmapped = -1,
			Axis0 = 0, Axis1, Axis2, Axis3,
			Axis4, Axis5, Axis6, Axis7,
			Axis8, Axis9, Axis10, Axis11,
			Axis12, Axis13, Axis14, Axis15,
			Axis16, Axis17, Axis18, Axis19,
			Axis20, Axis21, Axis22, Axis23,
			Axis24, Axis25, Axis26, Axis27,
			Axis28, Axis29, Axis30, Axis31
		}

		public ControllerMapping Clone() {
			var ret = new ControllerMapping();
			ret.IsBuiltIn = false;
			ret.Source = this.Source;
			ret.MappedType = this.MappedType;
			ret.UUID = Guid.NewGuid();
			ret.DeviceIndex = this.DeviceIndex;
			ret.VendorID = this.VendorID;
			ret.ProductID = this.ProductID;
			ret.ReportHash = this.ReportHash;
			foreach (var x in this.ButtonMaps)
				ret.ButtonMaps.Add(x.Clone());
			foreach (var x in this.ButtonToAxisMaps)
				ret.ButtonToAxisMaps.Add(x.Clone());
			foreach (var x in this.AxisMaps)
				ret.AxisMaps.Add(x.Clone());
			foreach (var x in this.AxisToButtonMaps)
				ret.AxisToButtonMaps.Add(x.Clone());
			return ret;
		}

	}

}
