using System.Collections.Generic;
using HidSharp;

namespace MuniaInput {
    public class N64Controller : MuniaController {
        private readonly List<bool> _buttons;
        private readonly List<int> _axes;

        public N64Controller(HidDevice hidDevice) : base(hidDevice) {
            _buttons = new List<bool>();
            for (int i = 0; i < 14; i++) _buttons.Add(false);
            _axes = new List<int>();
            for (int i = 0; i < 2; i++) _axes.Add(0);
        }

        public override List<int> Axes => _axes;
        public override List<bool> Buttons => _buttons;
        protected override bool Parse(byte[] ev) {
            // A B Z START
            _buttons[0] = (ev[1] & 0x80) != 0;
            _buttons[1] = (ev[1] & 0x40) != 0;
            _buttons[2] = (ev[1] & 0x20) != 0;
            _buttons[3] = (ev[1] & 0x10) != 0;

            // _ _ L R CUP CDOWN CLEFT CRIGHT
            _buttons[4] = (ev[2] & 0x20) != 0;
            _buttons[5] = (ev[2] & 0x10) != 0;
            _buttons[6] = (ev[2] & 0x08) != 0;
            _buttons[7] = (ev[2] & 0x04) != 0;
            _buttons[8] = (ev[2] & 0x02) != 0;
            _buttons[9] = (ev[2] & 0x01) != 0;

            // HAT, convert first
            byte hat = HatLookup[(byte)(ev[1] & 0x0F)];
            // UP DOWN LEFT RIGHT
            _buttons[10] = (hat & 0x08) != 0;
            _buttons[11] = (hat & 0x04) != 0;
            _buttons[12] = (hat & 0x02) != 0;
            _buttons[13] = (hat & 0x01) != 0;

            _axes[0] = ev[3] - 128;
            _axes[1] = ev[4] - 128;

            return true;
        }
    }
}