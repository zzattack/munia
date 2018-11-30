using System;
using System.Collections.Generic;
using System.Linq;
using HidSharp;

namespace MUNIA.Controllers {
	public class MappedController : IController {
		private readonly ControllerMapping _mapping;
		private IController _controller;

		public MappedController(ControllerMapping mapping, IController controller) {
			_mapping = mapping;
			_controller = controller;
		}

		public string Name => $"{_controller.Name} -> {_mapping.MappedType}";

		public ControllerType Type => _mapping.MappedType;
		public bool RequiresPolling => _controller.RequiresPolling;
		public bool Activate() {
			return _controller.Activate();
		}

		public void Deactivate() {
			_controller.Deactivate();
		}

		public ControllerState GetState() {
			return _mapping.ApplyMap(_controller.GetState());
		}

		public event EventHandler StateUpdated {
			add => _controller.StateUpdated += value;
			remove => _controller.StateUpdated -= value;
		}
		
		public bool IsAvailable => _controller.IsAvailable;

		public string DevicePath => _controller.DevicePath;

		public new static IEnumerable<MappedController> ListDevices() {
			// first find the generic/raw input controllers
			var controllers = GenericController.ListDevices();
			foreach (var mapping in ConfigManager.ControllerMappings) {
				foreach (var controller in controllers.Where(mapping.AppliesTo)) {
					yield return new MappedController(mapping, controller);
				}
			}
		}

		public bool IsAxisTrigger(int axisNum) {
			var map = _mapping.AxisMaps.FirstOrDefault(a => (int)a.Target == axisNum);
			return map?.IsTrigger == true;
		}
	}
}