using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MUNIA.Controllers;
using MUNIA.Interop;

namespace MUNIA.Forms {
	public partial class GamepadTesterForm : Form {
		List<MuniaController> munias;
		List<ArduinoController> nspys;
		List<GenericController> generics;
		private IController _activeController;

		public GamepadTesterForm() {
			InitializeComponent();

			UpdateDevices();
			lbMuniaDevices.SelectedItem = null;
			lbNintendoSpyDevices.SelectedItem = null;
			lbGenericDevices.SelectedItem = null;
			UpdateUI();

			UsbNotification.DeviceArrival += (sender, args) => UpdateDevices();
			UsbNotification.DeviceRemovalComplete += (sender, args) => UpdateDevices();
		}

		public GamepadTesterForm(IController selectedController) : this() {
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

		private void StartTesting(IController controller) {
			if (controller != null) {
				if (gamepadViewer.StartTesting(controller)) {
					rtb.AppendText($"Started testing controller {controller.Name}, press some buttons\r\n");
				}
				else {
					rtb.AppendText($"Could not activate controller\r\n");
				}
			}
			else {
				rtb.AppendText("Select a valid controller first\r\n");
			}
		}

		private void btnTestNSpy_Click(object sender, EventArgs e) {
			StartTesting(lbNintendoSpyDevices.SelectedItem as ArduinoController);
		}

		private void btnTestGeneric_Click(object sender, EventArgs e) {
			StartTesting(lbGenericDevices.SelectedItem as GenericController);
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

		private void GamepadTesterForm_FormClosing(object sender, FormClosingEventArgs e) {
			gamepadViewer.StopTesting();
		}
	}
}
