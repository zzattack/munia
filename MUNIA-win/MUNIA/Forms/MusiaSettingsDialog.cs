using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using HidSharp;
using MUNIA.Controllers;

namespace MUNIA.Forms {
	public partial class MusiaSettingsDialog : Form {
		private readonly MusiaConfigInterface _intf;
		private MusiaDeviceInfo _deviceInfo;

		private MusiaSettingsDialog() {
			InitializeComponent();
			cbPollingFrequency.DataSource = Enum.GetValues(typeof(MusiaDeviceInfo.PollingFrequencySetting));
		}

		public MusiaSettingsDialog(MusiaConfigInterface intf) : this() {
			_intf = intf;

			using (HidStream stream = intf.Open()) {
				var frInfo = new byte[9];
				frInfo[0] = MusiaDeviceInfo.CFG_CMD_INFO;
				stream.GetFeature(frInfo);

				var frConfig = new byte[9];
				frConfig[0] = MusiaDeviceInfo.CFG_CMD_READ;
				stream.GetFeature(frConfig);

				var deviceInfo = new MusiaDeviceInfo();
				if (deviceInfo.Parse(frInfo, frConfig)) {
					LoadDeviceInfo(deviceInfo);
				}
				else {
					throw new InvalidOperationException("Invalid settings container received from MUNIA device");
				}
			}
		}

		private void LoadDeviceInfo(MusiaDeviceInfo deviceInfo) {
			_deviceInfo = deviceInfo;

			tbFirmware.Text = deviceInfo.Version.ToString(2);
			tbHardware.Text = deviceInfo.HardwareRevision.ToString();
			tbMCUId.Text = _intf.GetDevice().GetSerialNumber();
			tbMicroController.Text = deviceInfo.DeviceTypeName;

			rbOutputPS2.Checked = deviceInfo.Output == MusiaDeviceInfo.OutputMode.PS2;
			rbOutputPC.Checked = deviceInfo.Output == MusiaDeviceInfo.OutputMode.PC;
			cbPollingFrequency.SelectedItem = deviceInfo.PollingFrequency;
		}

		private void btnAccept_Click(object sender, EventArgs e) {
			_deviceInfo.Output = rbOutputPC.Checked ? MusiaDeviceInfo.OutputMode.PC : MusiaDeviceInfo.OutputMode.PS2;
			_deviceInfo.PollingFrequency = (MusiaDeviceInfo.PollingFrequencySetting)cbPollingFrequency.SelectedItem;
			try {
				using (HidStream stream = _intf.Open()) {
					var report = _deviceInfo.ToWriteReport();
					stream.SetFeature(report);
					Task.Run(()=> MessageBox.Show("Config updated successfully", "Success"));
				}
			}
			catch {
				MessageBox.Show("Report saving failed"); throw;
			}
		}
	}
}