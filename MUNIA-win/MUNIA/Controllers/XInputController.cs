using System.Collections.Generic;
using MUNIA.Util;
using SharpDX.XInput;

namespace MUNIA.Controllers {
	public class XInputController : GenericController {
		private readonly Controller _controller;
		private int _previousPacketNumber = -1;

		public XInputController(Controller controller) {
			this._controller = controller;
			Index = (int)controller.UserIndex;

			_axes.EnsureSize(6); // 2 sticks, 2 triggers
			_buttons.EnsureSize(12);
			_hats.EnsureSize(1);
		}


		public override bool IsAvailable => _controller.IsConnected;

		public override string DevicePath => "XINPUT:" + (int)_controller.UserIndex;

		public override string Name => DevicePath;

		public int Index { get; private set; }

		public override bool RequiresPolling => true;

		public override bool Activate() {
			// XInput is always active
			return true;
		}

		public override void Deactivate() {
		}

		public override ControllerState GetState() {
			// 
			State state = _controller.GetState();
			if (state.PacketNumber != _previousPacketNumber) {
				_previousPacketNumber = state.PacketNumber;
				// map buttons as close to the raw input version as possible
				int i = 0;
				_buttons[i++] = (state.Gamepad.Buttons & GamepadButtonFlags.A) != 0;
				_buttons[i++] = (state.Gamepad.Buttons & GamepadButtonFlags.B) != 0;
				_buttons[i++] = (state.Gamepad.Buttons & GamepadButtonFlags.X) != 0;
				_buttons[i++] = (state.Gamepad.Buttons & GamepadButtonFlags.Y) != 0;
				_buttons[i++] = (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0;
				_buttons[i++] = (state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0;
				_buttons[i++] = (state.Gamepad.Buttons & GamepadButtonFlags.Back) != 0;
				_buttons[i++] = (state.Gamepad.Buttons & GamepadButtonFlags.Start) != 0;
				_buttons[i++] = (state.Gamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0;
				_buttons[i++] = (state.Gamepad.Buttons & GamepadButtonFlags.RightThumb) != 0;
				// remaining buttons are not available when using raw input
				_buttons[i++] = (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0;
				_buttons[i++] = (state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0;
				
				i = 0;
				_axes[i++] = state.Gamepad.LeftThumbX / 32768.0;
				_axes[i++] = state.Gamepad.LeftThumbY / 32768.0;
				_axes[i++] = state.Gamepad.LeftTrigger / 65535.0;
				_axes[i++] = state.Gamepad.RightThumbX / 32768.0;
				_axes[i++] = state.Gamepad.RightThumbY / 32768.0;
				_axes[i++] = state.Gamepad.RightTrigger / 65535.0;

				Hat hat = Hat.None;
				if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0) hat |= Hat.Up;
				if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0) hat |= Hat.Down;
				if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0) hat |= Hat.Left;
				if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0) hat |= Hat.Right;
				_hats[0] = hat;
			}

			return base.GetState();
		}

		public override bool IsAxisTrigger(int axisNum) {
			return axisNum > 4;
		}

		public new static IEnumerable<XInputController> ListDevices() {
			var indices = new[] { UserIndex.One, UserIndex.Two, UserIndex.Three, UserIndex.Four };
			// find all devices with a gamepad or joystick usage page
			foreach (var index in indices) {
				var controller = new Controller(index);

				if (controller.IsConnected)
					yield return new XInputController(controller);
			}
		}

	}
}
