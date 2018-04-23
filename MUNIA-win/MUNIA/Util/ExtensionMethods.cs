using System;
using System.Collections.Generic;

namespace MUNIA.Util {
	static partial class ExtensionMethods {
		public static void EnsureSize<T>(this List<T> list, int count) {
			while (list.Count < count) list.Add(Activator.CreateInstance<T>());
		}
	}
}