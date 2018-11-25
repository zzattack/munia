using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HidSharp;
using MUNIA.Controllers;
using MUNIA.Interop;

namespace MUNIA.Forms {
	public partial class GamepadTester : Form {
		List<MuniaController> munias;
		List<ArduinoController> nspys;
		List<GenericController> generics;
		private IController _activeController;

		public GamepadTester() {
			InitializeComponent();

			UpdateDevices();
			lbMuniaDevices.SelectedItem = null;
			lbNintendoSpyDevices.SelectedItem = null;
			lbGenericDevices.SelectedItem = null;
			UpdateUI();

			UsbNotification.DeviceArrival += (sender, args) => UpdateDevices();
			UsbNotification.DeviceRemovalComplete += (sender, args) => UpdateDevices();
		}
		
		public GamepadTester(IController selectedController) : this() {
			if (selectedController is MuniaController mc)
				lbMuniaDevices.SelectedItem = munias.FirstOrDefault(d => d.DevicePath == mc.DevicePath);
			else if (selectedController is ArduinoController ac)
				lbNintendoSpyDevices.SelectedItem = nspys.FirstOrDefault(d => d.DevicePath == ac.DevicePath);
			else if (selectedController is GenericController gc)
				lbGenericDevices.SelectedItem = generics.FirstOrDefault(d => d.DevicePath == gc.DevicePath); ;
		}

		private void btnTestMUNIA_Click(object sender, EventArgs e) {
			StartTesting(lbMuniaDevices.SelectedItem as MuniaController);
		}

		private void btnTestNSpy_Click(object sender, EventArgs e) {
			StartTesting(lbNintendoSpyDevices.SelectedItem as ArduinoController);
		}

		private void btnTestGeneric_Click(object sender, EventArgs e) {
			StartTesting(lbGenericDevices.SelectedItem as GenericController);
		}

		private void StartTesting(IController controller) {
			StopTesting();
			if (controller != null) {
				_activeController = controller;
				_activeController.StateUpdated += OnControllerStateUpdated;
				if (_activeController.Activate()) {
					rtb.AppendText($"Started testing controller {_activeController.Name}, press some buttons\r\n");
					statePainter.UpdateState(_activeController.GetState());
					statePainter.Invalidate();
				}
				else {
					rtb.AppendText($"Could not activate controller\r\n");
				}
			}
			else {
				rtb.AppendText("Select a valid controller first\r\n");
			}
		}

		private void StopTesting() {
			if (_activeController != null) {
				_activeController.StateUpdated -= OnControllerStateUpdated;
				_activeController.Deactivate();
				rtb.AppendText($"Stopped testing controller {_activeController.Name}\r\n");
				_activeController = null;
			}
		}

		private void OnControllerStateUpdated(object sender, EventArgs e) {
			if (InvokeRequired) BeginInvoke((Action<object, EventArgs>)OnControllerStateUpdated, sender, e);
			else {
				statePainter.UpdateState(_activeController.GetState());
				statePainter.Invalidate();
			}
		}

		void UpdateDevices() {
			munias = MuniaController.ListDevices().ToList();
			lbMuniaDevices.DataSource = munias;

			nspys = ArduinoController.ListDevices().ToList();
			lbNintendoSpyDevices.DataSource = nspys;

			generics = GenericController.ListDevices().ToList();
			lbGenericDevices.DataSource = generics;
		}
		void UpdateUI(object sender, EventArgs args) {
			UpdateUI();
		}
		void UpdateUI() {
			btnTestMUNIA.Enabled = lbMuniaDevices.SelectedItem is MuniaController;
			btnTestNSpy.Enabled = lbNintendoSpyDevices.SelectedItem is ArduinoController;
			btnTestGeneric.Enabled = lbGenericDevices.SelectedItem is GenericController;
		}

		private void lbMuniaDevices_MouseDoubleClick(object sender, MouseEventArgs e) {
			int index = lbMuniaDevices.IndexFromPoint(e.Location);
			if (index != ListBox.NoMatches)
				btnTestMUNIA_Click(null, null);
		}

		private void lbNintendoSpyDevices_MouseDoubleClick(object sender, MouseEventArgs e) {
			int index = lbNintendoSpyDevices.IndexFromPoint(e.Location);
			if (index != ListBox.NoMatches)
				btnTestNSpy_Click(null, null);
		}

		private void lbGenericDevices_MouseDoubleClick(object sender, MouseEventArgs e) {
			int index = lbGenericDevices.IndexFromPoint(e.Location);
			if (index != ListBox.NoMatches)
				btnTestGeneric_Click(null, null);
		}
	}

	public sealed class ControllerStatePainter : Control {
		private ControllerState _state;
		public ControllerStatePainter() {
			DoubleBuffered = true;
		}
		public void UpdateState(ControllerState state) {
			_state = state;
		}

		protected override void OnPaint(PaintEventArgs pe) {
			var gfx = pe.Graphics;
			gfx.Clear(BackColor);
			if (_state == null) {
				gfx.DrawString("No controller state yet, press some buttons", DefaultFont, Brushes.Black, 5, 5);
			}
			else {
				int x = 10;
				gfx.DrawString("Hats", DefaultFont, Brushes.Black, 5, 5);
				for (int i = 0; i < _state.Hats.Count; i++) {
					var hat = _state.Hats[i];
					DrawHat(gfx, i, hat, x, 20);
					x += 100;
				}

				x = 10;
				gfx.DrawString("Buttons", DefaultFont, Brushes.Black, 5, 45);
				for (int i = 0; i < _state.Buttons.Count; i++) {
					DrawButton(gfx, i, _state.Buttons[i], x, 60);
					x += 40;
				}

				x = 10;
				gfx.DrawString("Axes", DefaultFont, Brushes.Black, 5, 85);
				for (int i = 0; i < _state.Axes.Count; i++) {
					DrawAxis(gfx, i, _state.Axes[i], x, 100);
					x += 40;
				}
			}
		}

		private void DrawHat(Graphics gfx, int hatNum, Hat hat, int x, int y) {
			gfx.DrawString($"{hatNum}: {hat}", DefaultFont, Brushes.Black, x, y);
		}

		private void DrawButton(Graphics gfx, int buttonNum, bool buttonState, int x, int y) {
			gfx.DrawString($"{buttonNum}: {(buttonState?"X":"O")}", DefaultFont, Brushes.Black, x, y);
		}

		private void DrawAxis(Graphics gfx, int axisNum, double axisValue, int x, int y) {
			gfx.DrawString($"{axisNum}: {axisValue:f2}", DefaultFont, Brushes.Black, x, y);
		}

	}
}
