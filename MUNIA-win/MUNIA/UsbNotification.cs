using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MUNIA {

	public static class UsbNotification {
		private static readonly Guid GuidUsbDevices = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"); // USB devices
		private static IntPtr _notificationHandle;
		private const int DbtDevtypDeviceinterface = 5;
		private static UsbDeviceNotificationListenerWindow _window;

		public static event EventHandler<UsbNotificationEventArgs> DeviceArrival;
		public static event EventHandler<UsbNotificationEventArgs> DeviceRemovalComplete;

		private static void OnDeviceArrival(UsbNotificationEventArgs args) {
			if (DeviceArrival != null)
				DeviceArrival(null, args);
		}

		private static void OnDeviceRemoveComplete(UsbNotificationEventArgs args) {
			if (DeviceRemovalComplete != null)
				DeviceRemovalComplete(null, args);
		}

		static UsbNotification() {
			_window = new UsbDeviceNotificationListenerWindow();
			RegisterUsbDeviceNotification(_window.Handle);
			Application.ApplicationExit += (sender, args) => {
				UnregisterUsbDeviceNotification();
				_window.Dispose();
			};
		}

		/// <summary>
		/// Registers a window to receive notifications when USB devices are plugged or unplugged.
		/// </summary>
		/// <param name="windowHandle">Handle to the window receiving notifications.</param>
		private static void RegisterUsbDeviceNotification(IntPtr windowHandle) {
			var dbi = new DevBroadcastDeviceinterface {
				DeviceType = DbtDevtypDeviceinterface,
				Reserved = 0,
				ClassGuid = GuidUsbDevices,
				Name = 0
			};

			dbi.Size = Marshal.SizeOf(dbi);
			IntPtr buffer = Marshal.AllocHGlobal(dbi.Size);
			Marshal.StructureToPtr(dbi, buffer, true);

			_notificationHandle = RegisterDeviceNotification(windowHandle, buffer, 0);
		}

		/// <summary>
		/// Unregisters the window for USB device notifications
		/// </summary>
		private static void UnregisterUsbDeviceNotification() {
			UnregisterDeviceNotification(_notificationHandle);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

		[DllImport("user32.dll")]
		private static extern bool UnregisterDeviceNotification(IntPtr handle);

		[StructLayout(LayoutKind.Sequential)]
		private struct DevBroadcastDeviceinterface {
			internal int Size;
			internal int DeviceType;
			internal int Reserved;
			internal Guid ClassGuid;
			internal short Name;
		}


		/// <summary>
		/// Represents the window that is used internally to get the messages.
		/// </summary>
		sealed class UsbDeviceNotificationListenerWindow : NativeWindow, IDisposable {
			const int DbtDeviceArrival = 0x8000; // system detected a new device        
			const int DbtDeviceRemoveComplete = 0x8004; // device is gone      
			const int WmDevicechange = 0x0219; // device change event      

			public UsbDeviceNotificationListenerWindow() {
				// create the handle for the window.
				CreateHandle(new CreateParams());
			}

			protected override void WndProc(ref Message m) {
				base.WndProc(ref m);
				if (m.Msg == WmDevicechange) {

					if ((int)m.WParam != DbtDeviceArrival && (int)m.WParam != DbtDeviceRemoveComplete)
						return;
					
					string name = null;

					var hdr = (DeviceBroadcastHeader)Marshal.PtrToStructure(m.LParam, typeof(DeviceBroadcastHeader));
					DeviceType type = hdr.dbcc_devicetype;
					if (type == DeviceType.DBT_DEVTYP_PORT) {
						var prt = (DEV_BROADCAST_PORT)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_PORT));
						name = new string(prt.dbcp_name);
					}
					else if (type == DeviceType.DBT_DEVTYP_DEVICEINTERFACE) {
						var dev = (DEV_BROADCAST_DEVICEINTERFACE)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_DEVICEINTERFACE));
						name = new string(dev.dbcc_name);
					}
					name = name?.Substring(0, name.IndexOf('\0'));

					var args = new UsbNotificationEventArgs(type, name);
					switch ((int)m.WParam) {
						case DbtDeviceRemoveComplete:
							OnDeviceRemoveComplete(args); //
							break;
						case DbtDeviceArrival:
							OnDeviceArrival(args);
							break;
					}
				}
			}
			public void Dispose() {
				DestroyHandle();
			}
		}

	}

	public class UsbNotificationEventArgs : EventArgs {
		public DeviceType DeviceType { get; private set; }
		public string Name { get; private set; }
		public UsbNotificationEventArgs(DeviceType type, string name) {
			DeviceType = type;
			Name = name;
		}
	}

	public enum DeviceType {
		DBT_DEVTYP_OEM = 0x00000000, //OEM-defined device type
		DBT_DEVTYP_DEVNODE = 0x00000001, //Devnode number
		DBT_DEVTYP_VOLUME = 0x00000002, //Logical volume
		DBT_DEVTYP_PORT = 0x00000003, //Serial, parallel
		DBT_DEVTYP_NET = 0x00000004, //Network resource
		DBT_DEVTYP_DEVICEINTERFACE = 0x00000005, //Device interface class
		DBT_DEVTYP_HANDLE = 0x00000006 //File system handle
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct DeviceBroadcastHeader {
		public Int32 dbcc_size;
		public DeviceType dbcc_devicetype;
		public Int32 dbcc_reserved;
	}
	
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DEV_BROADCAST_DEVICEINTERFACE {
		public Int32 dbcc_size;
		public Int32 dbcc_devicetype;
		public Int32 dbcc_reserved;
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
		public byte[] dbcc_classguid;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public char[] dbcc_name;
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public struct DEV_BROADCAST_VOLUME {
		public Int32 dbcv_size;
		public Int32 dbcv_devicetype;
		public Int32 dbcv_reserved;
		public Int32 dbcv_unitmask;
		public Int16 dbcv_flags;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DEV_BROADCAST_PORT {
		public Int32 dbcp_size;
		public Int32 dbcp_devicetype;
		public Int32 dbcp_reserved;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public char[] dbcp_name;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DEV_BROADCAST_OEM {
		public Int32 dbco_size;
		public Int32 dbco_devicetype;
		public Int32 dbco_reserved;
		public Int32 dbco_identifier;
		public Int32 dbco_suppfunc;
	}

}
