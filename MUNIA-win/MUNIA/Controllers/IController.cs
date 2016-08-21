using System;

namespace MUNIA.Controllers {
	public interface IController {
		ControllerState GetState();
		event EventHandler StateUpdated;

		bool IsActive { get; }
		bool IsAvailable { get; }
		string DevicePath { get; }
		string Name { get; }
		ControllerType Type { get; }

		bool Activate();
		void Deactivate();
	}
	
}
