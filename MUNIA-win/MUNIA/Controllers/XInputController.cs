using System;
using System.Collections.Generic;
using MUNIA.Interop;
using MUNIA.Util;
using SharpDX.XInput;

namespace MUNIA.Controllers {
	public class XInputController : GenericController {
		private Controller _controller;
		private int _previousPacketNumber = -1;
		private bool _activated;

		public XInputController() {
			_axes.EnsureSize(6); // 2 sticks, 2 triggers
			_buttons.EnsureSize(11 + 4); // 11 buttons + 4 for dpad
			_hats.EnsureSize(1);
		}

		public XInputController(Controller controller) : this() {
			_controller = controller;
		}

		public override bool IsAvailable => _controller.IsConnected;

		public override string DevicePath => "XINPUT:" + (int)_controller.UserIndex;

		public override string Name => DevicePath;

		public override ControllerType Type => ControllerType.XInput;

		public int Index {
			get => (int?)_controller?.UserIndex ?? -1;
			set => _controller = new Controller((UserIndex)value);
		}

		public override bool RequiresPolling => true;

		public override bool Activate() {
			_activated = true;
			// XInput is always active
			return true;
		}

		public override void Deactivate() {
			_activated = false;
		}

		public override ControllerState GetState() {
			// the mechanism with XInput require that we poll GetState
			// even when the device is removed, as this is how we detect that.
			// When we do, OnDeviceDetached is called and the subscriber should deactivate.
			if (!_activated) return base.GetState();

			// convert XInput state t internal state
			try {
				// if device disconnected this may fail
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
					_buttons[i++] = false /*|| (state.Gamepad.Buttons & GamepadButtonFlags.Logo) != 0*/; // todo XBOX button

					Hat hat = Hat.None;
					if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0) hat |= Hat.Up;
					if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0) hat |= Hat.Down;
					if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0) hat |= Hat.Left;
					if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0) hat |= Hat.Right;
					_hats[0] = hat;

					// for skinning simplicity sake, map hats also to buttons
					for (int h = 0; h < _hats.Count; h++) {
						// UP DOWN LEFT RIGHT
						_buttons[i++] = _hats[h].HasFlag(Hat.Up);
						_buttons[i++] = _hats[h].HasFlag(Hat.Down);
						_buttons[i++] = _hats[h].HasFlag(Hat.Left);
						_buttons[i++] = _hats[h].HasFlag(Hat.Right);
					}

					i = 0;
					_axes[i++] = state.Gamepad.LeftThumbX / (double)short.MaxValue;
					_axes[i++] = state.Gamepad.LeftThumbY / -(double)short.MaxValue;
					_axes[i++] = state.Gamepad.LeftTrigger / (double)byte.MaxValue;
					_axes[i++] = state.Gamepad.RightThumbX / (double)short.MaxValue;
					_axes[i++] = state.Gamepad.RightThumbY / -(double)short.MaxValue;
					_axes[i++] = state.Gamepad.RightTrigger / (double)byte.MaxValue;
				}

				return base.GetState();
			}
			catch {
				return base.GetState();
			}
		}

		public override bool IsAxisTrigger(int axisNum) {
			return axisNum == 2 || axisNum >= 5;
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

		public override void Dispose() {
		}

	}
}
