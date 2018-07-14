using System;
using System.Linq;
using System.Windows.Forms;
using HidSharp;
using MUNIA.Controllers;

namespace MUNIA.Forms {
	public partial class MuniaSettingsDialog : Form {
		private readonly MuniaConfigInterface _intf;
		private MuniaDeviceInfo _deviceInfo;

		private MuniaSettingsDialog() {
			InitializeComponent();
		}

		public MuniaSettingsDialog(MuniaConfigInterface intf) : this() {
			_intf = intf;
			using (HidStream stream = intf.Open()) {
				byte[] buff = new byte[9];
				buff[0] = 0;
				buff[1] = 0x47; // CFG_CMD_READ
				stream.Write(buff, 0, 9);
				stream.Read(buff, 0, 9);

				var deviceInfo = new MuniaDeviceInfo();
				if (deviceInfo.Parse(buff)) {
					LoadDeviceInfo(deviceInfo);
				}
				else {
					throw new InvalidOperationException("Invalid settings container received from MUNIA device: " +
						string.Join(" ", buff.Select(x => x.ToString("X2"))));
				}
			}
		}

		private void LoadDeviceInfo(MuniaDeviceInfo deviceInfo) {
			_deviceInfo = deviceInfo;
			_blockRecurse = true;

			tbFirmware.Text = $"{deviceInfo.VersionMajor}.{deviceInfo.VersionMinor}";
			tbHardware.Text = "rev" + deviceInfo.HardwareRevision;
			tbMCUId.Text = "0x" + deviceInfo.DeviceID.ToString("x4");
			tbMCURevision.Text = deviceInfo.HardwareRevision.ToString();

			lblDeviceType.Text += deviceInfo.DevType.ToString();
			if (deviceInfo.IsLegacy) {
				if (deviceInfo.SNES == MuniaDeviceInfo.SnesMode.SNES_MODE_NGC) rbSnesNgc.Checked = true;
				else if (deviceInfo.SNES == MuniaDeviceInfo.SnesMode.SNES_MODE_PC) rbSnesPC.Checked = true;
				else rbSnesConsole.Checked = true;
				if (deviceInfo.N64 == MuniaDeviceInfo.N64Mode.N64_MODE_N64) rbN64Console.Checked = true;
				else rbN64PC.Checked = true;
				if (deviceInfo.NGC == MuniaDeviceInfo.NGCMode.NGC_MODE_NGC) rbNgcConsole.Checked = true;
				else rbNgcPC.Checked = true;
			}
			else {
				if (deviceInfo.DevType == MuniaDeviceInfo.DeviceType.MuniaOriginal) {
					// munia revision 2
					if (deviceInfo.Output == MuniaDeviceInfo.OutputMode.PC) {
						rbOutputPC.Checked = true;
						ckbNGC.Checked = deviceInfo.Inputs.HasFlag(MuniaDeviceInfo.InputSources.NGC);
						ckbN64.Checked = deviceInfo.Inputs.HasFlag(MuniaDeviceInfo.InputSources.N64);
						ckbSNES.Checked = deviceInfo.Inputs.HasFlag(MuniaDeviceInfo.InputSources.SNES);
					}
					else {
						rbOutputNGC.Checked = deviceInfo.Output == MuniaDeviceInfo.OutputMode.NGC;
						rbOutputN64.Checked = deviceInfo.Output == MuniaDeviceInfo.OutputMode.N64;
						rbOutputSNES.Checked = deviceInfo.Output == MuniaDeviceInfo.OutputMode.SNES;
						rbInputNGC.Checked = deviceInfo.Inputs.HasFlag(MuniaDeviceInfo.InputSources.NGC);
						rbInputN64.Checked = deviceInfo.Inputs.HasFlag(MuniaDeviceInfo.InputSources.N64);
						rbInputSNES.Checked = deviceInfo.Inputs.HasFlag(MuniaDeviceInfo.InputSources.SNES);
					}
				}
				else if (deviceInfo.DevType == MuniaDeviceInfo.DeviceType.MuniaNgc) {
					// MUNIA-NGC
					if (deviceInfo.Output == MuniaDeviceInfo.OutputMode.PC)
						rbOutputPC.Checked = true;
					else
						rbOutputNGC.Checked = true;
					ckbNGC.Checked = true;
				}
			}

			_blockRecurse = false;
			UpdateUI();
		}

		private bool _blockRecurse;

