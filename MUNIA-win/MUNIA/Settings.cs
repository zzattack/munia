using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace MUNIA {
	public static class Settings {
		private static readonly string settingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MUNIA");
		private static readonly string settingsFile = Path.Combine(settingsDir, "munia_settings.xml");

		public static string Email { get; set; } = "nobody@nothing.net";
		public static Color BackgroundColor { get; set; } = Color.Gray;

		public static void Load() {
			try {
				XmlDocument xdoc = new XmlDocument();
				xdoc.Load(settingsFile);
				XmlElement xroot = xdoc["settings"] ?? xdoc["root"];

				if (xroot["Email"] != null) Email = xroot["Email"].InnerText;
				if (xroot["BackgroundColor"] != null) BackgroundColor = Color.FromArgb(int.Parse(xroot["BackgroundColor"].InnerText));

				if (xroot["active_skin"] != null)
					ConfigManager.ActiveSkin = ConfigManager.Skins
						.FirstOrDefault(s => s.Path == xroot["active_skin"].InnerText);

				if (xroot["active_dev_path"] != null)
					ConfigManager.SetActiveController(ConfigManager.Controllers
						.FirstOrDefault(s => s.DevicePath == xroot["active_dev_path"].InnerText));

				foreach (XmlNode skinCfg in xroot["skin_settings"].ChildNodes) {
					string path = skinCfg.Attributes["skin_path"].Value;

					var wsz = skinCfg.Attributes["window_size"];
					if (wsz != null) {
						string size = wsz.Value;
						Size sz = new Size(int.Parse(size.Substring(0, size.IndexOf("x"))), int.Parse(size.Substring(size.IndexOf("x") + 1)));
						var skin = ConfigManager.Skins.FirstOrDefault(s => s.Path == path);
						if (skin != null)
							ConfigManager.WindowSizes[skin] = sz;
					}

				}
			}
			catch { }
		}

		public static void Save() {
			try {
				Directory.CreateDirectory(settingsDir);
				var ms = new MemoryStream();
				var xw = new XmlTextWriter(ms, Encoding.UTF8);
				xw.Formatting = Formatting.Indented;
				xw.WriteStartDocument();
				xw.WriteStartElement("settings");

				xw.WriteElementString("BackgroundColor", BackgroundColor.ToArgb().ToString());
				xw.WriteElementString("Email", Email);

				xw.WriteElementString("active_skin", ConfigManager.ActiveSkin?.Path ?? "");
				xw.WriteElementString("active_dev_path", ConfigManager.GetActiveController()?.DevicePath ?? "");

				xw.WriteStartElement("skin_settings");
				foreach (var skin in ConfigManager.Skins) {
					xw.WriteStartElement("skin");
					xw.WriteAttributeString("skin_path", skin.Path);
					if (ConfigManager.WindowSizes.ContainsKey(skin)) {
						var sz = ConfigManager.WindowSizes[skin];
						xw.WriteAttributeString("window_size", $"{sz.Width}x{sz.Height}");
					}
					xw.WriteEndElement();
				}
				xw.WriteEndElement();

				xw.WriteEndElement(); // settings
				xw.WriteEndDocument();
				xw.Flush();
				xw.Close();

				File.WriteAllBytes(settingsFile, ms.ToArray());
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
	}
}
