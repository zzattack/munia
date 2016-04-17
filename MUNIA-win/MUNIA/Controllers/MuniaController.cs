using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpLib.Hid;
using SharpLib.Win32;

namespace MuniaInput {
    public abstract class MuniaController : IDisposable {
        /// <summary>
        /// Can be used to register for WM_INPUT messages and parse them.
        /// For testing purposes it can also be used to solely register for WM_INPUT messages.
        /// </summary>
        private Handler _handler;

        public Device HidDevice;
        public abstract List<int> Axes { get; }
        public abstract List<bool> Buttons { get; }

        public event EventHandler StateUpdated;

        protected MuniaController(Device hidDevice) {
            this.HidDevice = hidDevice;
        }
        public override string ToString() {
            return HidDevice.Product;
        }
        
        public static IEnumerable<MuniaController> ListDevices() {
            RAWINPUTDEVICELIST[] ridList = null;
            uint deviceCount = 0;
            // first call to determine number of devices available
            int res = Function.GetRawInputDeviceList(ridList, ref deviceCount, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
            if (res != 0) yield break;
            // allocate and fetch
            ridList = new RAWINPUTDEVICELIST[deviceCount];
            res = Function.GetRawInputDeviceList(ridList, ref deviceCount, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
            if (res != deviceCount) yield break;

            //For each our device add a node to our treeview
            foreach (RAWINPUTDEVICELIST device in ridList) {
                Device hidDevice;

                // Try create our HID device.
                try {
                    hidDevice = new Device(device.hDevice);
                }
                catch {
                    // Just skip that device then
                    continue;
                }
                if (hidDevice.Product == "NinHID NGC") {
                    yield return new NgcController(hidDevice);
                }
                else if (hidDevice.Product == "NinHID N64") {
                    yield return new N64Controller(hidDevice);
                }
                else if (hidDevice.Product == "NinHID SNES") {
                    yield return new SnesController(hidDevice);
                }
            }
        }
        
        /// <summary>
        /// Registers device for receiving inputs.
        /// </summary>
        /// <param name="wnd">Handle to window receiving WM_INPUT message for this device.
        /// Window should override WndProc and give it to us.</param>
        public void Activate(IntPtr wnd) {
            // Register the input device to receive the commands from the
            // remote device. See http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnwmt/html/remote_control.asp
            // for the vendor defined usage page.
            if (_handler != null) {
                //First de-register
                _handler.Dispose();
                _handler = null;
            }

            RAWINPUTDEVICE rid = new RAWINPUTDEVICE {
                usUsagePage = 1, // this.HidDevice.UsagePage,
                usUsage = 4, // this.HidDevice.UsageCollection,
                dwFlags = 0x00000100, // RIDEV_INPUTSINK - enables input receiving when the caller is not in foreground
                hwndTarget = wnd
            };

            _handler = new Handler(new[] { rid });
            if (!_handler.IsRegistered) {
                Debug.WriteLine("Failed to register raw input devices: " + Marshal.GetLastWin32Error().ToString());
            }

            _handler.OnHidEvent += HandleHidEvent;
        }

        public void Dispose() {
            _handler?.Dispose();
            HidDevice.Dispose();
        }

        public void WndProc(ref Message message) {
            _handler.ProcessInput(ref message);
        }

        private void HandleHidEvent(object sender, Event ev) {
            if (Parse(ev))
                StateUpdated?.Invoke(this, EventArgs.Empty);
        }
        protected abstract bool Parse(Event ev);


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