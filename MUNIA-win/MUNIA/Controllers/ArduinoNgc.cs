using System.Collections.Generic;
using MUNIA.Interop;

namespace MUNIA.Controllers {
	public class ArduinoNgc : ArduinoController {
		public ArduinoNgc(SerialPortInfo port) : base(port) {
            for (int i = 0; i < 12; i++) Buttons.Add(false);
            for (int i = 0; i < 6; i++) Axes.Add(0);
		}
		public override ControllerType Type => ControllerType.NGC;

		protected override bool Parse(List<byte> buffer) {
			if (buffer.Count != 64) return false;
			var packet = Repack(buffer);

			// 0 0 0 START Y X B A
			Buttons[0] = (packet[0] & 0x10) != 0;
			Buttons[1] = (packet[0] & 0x08) != 0;
			Buttons[2] = (packet[0] & 0x04) != 0;
			Buttons[3] = (packet[0] & 0x02) != 0;
			Buttons[4] = (packet[0] & 0x01) != 0;

			// L R Z
			Buttons[5] = (packet[1] & 0x40) != 0;
			Buttons[6] = (packet[1] & 0x20) != 0;
			Buttons[7] = (packet[1] & 0x10) != 0;

			byte hat = (byte)(packet[1] & 0x0F);
			// UP DOWN RIGHT LEFT
			Buttons[8] = (hat & 0x08) != 0;
			Buttons[9] = (hat & 0x04) != 0;
			Buttons[11] = (hat & 0x02) != 0;
			Buttons[10] = (hat & 0x01) != 0;

			Axes[0] = packet[2] - 128;
			Axes[1] = 128 - packet[3];
			Axes[2] = packet[4] - 128;
			Axes[3] = 128 - packet[5];
			Axes[4] = packet[6];
			Axes[5] = packet[7];

			return true;
		}
	}
}