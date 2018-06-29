using System;

namespace MUNIA.Controllers {
	public class MusiaDeviceInfo {
			public enum OutputMode { PS2, PC };

			// new menu structure fields
			public OutputMode Output;

			// legacy fields
			public Version Version => new Version(VersionMajor, VersionMinor);

			public int VersionMajor;
			public int VersionMinor;
			public int HardwareRevision;
			public string DeviceID;

			public bool Parse(byte[] report) {
				if (report[0] != 0 || report[1] != 0x047 || report.Length != 9) return false;
				VersionMajor = report[2] >> 4;
				VersionMinor = report[2] & 0x0f;
				HardwareRevision = report[3] & 0x0f;
				var v = new Version(VersionMajor, VersionMinor);
				Output = (OutputMode)report[4];
				return true;
			}


			public byte[] ToWriteReport() {
				byte[] report = new byte[9];
				report[0] = 0;
				report[1] = 0x44;
				report[2] = (byte)Output;
				return report;
			}
	}
}