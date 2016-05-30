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
		public static string ActiveSkinPath { get; set; }
		public static Color BackgroundColor { get; set; } = Color.Gray;

		public static void Load() {
			try {
				XmlDocument xdoc = new XmlDocument();
				xdoc.Load(settingsFile);
				XmlElement settings = xdoc["settings"] ?? xdoc["root"];

				if (settings["Email"] != null) Email = settings["Email"].InnerText;
				if (settings["BackgroundColor"] != null) BackgroundColor = Color.FromArgb(int.Parse(settings["BackgroundColor"].InnerText));

				var skinSettings = settings["Skins"];
				ActiveSkinPath = skinSettings.Attributes["active"]?.Value;
				foreach (XmlNode skin in skinSettings) {
					string path = skin.Attributes["path"].Value;
					string size = skin.Attributes["size"].Value;
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

				xw.WriteStartElement("Skins");
				if (SkinManager.ActiveSkin != null)
					xw.WriteAttributeString("active", SkinManager.ActiveSkin.Svg.Path);
				foreach (var s in SkinManager.Skins) {
					xw.WriteStartElement("Skin");
					xw.WriteAttributeString("size", $"{s.WindowSize.Width}x{s.WindowSize.Height}");
					xw.WriteAttributeString("path", s.Svg.Path);
					xw.WriteEndElement();
				}
				xw.WriteEndElement(); // Skin

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
