using System;
using System.Collections.Generic;

namespace MUNIA.Controllers {
	public interface IController {
		ControllerState GetState();
		event EventHandler StateUpdated;

		bool IsActive { get; }
		bool IsAvailable { get; }
		string DevicePath { get; }
		string Name { get; }

		void Activate();
		void Deactivate();
	}
	
}
