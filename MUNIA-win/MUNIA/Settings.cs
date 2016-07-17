using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace MUNIA {
	public static class Settings {
		private static readonly string settingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MUNIA");
		private static readonly string settingsFile = Path.Combine(settingsDir, "munia_settings.xml");

		public static Dictionary<string, Size> SkinWindowSizes = new Dictionary<string, Size>();
		public static string Email { get; set; } = "nobody@nothing.net";
		public static string ActiveConfigPath { get; set; }
		public static Color BackgroundColor { get; set; } = Color.Gray;

		public static void Load() {
			try {
				XmlDocument xdoc = new XmlDocument();
				xdoc.Load(settingsFile);
				XmlElement xroot = xdoc["settings"] ?? xdoc["root"];

				if (xroot["Email"] != null) Email = xroot["Email"].InnerText;
				if (xroot["BackgroundColor"] != null) BackgroundColor = Color.FromArgb(int.Parse(xroot["BackgroundColor"].InnerText));

				var configs = xroot["Configs"];
				ActiveConfigPath = configs.Attributes["active"]?.Value;
				foreach (XmlNode cfg in configs) {
					string path = cfg.Attributes["skin_path"].Value;
					string size = cfg.Attributes["window_size"].Value;
					string devPath = cfg.Attributes["dev_path"].Value;
					Size s = new Size(int.Parse(size.Substring(0, size.IndexOf("x"))), int.Parse(size.Substring(size.IndexOf("x") + 1)));
					SkinWindowSizes[path] = s;
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

				if (ConfigManager.ActiveConfig != null) { 
					xw.WriteStartElement("ActiveConfig");
					xw.WriteAttributeString("skin_path", ConfigManager.ActiveConfig.Skin?.Path ?? "");
					xw.WriteAttributeString("dev_path", ConfigManager.ActiveConfig.Controller?.DevicePath ?? "");
					xw.WriteEndElement();
				}
				// todo save windowsizes per skin

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
