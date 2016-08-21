using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using HidSharp;

namespace MUNIA.Controllers {
    public abstract class MuniaController : IController, IDisposable {
        /// <summary>
        /// Can be used to register for WM_INPUT messages and parse them.
        /// For testing purposes it can also be used to solely register for WM_INPUT messages.
        /// </summary>

        public HidDevice HidDevice;
		private HidStream _stream;
		public string DevicePath => HidDevice.DevicePath;
		public string Name => HidDevice.ProductName;
	    public abstract ControllerType Type { get; }

	    protected abstract List<int> Axes { get; }
		protected abstract List<bool> Buttons { get; }
	    public ControllerState GetState() => new ControllerState(Axes, Buttons);

	    public event EventHandler StateUpdated;

		protected MuniaController(HidDevice hidDevice) {
            this.HidDevice = hidDevice;
        }
        public override string ToString() {
            return HidDevice.ProductName;
        }
        
        public static IEnumerable<MuniaController> ListDevices() {
			var loader = new HidDeviceLoader();
            foreach (var device in loader.GetDevices(0x04d8, 0x0058)) {
                if (device.ProductName == "NinHID NGC") {
                    yield return new NgcController(device);
                }
                else if (device.ProductName == "NinHID N64") {
                    yield return new N64Controller(device);
				}
				else if (device.ProductName == "NinHID SNES") {
					yield return new SnesController(device);
				}
			}
		}
		public static MuniaController GetByName(string deviceName) {
			return ListDevices().FirstOrDefault(c => c.HidDevice.ProductName == deviceName);
		}

		public static HidDevice GetConfigInterface() {
			var loader = new HidDeviceLoader();
			// todo: use foreach/window instead of firstordefault
		    return loader.GetDevices(0x04d8, 0x0058).FirstOrDefault(device => device.ProductName == "NinHID CFG");
	    }


		/// <summary>
		/// Registers device for receiving inputs.
		/// </summary>
		public void Activate() {
			_stream?.Dispose();
			_stream = HidDevice.Open();
			_stream.ReadTimeout = Timeout.Infinite;

			byte[] buffer = new byte[HidDevice.MaxInputReportLength];
			_stream.BeginRead(buffer, 0, buffer.Length, Callback, buffer);
		}

		public void Deactivate() {
			_stream?.Close();
		}
		public bool IsActive => _stream != null && _stream.CanRead;
		public bool IsAvailable { 
			get {
				HidStream s;
				var ret = HidDevice.TryOpen(out s);
				if (!ret) return false;
				ret = s.CanRead;
				s.Close();
				return ret;
			}
		}
		
	    private void Callback(IAsyncResult ar) {
			try {
				if (_stream == null) return;
				int numBytes = _stream.EndRead(ar);
				byte[] buffer = (byte[])ar.AsyncState;
				if (numBytes > 0) {
					if (Parse(buffer))
						StateUpdated?.Invoke(this, EventArgs.Empty);
					_stream.BeginRead(buffer, 0, buffer.Length, Callback, buffer);
				}
			}
			catch (IOException exc) {
				Debug.WriteLine("IOException: " + exc.Message);
			}
			catch (NullReferenceException) { }
		}

	    public void Dispose() {
			_stream?.Dispose();
        }
		
		protected abstract bool Parse(byte[] ev);
		
        protected Dictionary<byte, byte> HatLookup = new Dictionary<byte, byte> {
            { HAT_SWITCH_NORTH, 8 },
            { HAT_SWITCH_NORTH_EAST, 9 },
            { HAT_SWITCH_EAST, 1 },
            { HAT_SWITCH_SOUTH_EAST, 5 },
            { HAT_SWITCH_SOUTH, 4 },
            { HAT_SWITCH_SOUTH_WEST, 6 },
            { HAT_SWITCH_WEST, 2 },
            { HAT_SWITCH_NORTH_WEST, 10 },
            { HAT_SWITCH_NULL, 0 },
        };

        private const byte HAT_SWITCH_NORTH = 0x0;
        private const byte HAT_SWITCH_NORTH_EAST = 0x1;
        private const byte HAT_SWITCH_EAST = 0x2;
        private const byte HAT_SWITCH_SOUTH_EAST = 0x3;
        private const byte HAT_SWITCH_SOUTH = 0x4;
        private const byte HAT_SWITCH_SOUTH_WEST = 0x5;
        private const byte HAT_SWITCH_WEST = 0x6;
        private const byte HAT_SWITCH_NORTH_WEST = 0x7;
        private const byte HAT_SWITCH_NULL = 0x8;

    }
}