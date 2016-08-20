using System.Collections.Generic;
using HidSharp;

namespace MUNIA.Controllers {
    public class SnesController : MuniaController {
        private readonly List<bool> _buttons = new List<bool>(12); 

        public SnesController(HidDevice hidDevice) : base(hidDevice) {
            for (int i = 0; i < 12; i++) _buttons.Add(false);
        }

        protected override List<int> Axes => new List<int>();
        protected override List<bool> Buttons => _buttons;

        protected override bool Parse(byte[] ev) {
            // B Y SEL START
            int i = 0, mask = 0x80;
            _buttons[i] = (ev[1] & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (ev[1] & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (ev[1] & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (ev[1] & mask) != 0; mask >>= 1; i++;

            // A X L R
            mask = 0x80;
            _buttons[i] = (ev[2] & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (ev[2] & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (ev[2] & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (ev[2] & mask) != 0; mask >>= 1; i++;

            // HAT, convert first
            byte hat = HatLookup[(byte)(ev[1] & 0x0F)];
            // UP DOWN LEFT RIGHT
            _buttons[i] = (hat & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (hat & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (hat & mask) != 0; mask >>= 1; i++;
            _buttons[i] = (hat & mask) != 0;
            return true;
        }
        
    }
}