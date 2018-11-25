using System.Collections.Generic;
using MUNIA.Interop;
using MUNIA.Util;

namespace MUNIA.Controllers {
	public class ArduinoNgc : ArduinoController {
		public ArduinoNgc(SerialPortInfo port) : base(port) {
			_buttons.EnsureSize(12);
			_axes.EnsureSize(6);
			_hats.EnsureSize(1);
		}
		public override ControllerType Type => ControllerType.NGC;

		protected override bool Parse(List<byte> buffer) {
			if (buffer.Count != 64) return false;
			var packet = Repack(buffer);

			// 0 0 0 START Y X B A
			_buttons[0] = (packet[0] & 0x01) != 0;
			_buttons[1] = (packet[0] & 0x02) != 0;
			_buttons[2] = (packet[0] & 0x04) != 0;
			_buttons[3] = (packet[0] & 0x08) != 0;
			_buttons[4] = (packet[0] & 0x10) != 0;

			// L R Z
			_buttons[5] = (packet[1] & 0x10) != 0;
			_buttons[6] = (packet[1] & 0x20) != 0;
			_buttons[7] = (packet[1] & 0x40) != 0;
			
			byte bhat = (byte)(packet[1] & 0x0F);
			Hat hat = Hat.None;
			if ((bhat & 0x01) != 0) hat |= Hat.Left;
			if ((bhat & 0x02) != 0) hat |= Hat.Right;
			if ((bhat & 0x04) != 0) hat |= Hat.Down;
			if ((bhat & 0x08) != 0) hat |= Hat.Up;
			_hats[0] = hat;

	        _buttons[8] = hat.HasFlag(Hat.Up);
            _buttons[9] = hat.HasFlag(Hat.Down);
            _buttons[10] = hat.HasFlag(Hat.Left);
            _buttons[11] = hat.HasFlag(Hat.Right);

			_axes[0] = (packet[2] - 128) / 128.0;
			_axes[1] = (128 - packet[3]) / 128.0;
			_axes[2] = packet[6] / 256.0;
			_axes[3] = (packet[4] - 128) / 128.0;
			_axes[4] = (128 - packet[5]) / 128.0; 
			_axes[5] = packet[7] / 256.0; 

			return true;
		}
		public override bool IsAxisTrigger(int axisNum) => axisNum == 2 || axisNum == 5;

		public override string ToString() {
			return "NSpy NGC @ " + Port?.PortName;
		}
	}
}