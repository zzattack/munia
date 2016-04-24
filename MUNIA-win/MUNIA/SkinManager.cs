using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MUNIA.Controllers;

namespace MUNIA {
	public static class SkinManager {
		public static readonly HashSet<SkinEntry> Skins = new HashSet<SkinEntry>();
		public static SkinEntry ActiveSkin { get; set; }

		public static IEnumerable<SkinEntry> Load() {
			foreach (string svgPath in Directory.GetFiles("./skins", "*.svg")) {
				var controller = new SvgController();
				var loadResult = controller.Load(svgPath);
				var entry = Skins.FirstOrDefault(s => s.Controller.SvgPath == svgPath) ?? new SkinEntry();
				entry.Controller = controller;
				entry.LoadResult = loadResult;
				Skins.Add(entry);
			}
			return Skins;
		}
	}

	public class SkinEntry {
		public Size WindowSize;
		public SvgController Controller;
		public SvgController.LoadResult LoadResult;
	}
}
