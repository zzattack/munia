using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MUNIA.Controllers;
using MUNIA.Interop;

namespace MUNIA.Forms {
	public partial class DevicePicker : Form {
		private readonly IList<ConfigInterface> _devices;
		public ConfigInterface ChosenDevice { get; private set; }

		private DevicePicker() {
			InitializeComponent();
			UsbNotification.DeviceArrival += RefreshDeviceList;
		}

		private void RefreshDeviceList(object sender, UsbNotificationEventArgs e) {
			lbDevices.DataSource = MuniaController.GetConfigInterfaces().ToList();
		}

		public DevicePicker(IList<ConfigInterface> devices) : this() {
			_devices = devices;
			lbDevices.DataSource = devices;
		}


		private void BtnAccept_Click(object sender, EventArgs e) {
			ChosenDevice = lbDevices.SelectedItem as ConfigInterface;
		}

		private void lbDevices_SelectedIndexChanged(object sender, EventArgs e) {
			btnAccept.Enabled = lbDevices.SelectedItem is ConfigInterface;
		}

		private void lbDevices_MouseDoubleClick(object sender, MouseEventArgs e) {
			int index = this.lbDevices.IndexFromPoint(e.Location);
			if (index != ListBox.NoMatches) {
				ChosenDevice = lbDevices.Items[index] as ConfigInterface;
				DialogResult = DialogResult.OK;
				Close();
			}
		}

	}

}