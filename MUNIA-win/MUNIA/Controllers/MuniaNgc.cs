using HidSharp;
using MUNIA.Util;

namespace MUNIA.Controllers {
    public class MuniaNgc : MuniaController {
        public MuniaNgc(HidDevice hidDevice) : base(hidDevice) {
			_buttons.EnsureSize(12);
			_axes.EnsureSize(6);
			_hats.EnsureSize(1);
        }
		
	    public override ControllerType Type => ControllerType.NGC;
		public override string Name => string.IsNullOrEmpty(base.Name) ? "MUNIA NGC" : base.Name;
        protected override bool Parse(byte[] ev) {
            // 0 0 0 START Y X B A
            _buttons[0] = (ev[1] & 0x01) != 0;
            _buttons[1] = (ev[1] & 0x02) != 0;
            _buttons[2] = (ev[1] & 0x04) != 0;
            _buttons[3] = (ev[1] & 0x08) != 0;
            _buttons[4] = (ev[1] & 0x10) != 0;

            // L R Z
            _buttons[5] = (ev[2] & 0x10) != 0;
            _buttons[6] = (ev[2] & 0x20) != 0;
            _buttons[7] = (ev[2] & 0x40) != 0;

            // HAT, convert first
            Hat hat = ControllerState.HatLookup[(byte)(ev[2] & 0x0F)];
			_hats[0] = hat;

            // UP DOWN LEFT RIGHT
	        _buttons[8] = hat.HasFlag(Hat.Up);
            _buttons[9] = hat.HasFlag(Hat.Down);
            _buttons[10] = hat.HasFlag(Hat.Left);
            _buttons[11] = hat.HasFlag(Hat.Right);

            _axes[0] = (ev[3] - 128) / 128.0;
            _axes[1] = (ev[4] - 128) / 128.0;
			_axes[2] = ev[7] / 256.0;
			_axes[3] = (ev[5] - 128) / 128.0;
			_axes[4] = (ev[6] - 128) / 128.0;
            _axes[5] = ev[8] / 256.0;

            return true;
		}
		public override bool IsAxisTrigger(int axisNum) => axisNum == 2 || axisNum == 5;
	}
}