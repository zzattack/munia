using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using HidSharp;
using MUNIA.Controllers;

namespace MuniaInput {
    public abstract class MuniaController : IDisposable {
        /// <summary>
        /// Can be used to register for WM_INPUT messages and parse them.
        /// For testing purposes it can also be used to solely register for WM_INPUT messages.
        /// </summary>

        public HidDevice HidDevice;
		private HidStream _stream;
        public abstract List<int> Axes { get; }
        public abstract List<bool> Buttons { get; }

        public event EventHandler StateUpdated;

        protected MuniaController(HidDevice hidDevice) {
            this.HidDevice = hidDevice;
        }
        public override string ToString() {
            return HidDevice.ProductName;
        }
        
        public static IEnumerable<MuniaController> ListDevices() {
			var loader = new HidDeviceLoader();
			
	        //For each our device add a node to our treeview
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
		
	    public static HidDevice GetConfigInterface() {
			var loader = new HidDeviceLoader();
			//For each our device add a node to our treeview
			foreach (var device in loader.GetDevices(0x04d8, 0x0058)) {
				if (device.ProductName == "NinHID CFG") {
					return device;
				}
			}
			return null;
		}


		/// <summary>
		/// Registers device for receiving inputs.
		/// </summary>
		/// <param name="wnd">Handle to window receiving WM_INPUT message for this device.
		/// Window should override WndProc and give it to us.</param>
		public void Activate(IntPtr wnd) {
			_stream?.Dispose();
			_stream = HidDevice.Open();
			_stream.ReadTimeout = Timeout.Infinite;

			byte[] buffer = new byte[HidDevice.MaxInputReportLength];
			_stream.BeginRead(buffer, 0, buffer.Length, Callback, buffer);
		}

	    private void Callback(IAsyncResult ar) {
			try {
				int numBytes = _stream.EndRead(ar);
				byte[] buffer = (byte[])ar.AsyncState;
				if (numBytes > 0) {
					Parse(buffer);
					_stream.BeginRead(buffer, 0, buffer.Length, Callback, buffer);
				}
			}
			catch (IOException) { }
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