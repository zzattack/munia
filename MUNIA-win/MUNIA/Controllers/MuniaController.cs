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
		
        protected readonly List<bool> _buttons = new List<bool>();
        protected readonly List<int> _axes = new List<int>();
        protected readonly List<Hat> _hats = new List<Hat>();
	    public ControllerState GetState() => new ControllerState(_axes, _buttons, _hats);

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
                    yield return new MuniaNgc(device);
                }
                else if (device.ProductName == "NinHID N64") {
                    yield return new MuniaN64(device);
				}
				else if (device.ProductName == "NinHID SNES") {
					yield return new MuniaSnes(device);
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
	    public bool Activate() {
		    try {
			    _stream?.Dispose();
			    _stream = HidDevice.Open();
			    _stream.ReadTimeout = Timeout.Infinite;

			    byte[] buffer = new byte[HidDevice.MaxInputReportLength];
			    _stream.BeginRead(buffer, 0, buffer.Length, Callback, buffer);
			    return true;
		    }
		    catch {
			    return false;
		    }
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
		
    }
}