using System.Windows.Forms;
using HidSharp;

namespace MUNIA.Forms {
	public partial class MuniaSettingsDialog : Form {
		private readonly HidStream _hidStream;
		private readonly MuniaDeviceInfo _deviceInfo;

		public MuniaSettingsDialog() {
			InitializeComponent();
		}

		public MuniaSettingsDialog(HidStream hidStream, MuniaDeviceInfo deviceInfo) : this() {
			this._hidStream = hidStream;
			_deviceInfo = deviceInfo;
			
			_blockRecurse = true;
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
				if (_deviceInfo.Output == MuniaDeviceInfo.OutputMode.PC) {
					rbOutputPC.Checked = true;
					ckbNGC.Checked = _deviceInfo.Inputs.HasFlag(MuniaDeviceInfo.InputSources.NGC);
					ckbN64.Checked = _deviceInfo.Inputs.HasFlag(MuniaDeviceInfo.InputSources.N64);
					ckbSNES.Checked = _deviceInfo.Inputs.HasFlag(MuniaDeviceInfo.InputSources.SNES);
				}
				else {
					rbOutputNGC.Checked = _deviceInfo.Output == MuniaDeviceInfo.OutputMode.NGC;
					rbOutputN64.Checked = _deviceInfo.Output == MuniaDeviceInfo.OutputMode.N64;
					rbOutputSNES.Checked = _deviceInfo.Output == MuniaDeviceInfo.OutputMode.SNES;
					rbInputNGC.Checked = _deviceInfo.Inputs.HasFlag(MuniaDeviceInfo.InputSources.NGC);
					rbInputN64.Checked = _deviceInfo.Inputs.HasFlag(MuniaDeviceInfo.InputSources.N64);
					rbInputSNES.Checked = _deviceInfo.Inputs.HasFlag(MuniaDeviceInfo.InputSources.SNES);
				}
			}
			_blockRecurse = false;
			UpdateUI();

			tbFirmware.Text = $"{deviceInfo.VersionMajor}.{deviceInfo.VersionMinor}";
			tbHardware.Text = "rev" + deviceInfo.HardwareRevision;
			tbMCUId.Text = "0x" + deviceInfo.DeviceID.ToString("x4");
			tbMCURevision.Text = deviceInfo.HardwareRevision.ToString();
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
				pnlInputsPC.Visible = rbOutputPC.Checked;
				pnlInputs.Visible = !rbOutputPC.Checked;

				if (!rbOutputPC.Checked) {
					if (rbOutputNGC.Checked) {
						rbInputNGC.Enabled = true;
						rbInputN64.Enabled = false;
						rbInputSNES.Enabled = true;
						if (rbInputN64.Checked) {
							rbInputNGC.Checked = true;
							rbInputN64.Checked = false;
						}
					}
					else if (rbOutputN64.Checked) {
						rbInputNGC.Enabled = true;
						rbInputN64.Enabled = true;
						rbInputSNES.Enabled = false;
						if (rbInputSNES.Checked) {
							rbInputN64.Checked = true;
							rbInputSNES.Checked = false;
						}
					}
					else if (rbOutputSNES.Checked) {
						rbInputNGC.Enabled = false;
						rbInputN64.Enabled = false;
						rbInputSNES.Enabled = true;
						rbInputSNES.Checked = true;
					}
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
				_hidStream.Write(report, 0, report.Length);
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
