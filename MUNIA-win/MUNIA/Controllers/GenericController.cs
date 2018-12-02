using System;
using System.Collections.Generic;
using System.Linq;

namespace MUNIA.Controllers {
	public abstract class GenericController : IController {
		// The controller is the DeviceItem on HidDevice. One HidDevice may expose multiple DeviceItems.

		public abstract string Name { get; }
		public abstract ControllerType Type { get; }
		public abstract bool RequiresPolling { get; }

		public abstract bool Activate();
		public abstract void Deactivate();
		public abstract bool IsAxisTrigger(int axisNum);

		// internal state
		protected readonly List<bool> _buttons = new List<bool>();
		protected readonly List<double> _axes = new List<double>();
		protected readonly List<Hat> _hats = new List<Hat>();
		public virtual ControllerState GetState() => new ControllerState(_axes, _buttons, _hats);
		public event EventHandler StateUpdated;

		public abstract bool IsAvailable { get; }
		public abstract string DevicePath { get; }

		internal static IEnumerable<GenericController> ListDevices() {
			foreach (var controller in RawInputController.ListDevices())
				yield return controller;
			foreach (var controller in XInputController.ListDevices().Where(x => x.IsAvailable))
				yield return controller;
		}

		protected virtual void OnStateUpdated(EventArgs e) {
			StateUpdated?.Invoke(this, e);
		}

		public override string ToString() => Name;
		public abstract void Dispose();
	}
}
