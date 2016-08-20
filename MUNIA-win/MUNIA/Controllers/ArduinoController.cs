using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace MUNIA.Controllers {
	public abstract class ArduinoController : IController {
		public List<int> Axes { get; }
		public List<bool> Buttons { get; }
		public bool IsActive { get; set; }
		public bool IsAvailable { get; }
		public string DevicePath { get; set; }
		public string Name { get; set; }
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

	public class ArduinoSnesController : ArduinoController { }
	public class ArduinoN64Controller : ArduinoController { }
	public class ArduinoNGCController : ArduinoController { }
}
