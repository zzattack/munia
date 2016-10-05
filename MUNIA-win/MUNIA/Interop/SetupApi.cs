using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MUNIA.Interop {
	public static class SetupApi {

		#region Enumerations

		[Flags]
		public enum DiGetFlags {
			Default = 0x01,
			Present = 0x02,
			AllClasses = 0x04,
			Profile = 0x08,
			DeviceInterface = 0x10,
		}

		[Flags]
		public enum SpIntFlags {
			Active = 0x1,
			Default = 0x2,
			Removed = 0x4,
		}

		public enum SPDRP : int {
			/// <summary>
			/// DeviceDesc (R/W)
			/// </summary>
			SPDRP_DEVICEDESC = 0x00000000,

			/// <summary>
			/// HardwareID (R/W)
			/// </summary>
			SPDRP_HARDWAREID = 0x00000001,

			/// <summary>
			/// CompatibleIDs (R/W)
			/// </summary>
			SPDRP_COMPATIBLEIDS = 0x00000002,

			/// <summary>
			/// unused
			/// </summary>
			SPDRP_UNUSED0 = 0x00000003,

			/// <summary>
			/// Service (R/W)
			/// </summary>
			SPDRP_SERVICE = 0x00000004,

			/// <summary>
			/// unused
			/// </summary>
			SPDRP_UNUSED1 = 0x00000005,

			/// <summary>
			/// unused
			/// </summary>
			SPDRP_UNUSED2 = 0x00000006,

			/// <summary>
			/// Class (R--tied to ClassGUID)
			/// </summary>
			SPDRP_CLASS = 0x00000007,

			/// <summary>
			/// ClassGUID (R/W)
			/// </summary>
			SPDRP_CLASSGUID = 0x00000008,

			/// <summary>
			/// Driver (R/W)
			/// </summary>
			SPDRP_DRIVER = 0x00000009,

			/// <summary>
			/// ConfigFlags (R/W)
			/// </summary>
			SPDRP_CONFIGFLAGS = 0x0000000A,

			/// <summary>
			/// Mfg (R/W)
			/// </summary>
			SPDRP_MFG = 0x0000000B,

			/// <summary>
			/// FriendlyName (R/W)
			/// </summary>
			SPDRP_FRIENDLYNAME = 0x0000000C,

			/// <summary>
			/// LocationInformation (R/W)
			/// </summary>
			SPDRP_LOCATION_INFORMATION = 0x0000000D,

			/// <summary>
			/// PhysicalDeviceObjectName (R)
			/// </summary>
			SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0x0000000E,

			/// <summary>
			/// Capabilities (R)
			/// </summary>
			SPDRP_CAPABILITIES = 0x0000000F,

			/// <summary>
			/// UiNumber (R)
			/// </summary>
			SPDRP_UI_NUMBER = 0x00000010,

			/// <summary>
			/// UpperFilters (R/W)
			/// </summary>
			SPDRP_UPPERFILTERS = 0x00000011,

			/// <summary>
			/// LowerFilters (R/W)
			/// </summary>
			SPDRP_LOWERFILTERS = 0x00000012,

			/// <summary>
			/// BusTypeGUID (R)
			/// </summary>
			SPDRP_BUSTYPEGUID = 0x00000013,

			/// <summary>
			/// LegacyBusType (R)
			/// </summary>
			SPDRP_LEGACYBUSTYPE = 0x00000014,

			/// <summary>
			/// BusNumber (R)
			/// </summary>
			SPDRP_BUSNUMBER = 0x00000015,

			/// <summary>
			/// Enumerator Name (R)
			/// </summary>
			SPDRP_ENUMERATOR_NAME = 0x00000016,

			/// <summary>
			/// Security (R/W, binary form)
			/// </summary>
			SPDRP_SECURITY = 0x00000017,

			/// <summary>
			/// Security (W, SDS form)
			/// </summary>
			SPDRP_SECURITY_SDS = 0x00000018,

			/// <summary>
			/// Device Type (R/W)
			/// </summary>
			SPDRP_DEVTYPE = 0x00000019,

			/// <summary>
			/// Device is exclusive-access (R/W)
			/// </summary>
			SPDRP_EXCLUSIVE = 0x0000001A,

			/// <summary>
			/// Device Characteristics (R/W)
			/// </summary>
			SPDRP_CHARACTERISTICS = 0x0000001B,

			/// <summary>
			/// Device Address (R)
			/// </summary>
			SPDRP_ADDRESS = 0x0000001C,

			/// <summary>
			/// UiNumberDescFormat (R/W)
			/// </summary>
			SPDRP_UI_NUMBER_DESC_FORMAT = 0X0000001D,

			/// <summary>
			/// Device Power Data (R)
			/// </summary>
			SPDRP_DEVICE_POWER_DATA = 0x0000001E,

			/// <summary>
			/// Removal Policy (R)
			/// </summary>
			SPDRP_REMOVAL_POLICY = 0x0000001F,

			/// <summary>
			/// Hardware Removal Policy (R)
			/// </summary>
			SPDRP_REMOVAL_POLICY_HW_DEFAULT = 0x00000020,

			/// <summary>
			/// Removal Policy Override (RW)
			/// </summary>
			SPDRP_REMOVAL_POLICY_OVERRIDE = 0x00000021,

			/// <summary>
			/// Device Install State (R)
			/// </summary>
			SPDRP_INSTALL_STATE = 0x00000022,

			/// <summary>
			/// Device Location Paths (R)
			/// </summary>
			SPDRP_LOCATION_PATHS = 0x00000023,
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class SpDevInfoData {
			public uint cbSize;
			public Guid ClassGuid;
			public uint DevInst;
			public IntPtr Reserved;
		}
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
		public class SpDeviceInterfaceData {
			public int Size;
			public Guid interfaceClassGuid;
			public SpIntFlags flags;
			public IntPtr Reserved;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		public struct SpDeviceInterfaceDetailData {
			public int Size;
			// SizeConst is MaxPath
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string DevicePath;
		}

		#endregion

		#region Functions

		[DllImport("setupapi.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, string enumerator, IntPtr hWndParent, DiGetFlags flags);

		// Free device info set returned by SetupDiGetClassDevs
		[DllImport("setupapi.dll")]
		public static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern bool SetupDiEnumDeviceInfo(IntPtr deviceInfoSet, UInt32 memberIndex, SpDevInfoData deviceInfoData);

		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern bool SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, SpDevInfoData deviceInfoData,
			ref Guid interfaceClassGuid, uint memberIndex, SpDeviceInterfaceData deviceInterfaceData);

		[DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, SpDeviceInterfaceData deviceInterfaceData,
			ref SpDeviceInterfaceDetailData deviceInterfaceDetailData, int deviceInterfaceDetailDataSize,
			ref int requiredSize, IntPtr unusedDeviceInfoData);

		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern bool SetupDiSetDeviceRegistryProperty(IntPtr deviceInfoSet, SpDevInfoData devInfoData,
			int property, byte[] propertyBuffer, int propertyBufferSize);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SetupDiOpenDevRegKey(IntPtr hDeviceInfoSet, SetupApi.SpDevInfoData deviceInfoData, uint scope,
			uint hwProfile, uint parameterRegistryValueKind, uint samDesired);
        
		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern bool SetupDiGetDeviceRegistryProperty(IntPtr deviceInfoSet, SpDevInfoData devInfoData,
			int property, out int regDataType, [Out]byte[] propertyBuffer, int propertyBufferSize, out int requiredSize);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetupDiGetDeviceRegistryProperty(IntPtr deviceInfoSet, SpDevInfoData deviceInfoData,
			uint property, UInt32 propertyRegDataType, StringBuilder propertyBuffer, uint propertyBufferSize, out UInt32 requiredSize);

		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern bool SetupDiGetDeviceInstanceId(IntPtr deviceInfoSet, SpDevInfoData devInfoData,
			[Out]byte[] deviceInstanceIdBuffer, int deviceInstanceIdSize, out int requiredSize);

		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern int CM_Get_Device_ID(uint devInst, byte[] buffer, int bufferLen, int unused);

		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern int CM_Get_Parent(out uint parentDevInst, uint devInst, int unused);

		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern bool SetupDiClassGuidsFromName(string ClassName, ref Guid ClassGuidArray1stItem, UInt32 ClassGuidArraySize, out UInt32 RequiredSize);

		#endregion
	}
}