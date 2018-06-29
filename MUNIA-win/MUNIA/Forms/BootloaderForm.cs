using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HidSharp;
using MUNIA.Bootloader;
using MUNIA.Controllers;
using MUNIA.Interop;
using MUNIA.Properties;

namespace MUNIA.Forms {
	public partial class BootloaderForm : Form {
		private readonly HidDeviceLoader _loader = new HidDeviceLoader();
		private HidDevice _blDevice;
		private HidBootloader _blInterface;
		private IntelHexFile _hexFile;
		const int VID = 0x04D8;
		const int PID_BL = 0x003c;
		const int PID_MUNIA = 0x0058;

		public BootloaderForm() {
			InitializeComponent();
			UsbNotification.DeviceArrival += UsbDeviceListChanged;
			UsbNotification.DeviceRemovalComplete += UsbDeviceListChanged;
			UpdateUI();
		}

		private void UpdateUI() {
			// enter bootloader button is enabled if vid/pid of MUNIA
			// is observed, if so all other functions are disabled
			var muniaInterfaces = _loader.GetDevices(VID, PID_MUNIA);
			if (muniaInterfaces.Count() > 1) {
				// munia detected, so it's not in BL, but we can send a packet to request this
				tsbLoadHex.Enabled = tsbProgram.Enabled = tsbReset.Enabled = false;
				_blDevice = null;
				_blInterface = null;
				imgBlStatus.Image = Properties.Resources.warn;
				lblStatus.Text = "Status: user";
				imgBlStatus.ToolTipText = "MUNIA is currently in user mode.\r\nReboot it in bootloader mode to continue.";
				tsbEnterBootloader.Enabled = true;
				lblHEX.Visible = imgHexStatus.Visible = pbFlash.Visible = lblProgress.Visible = false;
				tsbLoadHex.Enabled = tsbProgram.Enabled = tsbReset.Enabled = false;
				return;
			}
			else
				tsbEnterBootloader.Enabled = false;

			// see if there is a selected device, and whether is still valid
			HidStream s = null;
			bool blDeviceOk = false;
			if (_blDevice != null && _blDevice.TryOpen(out s)) {
				s.Dispose();
				blDeviceOk = true;
			}
			else
				_blDevice = null;

			// if no device selected, see if we can find one
			if (_blDevice == null) {
				_blDevice = _loader.GetDeviceOrDefault(VID, PID_BL);
				blDeviceOk = _blDevice != null && _blDevice.TryOpen(out s);
				if (blDeviceOk) {
					s.Dispose();
					_blInterface = new HidBootloader(_blDevice);
				}
			}

			if (blDeviceOk) {
				imgBlStatus.Image = Resources.ok;
				lblStatus.Text = "Status: in BL";
				lblStatus.ToolTipText = "MUNIA is currently in bootloader mode,\r\n and is ready to be flashed.";
			}
			else {
				imgBlStatus.Image = Resources.notok;
				lblStatus.Text = "Status: not found";
				lblStatus.ToolTipText = "MUNIA is currently not plugged in or malfunctioning.";
				lblHEX.Visible = imgHexStatus.Visible = pbFlash.Visible = lblProgress.Visible = false;
				tsbLoadHex.Enabled = tsbProgram.Enabled = tsbReset.Enabled = false;
				return;
			}

			lblHEX.Visible = imgHexStatus.Visible = pbFlash.Visible = lblProgress.Visible = true;
			tsbLoadHex.Enabled = true;
			tsbReset.Enabled = true;
			if (_hexFile?.Size > 0x100) {
				tsbProgram.Enabled = true;
				imgHexStatus.Image = Resources.ok;
				imgHexStatus.ToolTipText = "Firmware hex file is successfully loaded.";
			}
			else {
				tsbProgram.Enabled = false;
				imgHexStatus.Image = Resources.notok;
				imgHexStatus.ToolTipText = "Load a firmware hex file to flash first.";
			}
		}

		private void UsbDeviceListChanged(object sender, UsbNotificationEventArgs args) {
			UpdateUI();
		}

		private void tsbEnterBootloader_Click(object sender, EventArgs e) {
			var dev = MuniaController.GetMuniaConfigInterfaces().FirstOrDefault();
			if (dev == null) {
				MessageBox.Show("Interface not found");
				return;
			}
			try {
				using (var stream = dev.Open()) {
					byte[] buff = new byte[9];
					buff[0] = 0;
					buff[1] = 0x48; // CFG_CMD_RESET_BL
					stream.Write(buff, 0, buff.Length);
				}
			}
			catch (Win32Exception) { }
			catch (IOException) { }
			catch (TimeoutException) { }
			catch (Exception exc) {
				MessageBox.Show("An error occurred while retrieving information from the MUNIA device:\r\n\r\n" + exc,
					"Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void tsbLoadHex_Click(object sender, EventArgs e) {
			if (ofd.ShowDialog() == DialogResult.OK) {
				_hexFile = new IntelHexFile(ofd.FileName);
				UpdateUI();
			}
		}

		private void tsbProgram_Click(object sender, EventArgs e) {
			try {
				pbFlash.Value = 3;

				_blInterface.Query();
				pbFlash.Value = 10;

				_blInterface.Erase();
				pbFlash.Value = 30;

				_blInterface.ProgressChanged += (o, args) => pbFlash.Value = (int)(30.0 + args.ProgressPercentage / 100.0 * 50.0);
				_blInterface.Program(_hexFile, false);

				if (_blInterface.Verify(_hexFile)) {
					pbFlash.Value = 90;

					_blInterface.SignFlash();
					pbFlash.Value = 100;
					MessageBox.Show("Firmware flashed successfully", "Success");
				}
			}
			catch (Exception exc) {
				MessageBox.Show("Unknown error occurred during firmware flashing: " + exc.Message,
					"Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				throw;
			}
		}
		
		private void tsbReset_Click(object sender, EventArgs e) {
			_blInterface?.Reset();
		}
	}
}
