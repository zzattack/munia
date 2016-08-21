using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace MUNIA.Controllers {
	public abstract class ArduinoController : IController {

		private List<int> Axes { get; }
		private List<bool> Buttons { get; }
		public bool IsActive { get; set; }
		public bool IsAvailable { get; }
		public string DevicePath { get; set; }
		public string Name { get; set; }
		public abstract ControllerType Type { get; }

		public ControllerState GetState() => new ControllerState(Axes, Buttons);
		
		public event EventHandler StateUpdated;
		public SerialPort Port;
		public void Activate() {
			// port open
		}

		public void Deactivate() {
			// port close
		}
	}

	public class ArduinoControllerManager {
		public static List<ArduinoController> ListDevices() {
			return new List<ArduinoController>();
		}
	}

	public class ArduinoSnesController : ArduinoController {
		public override ControllerType Type => ControllerType.SNES;
	}
	public class ArduinoN64Controller : ArduinoController {
		public override ControllerType Type => ControllerType.N64;
	}
	public class ArduinoNgcController : ArduinoController {
		public override ControllerType Type => ControllerType.NGC;
	}
}
