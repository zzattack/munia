using System.Collections.Generic;
using MUNIA.Interop;
using MUNIA.Util;

namespace MUNIA.Controllers {
	public class ArduinoSnes : ArduinoController {
		public ArduinoSnes(SerialPortInfo port) : base(port) {
			_buttons.EnsureSize(12);
			_hats.EnsureSize(1);
		}

		public override ControllerType Type => ControllerType.SNES;
		protected override bool Parse(List<byte> buffer) {
			if (buffer.Count != 16) return false;
			var packet = Repack(buffer);

			// B Y SEL START
			int i = 0, mask = 0x10;
			_buttons[i++] = (packet[0] & mask) != 0; mask <<= 1;
			_buttons[i++] = (packet[0] & mask) != 0; mask <<= 1;
			_buttons[i++] = (packet[0] & mask) != 0; mask <<= 1;
			_buttons[i++] = (packet[0] & mask) != 0; mask <<= 1;	 

			// A X L R
			mask = 0x10;										
			_buttons[i++] = (packet[1] & mask) != 0; mask <<= 1;
			_buttons[i++] = (packet[1] & mask) != 0; mask <<= 1;
			_buttons[i++] = (packet[1] & mask) != 0; mask <<= 1;
			_buttons[i++] = (packet[1] & mask) != 0; mask <<= 1;

			byte bhat = (byte)(packet[0] & 0x0F);
			Hat hat = Hat.None;
			// inverted order UP DOWN LEFT RIGHT
			if ((bhat & 0x01) != 0) hat |= Hat.Right;
			if ((bhat & 0x02) != 0) hat |= Hat.Left;
			if ((bhat & 0x04) != 0) hat |= Hat.Down;
			if ((bhat & 0x08) != 0) hat |= Hat.Up;
			_hats[0] = hat;

	        _buttons[i++] = hat.HasFlag(Hat.Up);
            _buttons[i++] = hat.HasFlag(Hat.Down);
            _buttons[i++] = hat.HasFlag(Hat.Left);
            _buttons[i++] = hat.HasFlag(Hat.Right);

			return true;
		}

		public override bool IsAxisTrigger(int axisNum) => false;
	}
}