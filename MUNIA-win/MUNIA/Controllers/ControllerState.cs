using System;
using System.Collections.Generic;

namespace MUNIA.Controllers {
	public interface IControllerState {
		List<int> Axes { get; }
		List<bool> Buttons { get; }
		event EventHandler StateUpdated;
	}
}
