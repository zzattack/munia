using System.Collections.Generic;
using MUNIA.Interop;

namespace MUNIA.Controllers {
	public class ArduinoN64 : ArduinoController {
		public ArduinoN64(SerialPortInfo port) : base(port) {
			for (int i = 0; i < 14; i++) Buttons.Add(false);
			for (int i = 0; i < 2; i++) Axes.Add(0);
		}
		public override ControllerType Type => ControllerType.N64;
		protected override bool Parse(List<byte> buffer) {
			if (buffer.Count != 32) return false;
			var packet = Repack(buffer);

			// A B Z START
			Buttons[0] = (packet[0] & 0x80) != 0;
			Buttons[1] = (packet[0] & 0x40) != 0;
			Buttons[2] = (packet[0] & 0x20) != 0;
			Buttons[3] = (packet[0] & 0x10) != 0;

			// _ _ L R CUP CDOWN CLEFT CRIGHT
			Buttons[4] = (packet[1] & 0x20) != 0;
			Buttons[5] = (packet[1] & 0x10) != 0;
			Buttons[6] = (packet[1] & 0x08) != 0;
			Buttons[7] = (packet[1] & 0x04) != 0;
			Buttons[8] = (packet[1] & 0x02) != 0;
			Buttons[9] = (packet[1] & 0x01) != 0;

			byte hat = (byte)(packet[0] & 0x0F);
			// UP DOWN LEFT RIGHT
			Buttons[10] = (hat & 0x08) != 0;
			Buttons[11] = (hat & 0x04) != 0;
			Buttons[12] = (hat & 0x02) != 0;
			Buttons[13] = (hat & 0x01) != 0;

			Axes[0] = (sbyte)packet[2];
			Axes[1] = -(sbyte)packet[3];

			return true;
		}
	}
}