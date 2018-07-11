using System;
using System.Diagnostics;

namespace MUNIA.Controllers {
	public class MusiaDeviceInfo {
		public const byte CFG_CMD_WRITE = 0x44;
		public const byte CFG_CMD_INFO = 0x46;
		public const byte CFG_CMD_READ = 0x47;
		

		// info fields
		public uint DeviceType { get; private set; }
		public int VersionMajor { get; private set; }
		public int VersionMinor { get; private set; }
		public int HardwareRevision { get; private set; }
		public string DeviceTypeName {
			get {
				switch (DeviceType & 0xFFF) {
					case 0x448: return "STM32F072C6";
					case 0x445: return "STM32F042C6";
					default: return "unknown";
				}
			}
		}
		public Version Version => new Version(VersionMajor, VersionMinor);

		// config fields
		public enum OutputMode : byte { PS2, PC };
		public OutputMode Output { get; set; }
		public enum PollingFrequencySetting : byte {
			Poll25Hz = 25,
			Poll30Hz = 30,
			Poll50Hz = 50,
			Poll60Hz = 60,
			Poll100Hz = 100,
			Poll120Hz = 120,
		};

		public PollingFrequencySetting PollingFrequency { get; set; }

		public bool Parse(byte[] info, byte[] config) {
			DeviceType = BitConverter.ToUInt32(info, 0);
			VersionMajor = info[4] & 0x0F;
			HardwareRevision = info[4] >> 4;
			VersionMinor = info[5];

			Output = (OutputMode)config[0];
			PollingFrequency = (PollingFrequencySetting)config[2];

			return true;
		}
		
		public byte[] ToWriteReport() {
			byte[] report = new byte[9];
			report[0] = CFG_CMD_WRITE;
			report[1] = (byte)(Output == OutputMode.PS2 ? 0 : 1);
			report[2] = 0; // was rumble allowed
			report[3] = (byte)PollingFrequency;
			return report;
		}
	}
}