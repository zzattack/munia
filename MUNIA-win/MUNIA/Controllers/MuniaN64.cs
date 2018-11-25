using HidSharp;
using MUNIA.Util;

namespace MUNIA.Controllers {
    public class MuniaN64 : MuniaController {
        public MuniaN64(HidDevice hidDevice) : base(hidDevice) {
			_buttons.EnsureSize(14);
			_axes.EnsureSize(2);
			_hats.EnsureSize(1);
        }
		
	    public override ControllerType Type => ControllerType.N64;
		public override string Name => string.IsNullOrEmpty(base.Name) ? "MUNIA N64" : base.Name;
        protected override bool Parse(byte[] ev) {
            // A B Z START
            _buttons[0] = (ev[1] & 0x10) != 0;
            _buttons[1] = (ev[1] & 0x20) != 0;
            _buttons[2] = (ev[1] & 0x40) != 0;
            _buttons[3] = (ev[1] & 0x80) != 0;

            // _ _ L R CUP CDOWN CLEFT CRIGHT
            _buttons[4] = (ev[2] & 0x01) != 0;
			_buttons[5] = (ev[2] & 0x02) != 0;
            _buttons[6] = (ev[2] & 0x04) != 0;
            _buttons[7] = (ev[2] & 0x08) != 0;
            _buttons[8] = (ev[2] & 0x10) != 0;
            _buttons[9] = (ev[2] & 0x20) != 0;
            
            // UP DOWN LEFT RIGHT
            Hat hat = ControllerState.HatLookup[(byte)(ev[1] & 0x0F)];
			_hats[0] = hat;

	        _buttons[10] = hat.HasFlag(Hat.Up);
            _buttons[11] = hat.HasFlag(Hat.Down);
            _buttons[12] = hat.HasFlag(Hat.Left);
            _buttons[13] = hat.HasFlag(Hat.Right);

            _axes[0] = (ev[3] - 128) / 128.0;
            _axes[1] = (ev[4] - 128) / 128.0;
			
            return true;
		}

		public override bool IsAxisTrigger(int axisNum) => false;
	}
}