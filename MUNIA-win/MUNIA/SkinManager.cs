using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MUNIA.Controllers;

namespace MUNIA {
	public static class SkinManager {
		public static readonly HashSet<Skin> Skins = new HashSet<Skin>();
		public static Skin ActiveSkin { get; set; }

		public static IEnumerable<Skin> Load() {
			foreach (string svgPath in Directory.GetFiles("./skins", "*.svg")) {
				var svg = new SvgController();
				svg.LoadSvg(svgPath);
				var entry = Skins.FirstOrDefault(s => s.Svg.Path == svgPath) ?? new Skin();
				entry.Svg = svg;
				if (!Skins.Contains(entry))
					Skins.Add(entry);
			}

			return Skins;
		}

		public static void UpdateInputDevices() {
			// update loadresult on existing skins
			foreach (var skin in Skins) {
				if (skin.Svg.LoadResult >= SvgController.SvgLoadResult.SvgOk) {
					skin.Controller = MuniaController.GetByName(skin.Svg.DeviceName);
				}
			}
		}

	}

	public class Skin {
		public SvgController Svg;
		public MuniaController Controller;
		public Size WindowSize;
	}
}
