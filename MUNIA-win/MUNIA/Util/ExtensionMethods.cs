using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>
			(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (TSource element in source) {
				if (seenKeys.Add(keySelector(element))) {
					yield return element;
				}
			}
		}

		public static int GetSequenceHashCode<T>(this IList<T> sequence) {
			const int seed = 487;
			const int modifier = 31;

			unchecked {
				return sequence.Aggregate(seed, (current, item) =>
					(current * modifier) + item.GetHashCode());
			}
		}
	}
}