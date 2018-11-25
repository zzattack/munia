using HidSharp;
using MUNIA.Util;

namespace MUNIA.Controllers {
	public class MuniaSnes : MuniaController {
		public MuniaSnes(HidDevice hidDevice) : base(hidDevice) {
			_buttons.EnsureSize(12);
			_hats.EnsureSize(1);
		}
		
		public override ControllerType Type => ControllerType.SNES;
		public override string Name => string.IsNullOrEmpty(base.Name) ? "MUNIA SNES" : base.Name;

		protected override bool Parse(byte[] ev) {
			// B Y SEL START
			int i = 0, mask = 0x10;
			_buttons[i++] = (ev[1] & mask) != 0; mask <<= 1;
			_buttons[i++] = (ev[1] & mask) != 0; mask <<= 1;
			_buttons[i++] = (ev[1] & mask) != 0; mask <<= 1;
			_buttons[i++] = (ev[1] & mask) != 0; mask <<= 1;

			// A X L R
			mask = 0x10;
			_buttons[i++] = (ev[2] & mask) != 0; mask <<= 1;
			_buttons[i++] = (ev[2] & mask) != 0; mask <<= 1;
			_buttons[i++] = (ev[2] & mask) != 0; mask <<= 1;
			_buttons[i++] = (ev[2] & mask) != 0; mask <<= 1;

            // UP DOWN LEFT RIGHT
            Hat hat = ControllerState.HatLookup[(byte)(ev[1] & 0x0F)];
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