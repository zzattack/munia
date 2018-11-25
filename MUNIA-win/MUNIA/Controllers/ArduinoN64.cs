using System.Collections.Generic;
using MUNIA.Interop;
using MUNIA.Util;

namespace MUNIA.Controllers {
	public class ArduinoN64 : ArduinoController {
		public ArduinoN64(SerialPortInfo port) : base(port) {
			_buttons.EnsureSize(14);
			_axes.EnsureSize(2);
			_hats.EnsureSize(1);
		}
		public override ControllerType Type => ControllerType.N64;
		protected override bool Parse(List<byte> buffer) {
			if (buffer.Count != 32) return false;
			var packet = Repack(buffer);

			// A B Z START
			_buttons[0] = (packet[0] & 0x10) != 0;
			_buttons[1] = (packet[0] & 0x20) != 0;
			_buttons[2] = (packet[0] & 0x40) != 0;
			_buttons[3] = (packet[0] & 0x80) != 0;

			// _ _ L R CUP CDOWN CLEFT CRIGHT
			_buttons[4] = (packet[1] & 0x01) != 0;
			_buttons[5] = (packet[1] & 0x02) != 0;
			_buttons[6] = (packet[1] & 0x04) != 0;
			_buttons[7] = (packet[1] & 0x08) != 0;
			_buttons[8] = (packet[1] & 0x10) != 0;
			_buttons[9] = (packet[1] & 0x20) != 0;

			byte bhat = (byte)(packet[0] & 0x0F);
			Hat hat = Hat.None;
			if ((bhat & 0x01) != 0) hat |= Hat.Right;
			if ((bhat & 0x02) != 0) hat |= Hat.Left;
			if ((bhat & 0x04) != 0) hat |= Hat.Down;
			if ((bhat & 0x08) != 0) hat |= Hat.Up;
			_hats[0] = hat;
			
	        _buttons[10] = hat.HasFlag(Hat.Up);
            _buttons[11] = hat.HasFlag(Hat.Down);
            _buttons[12] = hat.HasFlag(Hat.Left);
            _buttons[13] = hat.HasFlag(Hat.Right);

			_axes[0] = (sbyte)packet[2] / 128.0;
			_axes[1] = -(sbyte)packet[3] / 128.0;

			return true;
		}

		public override bool IsAxisTrigger(int axisNum) => false;
	}
}