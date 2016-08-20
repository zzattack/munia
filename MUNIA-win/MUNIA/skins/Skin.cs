using System;
using MUNIA.Controllers;

namespace MUNIA.Skins {

	public abstract class Skin {
		public string Path { get; set; }
		public SkinLoadResult LoadResult { get; protected set; }
		public string ControllerName { get; set; }
		public string SkinName { get; set; }

		public abstract void Render(int width, int height);

		public void UpdateState(IController controller) {
			State = controller.GetState();
		}
		protected ControllerState State;
	}


	public enum SkinLoadResult {
		Fail,
		Ok,
	}

}
