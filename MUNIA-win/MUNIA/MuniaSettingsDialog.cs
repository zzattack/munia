using System.Windows.Forms;
using HidSharp;

namespace MUNIA {
	public partial class MuniaSettingsDialog : Form {
		private HidStream _hidStream;
		private readonly MuniaSettings _settings;

		public MuniaSettingsDialog() {
			InitializeComponent();
		}

		public MuniaSettingsDialog(HidStream hidStream, MuniaSettings settings) : this() {
			this._hidStream = hidStream;
			_settings = settings;

			if (settings.SNES == MuniaSettings.SnesMode.SNES_MODE_NGC) rbSnesNgc.Checked = true;
			else if (settings.SNES == MuniaSettings.SnesMode.SNES_MODE_PC) rbSnesPC.Checked = true;
			else rbSnesConsole.Checked = true;

			if (settings.N64 == MuniaSettings.N64Mode.N64_MODE_N64) rbN64Console.Checked = true;
			else rbN64PC.Checked = true;

			if (settings.NGC == MuniaSettings.NGCMode.NGC_MODE_NGC) rbNgcConsole.Checked = true;
			else rbNgcPC.Checked = true;

			tbFirmware.Text = $"{settings.VersionMajor}.{settings.VersionMinor}";
			tbHardware.Text = "rev" + settings.HardwareRevision;
		}

		private void btnAccept_Click(object sender, System.EventArgs e) {
			if (rbSnesConsole.Checked) _settings.SNES = MuniaSettings.SnesMode.SNES_MODE_SNES;
			else if (rbSnesNgc.Checked) _settings.SNES = MuniaSettings.SnesMode.SNES_MODE_NGC;
			else _settings.SNES = MuniaSettings.SnesMode.SNES_MODE_PC;

			if (rbN64Console.Checked) _settings.N64 = MuniaSettings.N64Mode.N64_MODE_N64;
			else _settings.N64 = MuniaSettings.N64Mode.N64_MODE_PC;

			if (rbNgcConsole.Checked) _settings.NGC = MuniaSettings.NGCMode.NGC_MODE_NGC;
			else _settings.NGC = MuniaSettings.NGCMode.NGC_MODE_PC;

			var report = _settings.ToWriteReport();
			try {
				_hidStream.Write(report, 0, report.Length);
				MessageBox.Show("Config updated successfully", "Success");
			}
			catch {
				MessageBox.Show("Report saving failed"); throw;
			}
		}
	}
}
