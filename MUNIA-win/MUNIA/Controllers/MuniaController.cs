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
		public virtual string Name => HidDevice.ProductName;
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
            return Name;
        }
        
        public static IEnumerable<MuniaController> ListDevices() {
			var loader = new HidDeviceLoader();
            foreach (var device in loader.GetDevices(0x04d8, 0x0058)) {
                if (device.ProductName == "NinHID NGC" || (device.MaxInputReportLength == 9 && device.MaxOutputReportLength == 0)) {
                    yield return new MuniaNgc(device);
                }
                else if (device.ProductName == "NinHID N64" || device.MaxInputReportLength == 5) {
                    yield return new MuniaN64(device);
                }
                else if (device.ProductName == "NinHID SNES" || device.MaxInputReportLength == 3) {
                    yield return new MuniaSnes(device);
                }
            }

            foreach (var device in loader.GetDevices(0x1209, 0x8844)) {
                if (device.ProductName == "PS2 controller" || (device.MaxInputReportLength == 7 && device.MaxOutputReportLength == 0x10)) {
                    yield return new MusiaPS2(device);
                }
            }
        }
		public static MuniaController GetByName(string deviceName) {
			return ListDevices().FirstOrDefault(c => c.HidDevice.ProductName == deviceName);
		}

		public static HidDevice GetConfigInterface() {
			var loader = new HidDeviceLoader();
			// todo: use foreach/window instead of firstordefault
		    return loader.GetDevices(0x04d8, 0x0058).FirstOrDefault(device => device.ProductName == "NinHID CFG"
				|| (device.MaxInputReportLength == device.MaxOutputReportLength && device.MaxInputReportLength == 9));
	    }

	    class StreamAndBuffer {
		    public byte[] buffer;
		    public Stream stream;
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
			    var sb = new StreamAndBuffer {buffer = buffer, stream = _stream};

			    _stream.BeginRead(buffer, 0, buffer.Length, Callback, sb);
			    return true;
		    }
		    catch {
			    return false;
		    }
	    }

	    public void Deactivate() {
			_stream?.Dispose();
		    _stream = null;
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
			var sb = (StreamAndBuffer)ar.AsyncState;
			try {
				int numBytes = sb.stream.EndRead(ar);
				if (numBytes > 0) {
					if (Parse(sb.buffer))
						StateUpdated?.Invoke(this, EventArgs.Empty);
					sb.stream.BeginRead(sb.buffer, 0, sb.buffer.Length, Callback, sb);
				}
			}
			catch (IOException exc) {
				_stream = null;
				Debug.WriteLine("IOException: " + exc.Message);
				sb.stream.Dispose();
			}
			catch (NullReferenceException) { }
		}

	    public void Dispose() {
			_stream?.Dispose();
        }
		
		protected abstract bool Parse(byte[] ev);
		
    }
}