using System;
using System.Collections.Generic;
using System.Drawing;

namespace MUNIA.Util {
	static partial class ExtensionMethods {
		public static void EnsureSize<T>(this List<T> list, int count) {
			while (list.Count < count) list.Add(Activator.CreateInstance<T>());
		}

		public static string ToHexValue(this Color color) {
			return "#" + color.R.ToString("X2") +
					color.G.ToString("X2") +
					color.B.ToString("X2");
		}

	}
}