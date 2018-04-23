using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MUNIA.Util {
	public interface IRange {
		bool Contains(int num);
	}
	/// <summary>
	/// Represents a range of integers
	/// </summary
	public class Range : IRange {
		public Range() { }
		public Range(int lowerAndUpper) {
			LowerBound = UpperBound = lowerAndUpper;
		}
		public Range(int lower, int upper) {
			LowerBound = lower;
			UpperBound = upper;
		}

		public int LowerBound { get; set; }
		public int UpperBound { get; set; }

		/// <summary>
		/// Parses a single range. Seperator must be '-', or absent.
		/// </summary>
		public static Range Parse(string range) {
			if (range.Contains("-")) {
				string[] temp = range.Split(new[] { '-' });
				int a = int.Parse(temp[0]);
				int b = int.Parse(temp[1]);
				return new Range(a, b);
			}
			else
				return new Range(int.Parse(range));
		}

		public List<int> Flatten() {
			return Enumerable.Range(LowerBound, UpperBound - LowerBound + 1).ToList();
		}

		public static List<Range> ParseMultiple(string input) {
			return ParseMultiple(input, new char[] { ';', ',', '+' });
		}

		public static List<Range> ParseMultiple(string input, char[] seperators) {
			var ret = new List<Range>();
			if (!string.IsNullOrWhiteSpace(input)) {
				string[] parts = input.Replace(" ", "").Split(seperators);
				ret.AddRange(parts.Select(Parse));
			}
			return ret;
		}

		public override string ToString() {
			if (LowerBound == UpperBound)
				return LowerBound.ToString(CultureInfo.InvariantCulture);
			else
				return string.Format("[{0}-{1}]", LowerBound, UpperBound);
		}

		/// <summary>
		/// Formats a list of ranges to string.
		/// </summary>
		/// <param name="ranges">List of ranges to compress</param>
		/// <param name="seperator">String to separate ranges</param>
		/// <param name="compact">Whether duplicates filtering is performed first</param>
		/// <returns></returns>
		public static List<Range> Compress(IList<int> ranges, bool compact = false, bool sort = false) {
			var rs = new List<Range>();

			if (ranges == null || ranges.Count == 0)
				return rs;

			if (sort)
				ranges = ranges.OrderBy(o => o).ToList();
			if (compact)
				ranges = ranges.Distinct().ToList();

			int rangeFirst = ranges[0];
			int rangePrevious = rangeFirst;
			for (int i = 1; i < ranges.Count(); i++) {
				int x = ranges[i];
				if (x != rangePrevious + 1) {
					// range broken, split here
					rs.Add(new Range(rangeFirst, rangePrevious));
					rangeFirst = x;
				}
				rangePrevious = x;
			}
			// final range still needs to be closed
			rs.Add(new Range(rangeFirst, rangePrevious));

			return rs;
		}

		public virtual bool Contains(int num) {
			return LowerBound <= num && num <= UpperBound;
		}
	}

	public class RangeStrict : Range {
		public RangeStrict() { }
		public RangeStrict(int lower, int upper) : base(lower, upper) { }
		public RangeStrict(int lowerAndUpper) : base(lowerAndUpper) { }
		public override bool Contains(int num) {
			return LowerBound <= num && num < UpperBound;
		}

		public override string ToString() {
			if (LowerBound >= UpperBound)
				return "Empty range";
			else
				return string.Format("[{0}-{1})", LowerBound, UpperBound);
		}
	}

	public class RangeUnion : Range {
		readonly List<Range> _ranges = new List<Range>();
		public RangeUnion(IEnumerable<Range> ranges) { 
			_ranges.AddRange(ranges);
		}
		public List<Range> Ranges { get { return _ranges; } } 
		public override bool Contains(int num) {
			return _ranges.Any(r => r.Contains(num));
		}
		public override string ToString() {
			return string.Join(" + ", _ranges.Select(r=>r.ToString()));
		}
	}

	public static partial class ExtensionMethods {
		/// <summary>
		/// Flattens a range of lists. Flatten(new[] {new Range(0,3), new Range(5,6)}) = {0,1,2,3,5,6}.
		/// </summary>
		public static List<int> Flatten(this IEnumerable<Range> ranges) {
			var ret = new List<int>();
			foreach (var r in ranges)
				ret.AddRange(r.Flatten());
			return ret;
		}
		/// <summary>
		/// Flattens a range of lists. Flatten(new[] {new Range(0,3), new Range(5,6)}) = {0,1,2,3,5,6}.
		/// </summary>
		public static string Format(this IEnumerable<Range> ranges, string seperator = ", ") {
			if (ranges == null || !ranges.Any())
				return "";
			return String.Join(seperator, ranges.Select(r => String.Format(
				r.LowerBound != r.UpperBound ? "{0}-{1}" : "{0}", r.LowerBound, r.UpperBound)));
		}

		public static List<Range> Compact(this IList<Range> ranges) { 
			return Range.Compress(ranges.Flatten(), true, true);
		} 

		public static bool ContainsFast(this IEnumerable<Range> ranges, int num) {
			return ranges.Any(r => r.Contains(num));
		}
	}
}
