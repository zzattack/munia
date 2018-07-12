using HidSharp;

namespace MUNIA.Controllers {
	public abstract class ConfigInterface {
		protected HidDevice _dev;

		public ConfigInterface(HidDevice dev) {
			this._dev = dev;
		}

		public HidStream Open() { return _dev.Open(); }
		public bool TryOpen(out HidStream stream) {
			return _dev.TryOpen(out stream);
		}
		public HidDevice GetDevice() {
			return _dev;
		}
	}


	public class MuniaConfigInterface : ConfigInterface {
		public MuniaConfigInterface(HidDevice dev) : base(dev) { }
		public override string ToString() {
			return "MUNIA " + _dev.GetSerialNumber();
		}
	}

	public class MusiaConfigInterface : ConfigInterface {
		public MusiaConfigInterface(HidDevice dev) : base(dev) { }
		public override string ToString() {
			return "MUSIA " + _dev.GetSerialNumber();
		}

	}
}