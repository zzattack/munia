using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MUNIA.Controllers;
using MUNIA.Skins;

namespace MUNIA {
	public static class ConfigManager {
		public static List<IController> Controllers { get; } = new List<IController>();
		public static List<Skin> Skins { get; } = new List<Skin>();
		public static Dictionary<Skin, Size> WindowSizes { get; } = new Dictionary<Skin, Size>();
		private static TimeSpan _delay;

		public static TimeSpan Delay {
			get { return _delay; }
			set {
				_delay = value;
				if (_delay != TimeSpan.Zero) {
					_bufferedActiveController?.Deactivate();
					_bufferedActiveController = new BufferedController(_activeController) {Delay = value};
				}
			}
		}

		private static IController _activeController;
		private static BufferedController _bufferedActiveController;

		public static void SetActiveController(IController value) {
			// deactive old controller
			_activeController?.Deactivate();
			_bufferedActiveController?.Deactivate();

			_activeController = value;
			if (Delay != TimeSpan.Zero)
				_bufferedActiveController = new BufferedController(value) { Delay = Delay };
		}

		public static IController GetActiveController() {
			return Delay == TimeSpan.Zero ? _activeController : _bufferedActiveController;
		}

		public static Skin ActiveSkin { get; set; }


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

		public static void LoadControllers() {
			Controllers.Clear();
			foreach (var dev in MuniaController.ListDevices()) {
				Controllers.Add(dev);
			}
			foreach (var dev in ArduinoControllerManager.ListDevices()) {
				Controllers.Add(dev);
			}
		}


	}
	

}
