using System;

namespace MUNIA {
	public class MuniaDeviceInfo {
		public enum OutputMode { NGC, N64, SNES, PC };

		[Flags]
		public enum InputSources {
			None = 0,
			NGC = 1,
			N64 = 2,
			SNES = 4
		};

		// new menu structure fields
		public InputSources Inputs;
		public OutputMode Output;

		// legacy fields
		public bool IsLegacy { get; private set; }
		public SnesMode SNES;
		public N64Mode N64;
		public NGCMode NGC;
		public enum SnesMode { SNES_MODE_SNES, SNES_MODE_PC, SNES_MODE_NGC };
		public enum N64Mode { N64_MODE_N64, N64_MODE_PC };
		public enum NGCMode { NGC_MODE_NGC, NGC_MODE_PC, NGC_MODE_N64 };

		public int VersionMajor;
		public int VersionMinor;
		public int HardwareRevision;
		public ushort DeviceID;
		public byte PICRevision;

		public bool Parse(byte[] report) {
			if (report[0] != 0 || report[1] != 0x047 || report.Length != 9) return false;
			VersionMajor = report[2] >> 4;
			VersionMinor = report[2] & 0x0f;
			HardwareRevision = report[3] & 0x0f;
			var v = new Version(VersionMajor, VersionMinor);
			if (v >= Version.Parse("1.5")) {
				IsLegacy = false;
				Output = (OutputMode) report[4];
				Inputs = (InputSources) report[5];
			}
			else {
				IsLegacy = true;
				NGC = (NGCMode)report[4];
				N64 = (N64Mode)report[5];
				SNES = (SnesMode)report[6];
			}
			DeviceID = (ushort) ((report[7] << 8) | report[8]);
			PICRevision = (byte) (DeviceID & 0x0F);
			DeviceID &= 0xFFF0;
			return true;
		}


		public byte[] ToWriteReport() {
			byte[] report = new byte[9];
			report[0] = 0;
			report[1] = (byte)(IsLegacy ? 0x46 : 0x44);
			report[2] = (byte)Output;
			report[3] = (byte)Inputs;
			SNES = (SnesMode)report[4];
			N64 = (N64Mode)report[5];
			NGC = (NGCMode)report[6];
			return report;
		}
	}
}