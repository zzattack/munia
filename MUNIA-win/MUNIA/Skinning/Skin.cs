using System.Collections.Generic;
using System.Drawing;
using MUNIA.Controllers;

namespace MUNIA.Skinning {

	public abstract class Skin {
		public string Name { get; set; }
		public string Path { get; set; }
		public SkinLoadResult LoadResult { get; protected set; }
		public List<ControllerType> Controllers { get; } = new List<ControllerType>();

		public abstract void Render(int width, int height, bool force = false);

		public bool UpdateState(IController controller) {
			var oldState = State;
			State = controller?.GetState();
			return !Equals(oldState, State);
		}
		public void UpdateState(ControllerState state) {
			State = state;
		}
		protected ControllerState State;

		public abstract void Activate();
		public abstract void Deactivate();

		public static Skin Clone(Skin skin) {
			if (skin is SvgSkin svg) {
				var clone = new SvgSkin();
				clone.Load(svg.Path);
				return clone;
			}
			else if (skin is NintendoSpySkin nspy) {
				var clone = new NintendoSpySkin();
				clone.Load(nspy.Path);
				return clone;
			}
			else if (skin is PadpyghtSkin ppyght) {
				var clone = new PadpyghtSkin();
				clone.Load(ppyght.Path);
				return clone;
			}

			return null;
		}

		public abstract void GetNumberOfElements(out int numButtons, out int numAxes);

		public abstract bool GetElementsAtLocation(Point location, Size skinSize,
			List<ControllerMapping.Button> buttons, List<ControllerMapping.Axis[]> axes);
	}

	public enum SkinLoadResult {
		Fail,
		Ok,
	}
}
