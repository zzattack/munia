using System;
using System.Windows.Forms;

namespace MUNIA.Forms {
	public partial class DelayValuePicker : Form {
		public TimeSpan ChosenDelay;
		private TimeSpan _preload;

		private DelayValuePicker() { InitializeComponent(); }

		public DelayValuePicker(TimeSpan preload) : this() {
			_preload = preload;
			nudDelay.Value = (int)preload.TotalMilliseconds;
		}

		private void btnReset_Click(object sender, EventArgs e) { nudDelay.Value = (int)_preload.TotalMilliseconds; }

		private void nudDelay_Enter(object sender, EventArgs e) {
			nudDelay.Select(0, nudDelay.Text.Length);
		}

		private void BtnAccept_Click(object sender, EventArgs e) {
			ChosenDelay = TimeSpan.FromMilliseconds((int)nudDelay.Value);
		}
	}

}