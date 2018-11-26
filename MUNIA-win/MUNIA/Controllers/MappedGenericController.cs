using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using HidSharp;
using HidSharp.Reports;
using MUNIA.Util;

namespace MUNIA.Controllers {
	public class MappedGenericController : GenericController {
		private readonly ControllerMapping _mapping;

		public MappedGenericController(ControllerMapping mapping, HidDevice device, DeviceItem deviceItem) : base(device, deviceItem) {
			_mapping = mapping;
		}

		public override ControllerType Type => _mapping.MappedType;
		public override ControllerState GetState() {
			return _mapping.ApplyMap(base.GetState());
		}

		public new static IEnumerable<MappedGenericController> ListDevices() {
			var controllers = DeviceList.Local.GetHidDevices();
			foreach (var mapping in ConfigManager.ControllerMappings) {
				foreach (var matchingController in controllers.Where(c =>
					c.ProductID == mapping.ProductID &&
					c.VendorID == mapping.VendorID &&
					CRC32.Calc(c.GetRawReportDescriptor()) == mapping.ReportHash)) {

					var devItem = matchingController.GetReportDescriptor().DeviceItems[mapping.DeviceItemIndex];
					yield return new MappedGenericController(mapping, matchingController, devItem);
				}

			}
		}

		public override bool IsAxisTrigger(int axisNum) {
			var map = _mapping.AxisMaps.FirstOrDefault(a => a.Target == axisNum);
			return map?.IsTrigger == true;
		}
	}

	public class ControllerMapping {
		public int VendorID { get; set; }
		public int ProductID { get; set; }
		public int DeviceItemIndex { get; set; }
		public uint ReportHash { get; set; }

		public ControllerType MappedType { get; set; }


		public BindingList<HatMap> HatMaps = new BindingList<HatMap>();
		public BindingList<AxisMap> AxisMaps = new BindingList<AxisMap>();
		public BindingList<ButtonMap> ButtonMaps = new BindingList<ButtonMap>();

		public ControllerMapping(XmlNode xn) {
			LoadFrom(xn);
		}

		public ControllerMapping(GenericController controller) {
			VendorID = controller.HidDevice.VendorID;
			ProductID = controller.HidDevice.ProductID;
			DeviceItemIndex = controller.DeviceItemIndex;
			ReportHash = CRC32.Calc(controller.HidDevice.GetRawReportDescriptor());

			ControllerState state = null;
			try {
				controller.Activate();
				state = controller.GetState();
				controller.Deactivate();
			}
			catch { }

			// default mapping
			if (state != null) {
				for (int i = 0; i < state.Hats.Count; i++)
					HatMaps.Add(new HatMap { Source = i, Target = i });
				for (int i = 0; i < state.Buttons.Count; i++)
					ButtonMaps.Add(new ButtonMap { Source = i, Target = i });
				for (int i = 0; i < state.Axes.Count; i++)
					AxisMaps.Add(new AxisMap { Source = i, Target = i, IsTrigger = controller.IsAxisTrigger(i) });
			}
		}

		public void LoadFrom(XmlNode xn) {
			VendorID = int.Parse(xn.Attributes["vid"].Value);
			ProductID = int.Parse(xn.Attributes["pid"].Value);
			DeviceItemIndex = int.Parse(xn.Attributes["idx"].Value);
			ReportHash = uint.Parse(xn.Attributes["rpt_hash"].Value);

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

			foreach (XmlNode n in xn["hats"].ChildNodes) {
				var hat = new HatMap();
				hat.LoadFrom(n);
				HatMaps.Add(hat);
			}
		}

		public void SaveTo(XmlTextWriter xw) {
			xw.WriteStartElement("mapping");
			xw.WriteAttributeString("vid", VendorID.ToString());
			xw.WriteAttributeString("pid", ProductID.ToString());
			xw.WriteAttributeString("idx", DeviceItemIndex.ToString());
			xw.WriteAttributeString("rpt_hash", ReportHash.ToString());

			xw.WriteStartElement("buttons");
			foreach (var btn in ButtonMaps)
				btn.SaveTo(xw);
			xw.WriteEndElement();

			xw.WriteStartElement("axes");
			foreach (var axis in AxisMaps)
				axis.SaveTo(xw);
			xw.WriteEndElement();

			xw.WriteStartElement("hats");
			foreach (var hat in HatMaps)
				hat.SaveTo(xw);
			xw.WriteEndElement();

			xw.WriteEndElement();
		}

		public ControllerState ApplyMap(ControllerState state) {
			var ret = new ControllerState();
			foreach (var item in ButtonMaps) {
				if (state.Buttons.Count > item.Source) {
					ret.Buttons.EnsureSize(item.Target + 1);
					ret.Buttons[item.Target] = state.Buttons[item.Source];
				}
			}

			foreach (var item in HatMaps.OrderBy(h => h.Target)) {
				if (state.Hats.Count > item.Source) {
					ret.Hats.EnsureSize(item.Target + 1);
					var hat = state.Hats[item.Source];
					ret.Hats[item.Target] = hat;
					ret.Buttons.Add(hat.HasFlag(Hat.Up));
					ret.Buttons.Add(hat.HasFlag(Hat.Down));
					ret.Buttons.Add(hat.HasFlag(Hat.Left));
					ret.Buttons.Add(hat.HasFlag(Hat.Right));
				}
			}
			foreach (var item in AxisMaps) {
				if (state.Axes.Count > item.Source) {
					ret.Axes.EnsureSize(item.Target + 1);
					ret.Axes[item.Target] = state.Axes[item.Source];
				}
			}

			return ret;
		}

		public class HatMap {
			public int Source;
			public int Target;

			public void LoadFrom(XmlNode xn) {
				Source = int.Parse(xn.Attributes["source"].Value);
				Target = int.Parse(xn.Attributes["target"].Value);
			}

			public void SaveTo(XmlTextWriter xw) {
				xw.WriteStartElement("hat");
				xw.WriteAttributeString("source", Source.ToString());
				xw.WriteAttributeString("target", Target.ToString());
				xw.WriteEndElement();
			}
		}
		public class AxisMap {
			public int Source;
			public int Target;
			public bool IsTrigger;

			public void LoadFrom(XmlNode xn) {
				Source = int.Parse(xn.Attributes["source"].Value);
				Target = int.Parse(xn.Attributes["target"].Value);
				IsTrigger = bool.Parse(xn.Attributes["is_trigger"].Value);
			}

			public void SaveTo(XmlTextWriter xw) {
				xw.WriteStartElement("axis");
				xw.WriteAttributeString("source", Source.ToString());
				xw.WriteAttributeString("target", Target.ToString());
				xw.WriteAttributeString("is_trigger", IsTrigger.ToString());
				xw.WriteEndElement();
			}
		}
		public class ButtonMap {
			public int Source;
			public int Target;

			public void LoadFrom(XmlNode xn) {
				Source = int.Parse(xn.Attributes["source"].Value);
				Target = int.Parse(xn.Attributes["target"].Value);
			}

			public void SaveTo(XmlTextWriter xw) {
				xw.WriteStartElement("button");
				xw.WriteAttributeString("source", Source.ToString());
				xw.WriteAttributeString("target", Target.ToString());
				xw.WriteEndElement();
			}

		}
	}

}
