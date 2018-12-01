namespace MUNIA.Controllers {
	public enum ControllerType {
		None,
		SNES,
		N64,
        NGC,
        PS2,
		Generic,
		XInput,
        Unknown
	}

	public static class ControllerFactory {
		public static IController MakeController(ControllerType t) {
			switch (t) {
			case ControllerType.SNES:
				return new MuniaSnes(null);
			case ControllerType.N64:
				return new MuniaN64(null);
            case ControllerType.NGC:
                return new MuniaNgc(null);
			case ControllerType.Generic:
				return new RawInputController();
			case ControllerType.XInput:
				return new XInputController();
			default:
				return null;
			}
		}
	}
}