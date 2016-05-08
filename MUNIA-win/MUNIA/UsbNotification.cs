using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MUNIA {

	public static class UsbNotification {
		public static readonly Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"); // USB Raw Device.
		public static readonly Guid GUID_DEVINTERFACE_HID = new Guid("4d1e55b2-f16f-11cf-88cb-001111000030"); // USB HID Device.
		
		private static IntPtr _notificationHandle;
		private static readonly UsbDeviceNotificationListenerWindow _window;

		public static event EventHandler<UsbNotificationEventArgs> DeviceArrival;
		public static event EventHandler<UsbNotificationEventArgs> DeviceRemovalComplete;

		private static void OnDeviceArrival(UsbNotificationEventArgs args) {
			DeviceArrival?.Invoke(null, args);
		}

		private static void OnDeviceRemoveComplete(UsbNotificationEventArgs args) {
			DeviceRemovalComplete?.Invoke(null, args);
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
				DeviceType = DeviceType.DBT_DEVTYP_DEVICEINTERFACE,
				Reserved = 0,
				Name = 0,
				ClassGuid = GUID_DEVINTERFACE_HID, // not used when flags contains ALL_INTERFACE_CLASSES
			};

			dbi.Size = Marshal.SizeOf(dbi);
			IntPtr buffer = Marshal.AllocHGlobal(dbi.Size);
			Marshal.StructureToPtr(dbi, buffer, true);

			RegisterDeviceNotificationFlags flags =
				RegisterDeviceNotificationFlags.DEVICE_NOTIFY_WINDOW_HANDLE;
			_notificationHandle = RegisterDeviceNotification(windowHandle, buffer, flags);
		}

		/// <summary>
		/// Unregisters the window for USB device notifications
		/// </summary>
		private static void UnregisterUsbDeviceNotification() {
			UnregisterDeviceNotification(_notificationHandle);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, RegisterDeviceNotificationFlags flags);

		[DllImport("user32.dll")]
		private static extern bool UnregisterDeviceNotification(IntPtr handle);

		[StructLayout(LayoutKind.Sequential)]
		private struct DevBroadcastDeviceinterface {
			internal int Size;
			internal DeviceType DeviceType;
			internal int Reserved;
			internal Guid ClassGuid;
			internal short Name;
		}

		/// <summary>
		/// Represents the window that is used internally to get the messages.
		/// </summary>
		sealed class UsbDeviceNotificationListenerWindow : NativeWindow, IDisposable {
			const int WmDevicechange = 0x0219; // device change event      

			public UsbDeviceNotificationListenerWindow() {
				// create the handle for the window.
				CreateHandle(new CreateParams());
			}

			protected override void WndProc(ref Message m) {
				base.WndProc(ref m);
				if (m.Msg != WmDevicechange) return;

				WM_DeviceChangeMessage wp = (WM_DeviceChangeMessage)m.WParam;
				if (wp != WM_DeviceChangeMessage.DBT_DEVICEARRIVAL && wp != WM_DeviceChangeMessage.DBT_DEVICEREMOVECOMPLETE)
					return;

				Guid guid = Guid.Empty;
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
					guid = dev.dbcc_classguid;
				}
				name = name?.Substring(0, name.IndexOf('\0'));

				var args = new UsbNotificationEventArgs(type, name, guid);
				if (wp == WM_DeviceChangeMessage.DBT_DEVICEARRIVAL)
					OnDeviceArrival(args);
				else
					OnDeviceRemoveComplete(args);

			}
			public void Dispose() {
				DestroyHandle();
			}
		}

	}

	public class UsbNotificationEventArgs : EventArgs {
		public DeviceType DeviceType { get; private set; }
		public string Name { get; private set; }
		public Guid Guid { get; private set; }

		public UsbNotificationEventArgs(DeviceType type, string name, Guid guid) {
			DeviceType = type;
			Name = name;
			Guid = guid;
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

	public enum WM_DeviceChangeMessage {
		DBT_CONFIGCHANGECANCELED = 0x0019, // A request to change the current configuration (dock or undock) has been canceled.
		DBT_CONFIGCHANGED = 0x0018, // The current configuration has changed, due to a dock or undock.
		DBT_CUSTOMEVENT = 0x8006, // A custom event has occurred.
		DBT_DEVICEARRIVAL = 0x8000, // A device or piece of media has been inserted and is now available.
		DBT_DEVICEQUERYREMOVE = 0x8001, // Permission is requested to remove a device or piece of media. Any application can deny this request and cancel the removal.
		DBT_DEVICEQUERYREMOVEFAILED = 0x8002, // A request to remove a device or piece of media has been canceled.
		DBT_DEVICEREMOVECOMPLETE = 0x8004, // A device or piece of media has been removed.
		DBT_DEVICEREMOVEPENDING = 0x8003, // A device or piece of media is about to be removed. Cannot be denied.
		DBT_DEVICETYPESPECIFIC = 0x8005, // A device-specific event has occurred.
		DBT_DEVNODES_CHANGED = 0x0007, // A device has been added to or removed from the system.
		DBT_QUERYCHANGECONFIG = 0x0017, // Permission is requested to change the current configuration (dock or undock).
		DBT_USERDEFINED = 0xFFFF, // The meaning of this message is user-defined.
	}

	[Flags]
	public enum RegisterDeviceNotificationFlags : int {
		DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000, // The hRecipient parameter is a window handle
		DEVICE_NOTIFY_SERVICE_HANDLE = 0x00000001, // The hRecipient parameter is a service status handle
		DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 0x00000004, // Notifies the recipient of device interface events for all device interface classes
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct DeviceBroadcastHeader {
		public Int32 dbcc_size;
		public DeviceType dbcc_devicetype;
		public Int32 dbcc_reserved;
	}
	
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DEV_BROADCAST_DEVICEINTERFACE {
		public int dbcc_size;
		public int dbcc_devicetype;
		public int dbcc_reserved;
		public Guid dbcc_classguid;
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
