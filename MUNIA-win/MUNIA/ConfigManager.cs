using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MUNIA.Controllers;
using MUNIA.Forms;
using MUNIA.Skinning;

namespace MUNIA {
	public static class ConfigManager {
		private static readonly string SettingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MUNIA");
		private static readonly string SettingsFile = Path.Combine(SettingsDir, "munia_settings.xml");
		public static string Email { get; set; } = "nobody@nothing.net";
		public static Color BackgroundColor { get; set; } = Color.Gray;
		public static Dictionary<Skin, Size> WindowSizes { get; } = new Dictionary<Skin, Size>();
		public static Dictionary<SvgSkin, ColorRemap> SelectedRemaps { get; } = new Dictionary<SvgSkin, ColorRemap>();
		public static Dictionary<SvgSkin, BindingList<ColorRemap>> Remaps { get; } 
			= new Dictionary<SvgSkin, BindingList<ColorRemap>>();
		public static readonly ArduinoMapping ArduinoMapping = new ArduinoMapping();

		private static TimeSpan _delay;
		private static IController _activeController;
		private static BufferedController _bufferedActiveController;
		public static Skin ActiveSkin { get; set; }

		
		public static List<IController> Controllers { get; } = new List<IController>();
		public static List<Skin> Skins { get; } = new List<Skin>();

		public static TimeSpan Delay {
			get { return _delay; }
			set {
				_delay = value;
				if (_delay != TimeSpan.Zero && _activeController != null) {
					if (_bufferedActiveController == null) _bufferedActiveController = new BufferedController(_activeController, Delay);
					else _bufferedActiveController.Delay = _delay;
				}
			}
		}
		
		public static void SetActiveController(IController value) {
			// deactive old controller
			_activeController?.Deactivate();
			_activeController = value;
			if (_activeController != null) {
				_activeController.Activate();
				if (Delay != TimeSpan.Zero)
					_bufferedActiveController = new BufferedController(value, Delay);
			}
		}

		public static IController GetActiveController() {
			return Delay == TimeSpan.Zero ? _activeController : _bufferedActiveController;
		}

		public static void LoadSkins() {
			foreach (string svgPath in Directory.GetFiles("./skins", "*.svg")) {
				var svg = new SvgSkin();
				svg.Load(svgPath);
				Skins.Add(svg);

				// create default remaps
				Remaps[svg] = new BindingList<ColorRemap>() {
					ColorRemap.CreateFromSkin(svg),
				};
				SelectedRemaps[svg] = null;
			}
			foreach (string padpyghtDir in Directory.GetDirectories("./skins")) {
				foreach (string iniPath in Directory.GetFiles(padpyghtDir, "*.ini")) {
					var pp = new PadpyghtSkin();
					pp.Load(iniPath);
					if (pp.LoadResult == SkinLoadResult.Ok)
						Skins.Add(pp);
				}
			}
		}

		public static void LoadControllers() {
			Controllers.Clear();
			foreach (var dev in MuniaController.ListDevices()) {
				Controllers.Add(dev);
			}
			foreach (var dev in ArduinoControllerManager.ListDevices()) {
				Controllers.Add(dev);
			}
		}

		

