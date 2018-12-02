using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using MUNIA.Interop;

namespace MUNIA.Controllers {
	public abstract class ArduinoController : IController {
		protected ArduinoController(SerialPortInfo port) {
			PortInfo = port;
			Port = new SerialPort(port.Name, 115200);
			Port.DataReceived += OnDataReceived;
		}
		
		protected List<double> _axes { get; } = new List<double>();
		protected List<bool> _buttons { get; } = new List<bool>();
		protected List<Hat> _hats { get; } = new List<Hat>();

		public bool IsAvailable { get; }
		public string DevicePath => PortInfo.Name;

		public string Name => "Arduino " + Type;
		public abstract ControllerType Type { get; }
		public bool RequiresPolling => false;

		public SerialPortInfo PortInfo { get; set; }
		protected SerialPort Port;

		public ControllerState GetState() => new ControllerState(_axes, _buttons, _hats);
		public abstract bool IsAxisTrigger(int axisNum);

		public bool Activate() {
			try {
				Port?.Open();
				return true;
			}
			catch {
				return false;
			}
		}

		public void Deactivate() {
			Port?.Close();
		}


		private void OnDataReceived(object sender, SerialDataReceivedEventArgs args) {
			byte[] buff = new byte[512];
			while (Port.BytesToRead > 0) {
				int br = Port.Read(buff, 0, 512);
				AddToBuffer(buff, 0, br);
			}
		}

		private readonly List<byte> _buffer = new List<byte>();
		private void AddToBuffer(byte[] buff, int offset, int count) {
			for (int i = offset; i < offset + count; i++) {
				byte b = buff[i];
				if (b == '\n') {
					if (Parse(_buffer))
						OnStateUpdated();
					_buffer.Clear();
				}
				else _buffer.Add(b);
			}
		}

		public List<byte> Repack(List<byte> data) {
			// repack data bytes
			// can't believe the firmware is implemented like this... but oh well
			List<byte> packet = new List<byte>();

			byte x = 0x80, y = 0;
			foreach (byte b in _buffer) {
				if (b == '1') y |= x;
				x >>= 1;
				if (x == 0) {
					packet.Add(y);
					x = 0x80;
					y = 0;
				}
			}
			return packet;
		}
		protected abstract bool Parse(List<byte> buffer);
		public event EventHandler StateUpdated;
		protected virtual void OnStateUpdated() { StateUpdated?.Invoke(this, EventArgs.Empty); }

		public static List<ArduinoController> ListDevices() {
			var ret = new List<ArduinoController>();
			var ports = SerialPortInfo.GetPorts();
			foreach (var entry in ConfigManager.ArduinoMapping) {
				var port = ports.FirstOrDefault(p => p.Name == entry.Key && p.IsConnected);
				if (port != null) {
					ret.Add(CreateDevice(port, entry.Value));
				}
			}
			return ret;
		}

		public static ArduinoController CreateDevice(SerialPortInfo port, ControllerType type) {
			switch (type) {
			case ControllerType.SNES:
				return new ArduinoSnes(port);
			case ControllerType.N64:
				return new ArduinoN64(port);
			case ControllerType.NGC:
				return new ArduinoNgc(port);
			}
			return null;
		}

		public void Dispose() {
			Port?.Dispose();
			Port = null;
		}
	}
}
