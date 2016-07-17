using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MUNIA.Controllers;
using MUNIA.Skins;

namespace MUNIA {
	public static class ConfigManager {
		public static readonly HashSet<Config> Configs = new HashSet<Config>();
		public static Config ActiveConfig { get; set; }
		public static List<IController> Controllers { get; } = new List<IController>();
		public static List<Skin> Skins { get; } = new List<Skin>();

		public static void LoadSkins() {
			foreach (string svgPath in Directory.GetFiles("./skins", "*.svg")) {
				var svg = new SvgSkin();
				svg.Load(svgPath);
				Skins.Add(svg);
			}
			foreach (string padpyghtDir in Directory.GetDirectories("./skins")) {
				// todo
			}
		}

		public static void UpdateInputDevices() {
			Controllers.Clear();
			foreach (var dev in MuniaController.ListDevices()) {
				Controllers.Add(dev);
			}
			foreach (var dev in ArduinoControllerManager.ListDevices()) {
				Controllers.Add(dev);
			}
		}


	}

	public class Config {
		public Skin Skin;
		public IController Controller;
		public Size WindowSize;
	}

}
