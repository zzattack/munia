using System;
using System.Collections.Generic;
using MUNIA.Controllers;

namespace MUNIA.Skins {

	public abstract class Skin {
		public string Name { get; set; }
		public string Path { get; set; }
		public SkinLoadResult LoadResult { get; protected set; }
		public List<ControllerType> Controllers { get; } = new List<ControllerType>();

		public abstract void Render(int width, int height);

		public void UpdateState(IController controller) {
			State = controller?.GetState();
		}
		protected ControllerState State;
	}


	public enum SkinLoadResult {
		Fail,
		Ok,
	}

	public enum ControllerType {
		SNES,
		N64,
		NGC,
		Unknown
	}

}
