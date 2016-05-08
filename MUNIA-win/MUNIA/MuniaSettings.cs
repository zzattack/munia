namespace MUNIA {
	public class MuniaSettings {
		public enum SnesMode { SNES_MODE_SNES, SNES_MODE_PC, SNES_MODE_NGC };
		public enum N64Mode { N64_MODE_N64, N64_MODE_PC };
		public enum NGCMode { NGC_MODE_NGC, NGC_MODE_PC };
		public SnesMode SNES;
		public N64Mode N64;
		public NGCMode NGC;
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
			SNES = (SnesMode)report[4];
			N64 = (N64Mode)report[5];
			NGC = (NGCMode)report[6];
			DeviceID = (ushort)((report[7] << 8) | report[8]);
			PICRevision = (byte)(DeviceID & 0x0F);
			DeviceID &= 0xFFF0;
			return true;
		}

		public byte[] ToWriteReport() {
			byte[] report = new byte[9];
			report[0] = 0;
			report[1] = 0x46;
			report[2] = (byte)SNES;
			report[3] = (byte)N64;
			report[4] = (byte)NGC;
			return report;
		}
	}
}