using System.Collections.Generic;
using MuniaInput;
using SharpLib.Hid;

namespace MUNIA.Controllers {
    public class SnesController : MuniaController {
        private readonly List<bool> _buttons = new List<bool>(12); 

        public SnesController(Device hidDevice) : base(hidDevice) {
            for (int i = 0; i < 12; i++) _buttons.Add(false);
        }

        public override List<int> Axes => new List<int>();
        public override List<bool> Buttons => _buttons;

        protected override bool Parse(Event ev) {
            if (ev.Device.Product != "NinHID SNES") return false;

            // B Y SEL START
            int i = 0, mask = 0x80;
            _buttons[i] = (ev.InputReport[1] & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (ev.InputReport[1] & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (ev.InputReport[1] & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (ev.InputReport[1] & mask) != 0; mask >>= 1; i++;

            // A X L R
            mask = 0x80;
            _buttons[i] = (ev.InputReport[2] & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (ev.InputReport[2] & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (ev.InputReport[2] & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (ev.InputReport[2] & mask) != 0; mask >>= 1; i++;

            // HAT, convert first
            byte hat = HatLookup[(byte)(ev.InputReport[1] & 0x0F)];
            // UP DOWN LEFT RIGHT
            _buttons[i] = (hat & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (hat & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (hat & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (hat & mask) != 0;
            return true;
        }
        
    }
}