		#region Xml serialization
		public static void Load() {
			XmlElement xroot = null;

			try {
				XmlDocument xdoc = new XmlDocument();
				xdoc.Load(SettingsFile);
				xroot = xdoc["settings"] ?? xdoc["root"];

				// first load the simplest properties
				if (xroot["Email"] != null) Email = xroot["Email"].InnerText;
				if (xroot["BackgroundColor"] != null) BackgroundColor = Color.FromArgb(int.Parse(xroot["BackgroundColor"].InnerText));
				if (xroot["Delay"] != null) Delay = TimeSpan.FromMilliseconds(int.Parse(xroot["Delay"].InnerText));
			}
			catch (XmlException) { }
			catch (FormatException) { }


			// then load all skins as they have no dependencies
			LoadSkins();

			try {
				// now we can load the skin-specific settings
				if (xroot["active_skin"] != null)
					ActiveSkin = Skins.FirstOrDefault(s => s.Path == xroot["active_skin"].InnerText);

				foreach (XmlNode skinCfg in xroot["skin_settings"].ChildNodes) {
					string path = skinCfg.Attributes["skin_path"].Value;

					var skin = Skins.FirstOrDefault(s => s.Path == path);
					var wsz = skinCfg.Attributes["window_size"];
					if (wsz != null && skin != null) {
						string size = wsz.Value;
						Size sz = new Size(int.Parse(size.Substring(0, size.IndexOf("x"))), int.Parse(size.Substring(size.IndexOf("x") + 1)));
						WindowSizes[skin] = sz;
					}

					if (skin is SvgSkin svg && skinCfg["remaps"] != null) {
						var remaps = Remaps[svg];
						foreach (XmlNode xRemap in skinCfg["remaps"]) {
							remaps.Add(ColorRemap.LoadFrom(xRemap));
						}

						if (skinCfg.Attributes["active_remap"] != null) {
							Guid uuid = Guid.Parse(skinCfg.Attributes["active_remap"].Value);
							SelectedRemaps[svg] = remaps.FirstOrDefault(r => r.UUID == uuid);
						}
					}
				}

				// load arduino map, then controllers
				var arduinoMap = xroot["arduino_mapping"];
				if (arduinoMap != null) {
					foreach (XmlNode e in arduinoMap.ChildNodes) {
						ArduinoMapping[e.Attributes["port"].Value] =
							(ControllerType)Enum.Parse(typeof(ControllerType), e.Attributes["type"].Value, true);
					}
				}
			}
			catch { }
			
			LoadControllers();

			try {
				// finally we can determine the active controller
				if (xroot["active_dev_path"] != null)
					SetActiveController(Controllers.FirstOrDefault(c => c.DevicePath == xroot["active_dev_path"].InnerText));
			}
			catch { }
		}

		public static void Save() {
			try {
				Directory.CreateDirectory(SettingsDir);
				var ms = new MemoryStream();
				var xw = new XmlTextWriter(ms, Encoding.UTF8);
				xw.Formatting = Formatting.Indented;
				xw.WriteStartDocument();
				xw.WriteStartElement("settings");

				xw.WriteElementString("BackgroundColor", BackgroundColor.ToArgb().ToString());
				xw.WriteElementString("Email", Email);
				xw.WriteElementString("Delay", ((int)Delay.TotalMilliseconds).ToString());

				xw.WriteElementString("active_skin", ActiveSkin?.Path ?? "");
				xw.WriteElementString("active_dev_path", GetActiveController()?.DevicePath ?? "");
				
				xw.WriteStartElement("skin_settings");
				foreach (var skin in Skins) {
					xw.WriteStartElement("skin");
					xw.WriteAttributeString("skin_path", skin.Path);
					if (WindowSizes.ContainsKey(skin)) {
						var sz = WindowSizes[skin];
						xw.WriteAttributeString("window_size", $"{sz.Width}x{sz.Height}");
					}

					if (skin is SvgSkin svg) {

						if (SelectedRemaps[svg] is ColorRemap r) {
							xw.WriteAttributeString("active_remap", r.UUID.ToString());
						}

						xw.WriteStartElement("remaps");
						foreach (var remap in Remaps[svg])
							remap.Saveto(xw);
						xw.WriteEndElement(); // remaps
					}
					
					xw.WriteEndElement(); // skin
				}
				xw.WriteEndElement(); // skin_settings

				xw.WriteStartElement("arduino_mapping");
				foreach (var mapEntry in ArduinoMapping) {
					xw.WriteStartElement("map");
					xw.WriteAttributeString("port", mapEntry.Key);
					xw.WriteAttributeString("type", mapEntry.Value.ToString());
					xw.WriteEndElement();
				}
				xw.WriteEndElement(); // arduino_mapping

				xw.WriteEndElement(); // settings
				xw.WriteEndDocument();
				xw.Flush();
				xw.Close();

				File.WriteAllBytes(SettingsFile, ms.ToArray());
				ms.Dispose();
			}
			catch (IOException exc) {
				MessageBox.Show("IO Error while saving: " + exc);
			}
			catch (XmlException exc) {
				MessageBox.Show("XML Error while saving: " + exc);
			}
			catch (Exception exc) {
				MessageBox.Show("Unknown Error while saving: " + exc);
			}
		}

		#endregion

	}

}
