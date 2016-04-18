using System.Collections.Generic;
using SharpLib.Hid;

namespace MuniaInput {
    public class NgcController : MuniaController {
        private readonly List<bool> _buttons;
        private readonly List<int> _axes;

        public NgcController(Device hidDevice) : base(hidDevice) {
            _buttons = new List<bool>();
            for (int i = 0; i < 12; i++) _buttons.Add(false);
            _axes = new List<int>();
            for (int i = 0; i < 6; i++) _axes.Add(0);
        }

        public override List<int> Axes => _axes;
        public override List<bool> Buttons => _buttons;
        protected override bool Parse(Event ev) {
            if (ev.Device.Product != "NinHID NGC") return false;

            // 0 0 0 START Y X B A
            _buttons[0] = (ev.InputReport[1] & 0x10) != 0;
            _buttons[1] = (ev.InputReport[1] & 0x08) != 0;
            _buttons[2] = (ev.InputReport[1] & 0x04) != 0;
            _buttons[3] = (ev.InputReport[1] & 0x02) != 0;
            _buttons[4] = (ev.InputReport[1] & 0x01) != 0;

            // L R Z
            _buttons[5] = (ev.InputReport[2] & 0x40) != 0;
            _buttons[6] = (ev.InputReport[2] & 0x20) != 0;
            _buttons[7] = (ev.InputReport[2] & 0x10) != 0;

            // HAT, convert first
            byte hat = HatLookup[(byte)(ev.InputReport[2] & 0x0F)];
            // UP DOWN LEFT RIGHT
            _buttons[8] = (hat & 0x08) != 0;
            _buttons[9] = (hat & 0x04) != 0;
            _buttons[10] = (hat & 0x02) != 0;
            _buttons[11] = (hat & 0x01) != 0;

            _axes[0] = ev.InputReport[3] - 128;
            _axes[1] = ev.InputReport[4] - 128;
            _axes[2] = ev.InputReport[5] - 128;
            _axes[3] = ev.InputReport[6] - 128;
            _axes[4] = ev.InputReport[7];
            _axes[5] = ev.InputReport[8];

            return true;
        }
    }
}