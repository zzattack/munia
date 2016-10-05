using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace MUNIA.Interop {

	public class SerialPortInfo {
		public string Name { get; set; }
		public string Description { get; set; }
		public string HardwareID { get; set; }
		public bool IsConnected => SerialPort.GetPortNames().Contains(Name);

		private static IEnumerable<SerialPortInfo> _portsCache;
		public static IEnumerable<SerialPortInfo> GetPorts(bool useCache = false) {
			if (!useCache || _portsCache == null) {
				try {
					_portsCache = GetPortsSetupApi();
				}
				catch {
					try {
						_portsCache = GetPortsWMI();
					}
					catch { _portsCache = SerialPort.GetPortNames().Select(s => new SerialPortInfo { Name = s, }); }
				}
			}
			return _portsCache;
		}


		/// <summary>
		///     Obtains the serial ports and their descriptions using SetupAPI.
		///     Prefered over the WMI implementation because it is significantly
		///     faster, but requires admin access.
		/// </summary>
		private static IEnumerable<SerialPortInfo> GetPortsSetupApi() {
			var guids = new[] {
				Guid.Parse(GUID_DEVINTERFACE_COMPORT2),
				// Guid.AddToBuffer(GUID_DEVINTERFACE_COMPORT)
			};

			foreach (var guid in guids) {
				var guidCopy = guid;
				IntPtr hDeviceInfoSet = SetupApi.SetupDiGetClassDevs(ref guidCopy, null, IntPtr.Zero,
																	SetupApi.DiGetFlags.Present);

				if (hDeviceInfoSet == IntPtr.Zero)
					throw new Exception("Failed to get device information set for the COM ports");

				Int32 iMemberIndex = 0;
				while (true) {
					var deviceInfoData = new SetupApi.SpDevInfoData();
					deviceInfoData.cbSize = (uint)Marshal.SizeOf(typeof(SetupApi.SpDevInfoData));
					bool success = SetupApi.SetupDiEnumDeviceInfo(hDeviceInfoSet, (uint)iMemberIndex, deviceInfoData);
					if (!success) {
						// No more devices in the device information set
						break;
					}
					var spiq = new SerialPortInfo {
						Name = GetDeviceName(hDeviceInfoSet, deviceInfoData),
					};
					GetRegistryProperties(spiq, hDeviceInfoSet, deviceInfoData);
					if (!string.IsNullOrEmpty(spiq.Name) && spiq.Name.Contains("COM"))
						yield return spiq;
					iMemberIndex++;
				}
				SetupApi.SetupDiDestroyDeviceInfoList(hDeviceInfoSet);
			}
		}

		private static IEnumerable<SerialPortInfo> GetPortsWMI() {
			var comPortInfoList = new List<SerialPortInfo>();
			var objectQuery = new ObjectQuery("SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0");
			using (var comPortSearcher = new ManagementObjectSearcher(objectQuery)) {
				foreach (string caption in comPortSearcher.Get().Cast<ManagementObject>().Select(obj => obj["Caption"].ToString()).Where(caption => caption.Contains("(COM"))) {
					string name = caption.Substring(caption.LastIndexOf("(COM", StringComparison.Ordinal)).Replace("(", string.Empty).Replace(")", string.Empty);
					string descr = caption.Substring(0, caption.IndexOf("(COM") - 1);
					comPortInfoList.Add(new SerialPortInfo { Name = name, Description = descr });
				}
			}
			return comPortInfoList;
		}

		private static string GetDeviceName(IntPtr pDevInfoSet, SetupApi.SpDevInfoData deviceInfoData) {
			IntPtr hDeviceRegistryKey = SetupApi.SetupDiOpenDevRegKey(pDevInfoSet, deviceInfoData,
																	DICS_FLAG_GLOBAL, 0, DIREG_DEV, KEY_QUERY_VALUE);
			if (hDeviceRegistryKey == IntPtr.Zero) {
				throw new Exception("Failed to open a registry key for device-specific configuration information");
			}

			var deviceNameBuf = new StringBuilder(256);
			try {
				uint lpRegKeyType;
				uint length = (uint)deviceNameBuf.Capacity;
				int result = RegQueryValueEx(hDeviceRegistryKey, "PortName", 0, out lpRegKeyType, deviceNameBuf, ref length);
				if (result != 0) {
					throw new Exception("Can not read registry value PortName for device " + deviceInfoData.ClassGuid);
				}
			}
			finally {
				RegCloseKey(hDeviceRegistryKey);
			}

			string deviceName = deviceNameBuf.ToString();
			return deviceName;
		}

		private static void GetRegistryProperties(SerialPortInfo spiq, IntPtr hDeviceInfoSet, SetupApi.SpDevInfoData deviceInfoData) {
			var propertyBuffer = new StringBuilder(256);
			const uint propRegDataType = 0;
			uint length = (uint)propertyBuffer.Capacity;
			bool success = SetupApi.SetupDiGetDeviceRegistryProperty(hDeviceInfoSet, deviceInfoData, (int)SetupApi.SPDRP.SPDRP_DEVICEDESC,
																	propRegDataType, propertyBuffer, length, out length);

			//if (!success) throw new Exception("Can not read registry for device " + deviceInfoData.ClassGuid);
			if (success) spiq.Description = propertyBuffer.ToString();

			propertyBuffer = new StringBuilder(256);
			length = (uint)propertyBuffer.Capacity;
			success = SetupApi.SetupDiGetDeviceRegistryProperty(hDeviceInfoSet, deviceInfoData, (int)SetupApi.SPDRP.SPDRP_HARDWAREID,
																propRegDataType, propertyBuffer, length, out length);
			if (success) spiq.HardwareID = propertyBuffer.ToString();
		}

		public override string ToString() {
			return string.Format("{0} ({1})", Name, Description);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((SerialPortInfo)obj);
		}

		protected bool Equals(SerialPortInfo other) {
			return string.Equals(Name, other.Name);
		}

		public override int GetHashCode() {
			unchecked {
				return (Name?.GetHashCode() ?? 0) * 397;
			}
		}

		#region interop code

		private const UInt32 DIGCF_PRESENT = 0x00000002;
		private const UInt32 DIGCF_DEVICEINTERFACE = 0x00000010;

		private const UInt32 DICS_FLAG_GLOBAL = 0x00000001;
		private const UInt32 DIREG_DEV = 0x00000001;
		private const UInt32 KEY_QUERY_VALUE = 0x0001;
		private const string GUID_DEVINTERFACE_COMPORT = "86E0D1E0-8089-11D0-9CE4-08003E301F73";
		private const string GUID_DEVINTERFACE_COMPORT2 = "4d36e978-e325-11ce-bfc1-08002be10318";

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegQueryValueExW", SetLastError = true)]
		private static extern int RegQueryValueEx(IntPtr hKey, string lpValueName, int lpReserved, out uint lpType,
												StringBuilder lpData, ref uint lpcbData);

		[DllImport("advapi32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern int RegCloseKey(IntPtr hKey);

		#endregion
	}


}