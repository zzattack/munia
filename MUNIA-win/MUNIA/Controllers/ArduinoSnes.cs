using System.Collections.Generic;
using MUNIA.Interop;

namespace MUNIA.Controllers {
	public class ArduinoSnes : ArduinoController {
		public ArduinoSnes(SerialPortInfo port) : base(port) {
			for (int i = 0; i < 12; i++) Buttons.Add(false);
		}

		public override ControllerType Type => ControllerType.SNES;
		protected override bool Parse(List<byte> buffer) {
			if (buffer.Count != 16) return false;
			var packet = Repack(buffer);

			// B Y SEL START
			int i = 0, mask = 0x80;
			Buttons[i] = (packet[0] & mask) != 0; mask >>= 1; i++;
			Buttons[i] = (packet[0] & mask) != 0; mask >>= 1; i++;
			Buttons[i] = (packet[0] & mask) != 0; mask >>= 1; i++;
			Buttons[i] = (packet[0] & mask) != 0; mask >>= 1; i++;

			// A X L R
			mask = 0x80;
			Buttons[i] = (packet[1] & mask) != 0; mask >>= 1; i++;
			Buttons[i] = (packet[1] & mask) != 0; mask >>= 1; i++;
			Buttons[i] = (packet[1] & mask) != 0; mask >>= 1; i++;
			Buttons[i] = (packet[1] & mask) != 0; mask >>= 1; i++;

			byte hat = (byte)(packet[0] & 0x0F);
			// UP DOWN LEFT RIGHT
			Buttons[i] = (hat & mask) != 0; mask >>= 1; i++;
			Buttons[i] = (hat & mask) != 0; mask >>= 1; i++;
			Buttons[i] = (hat & mask) != 0; mask >>= 1; i++;
			Buttons[i] = (hat & mask) != 0;
			return true;
		}
	}
}