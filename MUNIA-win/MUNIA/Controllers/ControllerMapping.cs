using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Xml;
using HidSharp;
using MUNIA.Util;

namespace MUNIA.Controllers {
	public class ControllerMapping {
		public int VendorID { get; set; }
		public int ProductID { get; set; }
		public int DeviceIndex { get; set; }
		public uint ReportHash { get; set; }

		public ControllerType MappedType { get; set; }
		public enum SourceType { RawInput, XInput }
		public SourceType Source { get; set; }
		
		public List<AxisMap> AxisMaps = new List<AxisMap>();
		public List<ButtonMap> ButtonMaps = new List<ButtonMap>();

		public ControllerMapping(XmlNode xn) {
			LoadFrom(xn);
		}

		public ControllerMapping(GenericController controller, ControllerType type) {
			MappedType = type;

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
			return (dev != null ? dev.Name : "?") + " -> " + MappedType;
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

			foreach (var item in AxisMaps) {
				if (state.Axes.Count > (int)item.Source) {
					if (item.Target != Axis.Unmapped) {
						ret.Axes.EnsureSize((int)(item.Target + 1));
						ret.Axes[(int)item.Target] = state.Axes[(int)item.Source];
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
		}

		public void SaveTo(XmlTextWriter xw) {
			xw.WriteAttributeString("type", MappedType.ToString());
			xw.WriteAttributeString("source", Source.ToString());
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
		}

		public class AxisMap {
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
		public class ButtonMap {
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

	}

}