		private void UpdateUI() {
			if (_blockRecurse) return;
			_blockRecurse = true;

			gbSettingsLegacy.Visible = _deviceInfo.IsLegacy;
			gbSettings.Visible = !_deviceInfo.IsLegacy;

			if (_deviceInfo.IsLegacy) {
				if (_deviceInfo.SNES == MuniaDeviceInfo.SnesMode.SNES_MODE_NGC) rbSnesNgc.Checked = true;
				else if (_deviceInfo.SNES == MuniaDeviceInfo.SnesMode.SNES_MODE_PC) rbSnesPC.Checked = true;
				else rbSnesConsole.Checked = true;

				if (_deviceInfo.N64 == MuniaDeviceInfo.N64Mode.N64_MODE_N64) rbN64Console.Checked = true;
				else rbN64PC.Checked = true;

				if (_deviceInfo.NGC == MuniaDeviceInfo.NGCMode.NGC_MODE_NGC) rbNgcConsole.Checked = true;
				else rbNgcPC.Checked = true;
			}
			else {

				if (_deviceInfo.DevType == MuniaDeviceInfo.DeviceType.MuniaOriginal) {
					pnlInputsPC.Visible = rbOutputPC.Checked;
					pnlInputs.Visible = !rbOutputPC.Checked;

					if (!rbOutputPC.Checked) {
						if (rbOutputNGC.Checked) {
							rbInputNGC.Enabled = true;
							rbInputN64.Enabled = _deviceInfo.Version >= new Version(1, 6);
							rbInputSNES.Enabled = true;
						}
						else if (rbOutputN64.Checked) {
							rbInputNGC.Enabled = true;
							rbInputN64.Enabled = true;
							rbInputSNES.Enabled = _deviceInfo.Version >= new Version(1, 6);
						}
						else if (rbOutputSNES.Checked) {
							rbInputNGC.Enabled = _deviceInfo.Version >= new Version(1, 6);
							rbInputN64.Enabled = _deviceInfo.Version >= new Version(1, 6);
							rbInputSNES.Enabled = true;
						}
					}
				}

				else if (_deviceInfo.DevType == MuniaDeviceInfo.DeviceType.MuniaNgc) {
					// MUNIA-NGC
					pnlInputs.Visible = true;
					pnlInputsPC.Visible = false;

					rbOutputPC.Enabled = true;
					rbOutputNGC.Enabled = true;
					rbOutputN64.Enabled = false;
					rbOutputSNES.Enabled = false;

					rbInputNGC.Checked = true;
					rbInputNGC.Enabled = false;
					rbInputN64.Enabled = false;
					rbInputN64.Checked = false;
					rbInputSNES.Enabled = false;
					rbInputSNES.Checked = false;
				}

			}

			_blockRecurse = false;
		}


		private void btnAccept_Click(object sender, System.EventArgs e) {
			if (_deviceInfo.IsLegacy) {
				if (rbSnesConsole.Checked) _deviceInfo.SNES = MuniaDeviceInfo.SnesMode.SNES_MODE_SNES;
				else if (rbSnesNgc.Checked) _deviceInfo.SNES = MuniaDeviceInfo.SnesMode.SNES_MODE_NGC;
				else _deviceInfo.SNES = MuniaDeviceInfo.SnesMode.SNES_MODE_PC;

				if (rbN64Console.Checked) _deviceInfo.N64 = MuniaDeviceInfo.N64Mode.N64_MODE_N64;
				else _deviceInfo.N64 = MuniaDeviceInfo.N64Mode.N64_MODE_PC;

				if (rbNgcConsole.Checked) _deviceInfo.NGC = MuniaDeviceInfo.NGCMode.NGC_MODE_NGC;
				else _deviceInfo.NGC = MuniaDeviceInfo.NGCMode.NGC_MODE_PC;
			}
			else {
				if (rbOutputPC.Checked) {
					_deviceInfo.Output = MuniaDeviceInfo.OutputMode.PC;
					_deviceInfo.Inputs = 0;
					if (ckbNGC.Checked) _deviceInfo.Inputs |= MuniaDeviceInfo.InputSources.NGC;
					if (ckbN64.Checked) _deviceInfo.Inputs |= MuniaDeviceInfo.InputSources.N64;
					if (ckbSNES.Checked) _deviceInfo.Inputs |= MuniaDeviceInfo.InputSources.SNES;
				}
				else {
					if (rbInputNGC.Checked) _deviceInfo.Inputs = MuniaDeviceInfo.InputSources.NGC;
					else if (rbInputN64.Checked) _deviceInfo.Inputs = MuniaDeviceInfo.InputSources.N64;
					else if (rbInputSNES.Checked) _deviceInfo.Inputs = MuniaDeviceInfo.InputSources.SNES;

					if (rbOutputNGC.Checked) {
						_deviceInfo.Output = MuniaDeviceInfo.OutputMode.NGC;
					}
					else if (rbOutputN64.Checked) {
						_deviceInfo.Output = MuniaDeviceInfo.OutputMode.N64;
					}
					else if (rbOutputSNES.Checked) {
						_deviceInfo.Output = MuniaDeviceInfo.OutputMode.SNES;
					}
				}

			}
			var report = _deviceInfo.ToWriteReport();
			try {
				using (var stream = _intf.Open())
					stream.Write(report, 0, report.Length);
				MessageBox.Show("Config updated successfully", "Success");
			}
			catch {
				MessageBox.Show("Report saving failed"); throw;
			}
		}

		private void rbOutput_CheckedChanged(object sender, System.EventArgs e) {
			UpdateUI();
		}
	}
}
