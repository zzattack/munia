using System;
using System.Collections.Generic;
using System.Linq;
using MUNIA.Util;

namespace MUNIA.Controllers {
	public interface IController : IDisposable {
		ControllerState GetState();
		event EventHandler StateUpdated;

		bool IsAvailable { get; }
		string DevicePath { get; }
		string Name { get; }
		ControllerType Type { get; }
		bool RequiresPolling { get; }

		bool Activate();
		void Deactivate();

		bool IsAxisTrigger(int axisNum);
	}

	[Flags]
	public enum Hat {
		None = 0,
		Up = 1,
		Down = 2,
		Left = 4,
		Right = 8,
		Invalid = 15
	}

	public class ControllerState {
		public ControllerState() : this(new List<double>(), new List<bool>(), new List<Hat>()) { }
		public ControllerState(List<double> axes, List<bool> buttons, List<Hat> hats) {
			Axes.AddRange(axes);
			Buttons.AddRange(buttons);
			Hats.AddRange(hats);
		}

		public List<double> Axes { get; } = new List<double>();
		public List<bool> Buttons { get; } = new List<bool>();
		public List<Hat> Hats { get; } = new List<Hat>(); // usually copied to buttons for simplicity

		public static Dictionary<byte, Hat> HatLookup = new Dictionary<byte, Hat> {
			{ HAT_SWITCH_NORTH, Hat.Up },
			{ HAT_SWITCH_NORTH_EAST, Hat.Up | Hat.Right},
			{ HAT_SWITCH_EAST, Hat.Right },
			{ HAT_SWITCH_SOUTH_EAST, Hat.Down | Hat.Right },
			{ HAT_SWITCH_SOUTH, Hat.Down },
			{ HAT_SWITCH_SOUTH_WEST, Hat.Down | Hat.Left },
			{ HAT_SWITCH_WEST, Hat.Left },
			{ HAT_SWITCH_NORTH_WEST, Hat.Left | Hat.Up },
			{ HAT_SWITCH_NULL, Hat.None },
			{ 9, Hat.Invalid },
			{ 10, Hat.Invalid },
			{ 11, Hat.Invalid },
			{ 12, Hat.Invalid },
			{ 13, Hat.Invalid },
			{ 14, Hat.Invalid },
			{ 15, Hat.Invalid },
		};

		// microsoft-chosen values
		private const byte HAT_SWITCH_NORTH = 0x0;
		private const byte HAT_SWITCH_NORTH_EAST = 0x1;
		private const byte HAT_SWITCH_EAST = 0x2;
		private const byte HAT_SWITCH_SOUTH_EAST = 0x3;
		private const byte HAT_SWITCH_SOUTH = 0x4;
		private const byte HAT_SWITCH_SOUTH_WEST = 0x5;
		private const byte HAT_SWITCH_WEST = 0x6;
		private const byte HAT_SWITCH_NORTH_WEST = 0x7;
		private const byte HAT_SWITCH_NULL = 0x8;
		
		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((ControllerState) obj);
		}

		public override int GetHashCode() {
			unchecked {
				var hashCode = Axes.GetSequenceHashCode();
				hashCode = (hashCode*397) ^ Buttons.GetSequenceHashCode();
				hashCode = (hashCode*397) ^ Hats.GetSequenceHashCode();
				return hashCode;
			}
		}

		protected bool Equals(ControllerState other) {
			return Axes.SequenceEqual(other.Axes) && Buttons.SequenceEqual(other.Buttons) && Hats.SequenceEqual(other.Hats);
		}


	}
}

