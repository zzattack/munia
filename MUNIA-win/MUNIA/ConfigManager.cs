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
		private static readonly string RemapsFile = Path.Combine(SettingsDir, "munia_remaps.xml");
		public static string Email { get; set; } = "nobody@nothing.net";
		public static Color BackgroundColor { get; set; } = Color.Gray;
		public static Dictionary<Skin, Size> WindowSizes { get; } = new Dictionary<Skin, Size>();
		public static Dictionary<SvgSkin, ColorRemap> SelectedRemaps { get; } = new Dictionary<SvgSkin, ColorRemap>();
		public static Dictionary<NintendoSpySkin, string> SelectedNSpyBackgrounds { get; } = new Dictionary<NintendoSpySkin, string>();
		public static Dictionary<string, BindingList<ColorRemap>> AvailableRemaps { get; }
			= new Dictionary<string, BindingList<ColorRemap>>();
		public static readonly ArduinoMapping ArduinoMapping = new ArduinoMapping();

		public static BindingList<SkinDirectoryEntry> SkinFolders { get; private set; } = new BindingList<SkinDirectoryEntry>();
		public static BindingList<ControllerMapping> ControllerMappings { get; private set; } = new BindingList<ControllerMapping>();

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
			foreach (var dir in SkinFolders) {
				if (!Directory.Exists(dir.Path))
					continue;

				if ((dir.Types & SkinType.Svg) != 0) {
					foreach (string svgPath in Directory.GetFiles(dir.Path, "*.svg")) {
						var svg = new SvgSkin();
						svg.Load(svgPath);
						if (svg.LoadResult == SkinLoadResult.Ok) {
							Skins.Add(svg);
							var b = new BindingList<ColorRemap>();
							foreach (var map in svg.EmbeddedRemaps)
								b.Add(map);
							AvailableRemaps[svg.Path] = b;
							SelectedRemaps[svg] = null;
						}
					}
				}

				if ((dir.Types & SkinType.PadPyght) != 0) {
					foreach (string padpyghtDir in Directory.GetDirectories(dir.Path)) {
						foreach (string iniPath in Directory.GetFiles(padpyghtDir, "*.ini")) {
							var pp = new PadpyghtSkin();
							pp.Load(iniPath);
							if (pp.LoadResult == SkinLoadResult.Ok)
								Skins.Add(pp);
						}
					}
				}

				if ((dir.Types & SkinType.NintendoSpy) != 0) {
					foreach (string nspyDir in Directory.GetDirectories(dir.Path)) {
						foreach (string xmlPath in Directory.GetFiles(nspyDir, "skin.xml")) {
							var pp = new NintendoSpySkin();
							pp.Load(xmlPath);
							if (pp.LoadResult == SkinLoadResult.Ok)
								Skins.Add(pp);
						}
					}
				}
			}
		}

		public static void LoadControllers() {
			Controllers.Clear();
			foreach (var dev in MuniaController.ListDevices()) {
				Controllers.Add(dev);
			}
			foreach (var dev in ArduinoController.ListDevices()) {
				Controllers.Add(dev);
			}
			foreach (var dev in XInputController.ListDevices()) {
				Controllers.Add(dev);
			}
			foreach (var dev in MappedController.ListDevices()) {
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

				if (xroot["SkinFolders"] is XmlNode xSkinFolders) {
					foreach (XmlNode xDir in xSkinFolders) {
						var sf = new SkinDirectoryEntry {
							Path = xDir.Attributes["path"].Value,
							Types = (SkinType)int.Parse(xDir.Attributes["types"].Value)
						};
						// setup skin folders to default svg only
						SkinFolders.Add(sf);
					}
				}
			}
			catch (XmlException) { }
			catch (FormatException) { }

			if (!SkinFolders.Any()) {
				// setup skin folders to default svg only
				SkinFolders.Add(new SkinDirectoryEntry { Path = "./skins", Types = SkinType.Svg });
			}

			// then load all skins as they have no dependencies
			LoadSkins();
			LoadRemaps();

			try {
				// now we can load the skin-specific settings
				if (xroot["ActiveSkin"] != null)
					ActiveSkin = Skins.FirstOrDefault(s => s.Path == xroot["ActiveSkin"].InnerText);

				foreach (XmlNode skinCfg in xroot["SkinSettings"].ChildNodes) {
					string path = skinCfg.Attributes["path"].Value;

					var skin = Skins.FirstOrDefault(s => s.Path == path);
					var wsz = skinCfg.Attributes["WindowSize"];
					if (wsz != null && skin != null) {
						string size = wsz.Value;
						Size sz = new Size(int.Parse(size.Substring(0, size.IndexOf("x"))), int.Parse(size.Substring(size.IndexOf("x") + 1)));
						WindowSizes[skin] = sz;
					}

					if (skin is SvgSkin svg && skinCfg.Attributes["ActiveRemap"] != null) {
						Guid uuid = Guid.Parse(skinCfg.Attributes["ActiveRemap"].Value);

						var remapList = AvailableRemaps.Where(kvp => kvp.Value.Any(cr => cr.UUID == uuid));
						if (remapList.Any()) {
							var skinList = remapList.First().Value;
							SelectedRemaps[svg] = skinList.First(r => r.UUID == uuid);
						}
					}

					else if (skin is NintendoSpySkin nspySkin && skinCfg.Attributes["ActiveBackground"] != null) {
						SelectedNSpyBackgrounds[nspySkin] = skinCfg.Attributes["ActiveBackground"].Value;
					}
				}

				// load arduino mappings
				var arduinoMap = xroot["ArduinoMapping"];
				if (arduinoMap != null) {
					foreach (XmlNode e in arduinoMap.ChildNodes) {
						ArduinoMapping[e.Attributes["port"].Value] =
							(ControllerType)Enum.Parse(typeof(ControllerType), e.Attributes["type"].Value, true);
					}
				}

				// load generic controller mappings
				var mappings = xroot["ControllerMappings"];
				foreach (var map in BuiltInMappings.Get())
					ControllerMappings.Add(map);
				if (mappings != null) {
					foreach (XmlNode m in mappings.ChildNodes) {
						var mapping = new ControllerMapping(m);
						ControllerMappings.Add(mapping);
					}
				}
			}
			catch { }

			LoadControllers();

			try {
				// finally we can determine the active controller
				if (xroot["ActiveDevice"] is XmlNode xActiveDev) {
					SetActiveControllerFromDevicePath(xActiveDev.InnerText);
				}
			}
			catch { }
		}

		public static void SetActiveControllerFromDevicePath(string devicePath) {
			if (devicePath.StartsWith("MAP$")) {
				string guid = devicePath.Substring(4, devicePath.IndexOf('$', 5) - 4);
				if (Guid.TryParse(guid, out Guid uuid)) {
					var mapping = ControllerMappings.FirstOrDefault(m => m.UUID == uuid);
					if (mapping != null) {
						string devPath = devicePath.Substring(4 + guid.Length + 1);
						var controller = Controllers.FirstOrDefault(c => c.DevicePath == devPath);
						if (controller != null)
							SetActiveController(new MappedController(mapping, controller));
					}
				}
			}
			else {
				SetActiveController(Controllers.FirstOrDefault(c => c.DevicePath == devicePath));
			}
		}

		private static void LoadRemaps() {
			try {
				XmlDocument xdoc = new XmlDocument();
				xdoc.Load(RemapsFile);
				foreach (XmlNode xSkin in xdoc["SkinRemaps"]) {
					string path = xSkin.Attributes["path"].Value;
					if (!AvailableRemaps.ContainsKey(path))
						AvailableRemaps[path] = new BindingList<ColorRemap>();
					var remaps = AvailableRemaps[path];
					foreach (XmlNode xRemap in xSkin) {
						remaps.Add(ColorRemap.LoadFrom(xRemap));
					}
				}
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

				xw.WriteStartElement("SkinFolders");
				foreach (var sf in SkinFolders) {
					xw.WriteStartElement("folder");
					xw.WriteAttributeString("path", sf.Path);
					xw.WriteAttributeString("types", ((int)sf.Types).ToString());
					xw.WriteEndElement();
				}
				xw.WriteEndElement(); // SkinFolders

				xw.WriteElementString("ActiveSkin", ActiveSkin?.Path ?? "");
				xw.WriteElementString("ActiveDevice", GetActiveController()?.DevicePath ?? "");

				xw.WriteStartElement("SkinSettings");
				foreach (var skin in Skins) {
					xw.WriteStartElement("skin");
					xw.WriteAttributeString("path", skin.Path);
					if (WindowSizes.ContainsKey(skin)) {
						var sz = WindowSizes[skin];
						xw.WriteAttributeString("WindowSize", $"{sz.Width}x{sz.Height}");
					}

					if (skin is SvgSkin svg && SelectedRemaps[svg] is ColorRemap r) {
						xw.WriteAttributeString("ActiveRemap", r.UUID.ToString());
					}
					else if (skin is NintendoSpySkin nspySkin && nspySkin.SelectedBackground != null) {
						xw.WriteAttributeString("ActiveBackground", nspySkin.SelectedBackground.Name);
					}

					xw.WriteEndElement(); // skin
				}
				xw.WriteEndElement(); // skin_settings

				xw.WriteStartElement("ArduinoMapping");
				foreach (var mapEntry in ArduinoMapping) {
					xw.WriteStartElement("map");
					xw.WriteAttributeString("port", mapEntry.Key);
					xw.WriteAttributeString("type", mapEntry.Value.ToString());
					xw.WriteEndElement();
				}
				xw.WriteEndElement(); // ArduinoMapping

				xw.WriteStartElement("ControllerMappings");
				foreach (var mapEntry in ControllerMappings.Where(e => !e.IsBuiltIn)) {
					xw.WriteStartElement("mapping");
					mapEntry.SaveTo(xw);
					xw.WriteEndElement();
				}
				xw.WriteEndElement(); // ControllerMappings

				xw.WriteEndElement(); // settings
				xw.WriteEndDocument();
				xw.Flush();
				xw.Close();

				File.WriteAllBytes(SettingsFile, ms.ToArray());
				ms.Dispose();

				SaveRemaps();
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

		public static void SaveRemaps() {
			try {
				Directory.CreateDirectory(SettingsDir);
				var ms = new MemoryStream();
				var xw = new XmlTextWriter(ms, Encoding.UTF8);
				xw.Formatting = Formatting.Indented;
				xw.WriteStartDocument();
				xw.WriteStartElement("SkinRemaps");

				foreach (var kvp in AvailableRemaps) {
					xw.WriteStartElement("skin");
					xw.WriteAttributeString("path", kvp.Key);

					foreach (var remap in kvp.Value)
						if (!remap.IsSkinDefault)
							remap.SaveTo(xw);
					xw.WriteEndElement(); // skin
				}

				xw.WriteEndElement(); // skin_remaps
				xw.WriteEndDocument();

				xw.Flush();
				xw.Close();
				File.WriteAllBytes(RemapsFile, ms.ToArray());
				ms.Dispose();
			}
			catch { }
		}

		#endregion

	}

}
