using System;
using System.Drawing;
using System.Windows.Forms;

namespace MUNIA {
	public partial class WindowSizePicker : Form {
		private WindowSizePicker() {
			InitializeComponent();
		}
		public WindowSizePicker(Size preload) : this() {
			nudWidth.Value = preload.Width;
			nudHeight.Value = preload.Height;
		}

		public Size ChosenSize;

		private void BtnAccept_Click(object sender, EventArgs e) {
			ChosenSize.Width = (int)nudWidth.Value;
			ChosenSize.Height = (int)nudHeight.Value;
		}

		private void nudWidth_Enter(object sender, EventArgs e) {
			nudWidth.Select(0, nudWidth.Text.Length);
		}

		private void nudHeight_Enter(object sender, EventArgs e) {
			nudHeight.Select(0, nudHeight.Text.Length);
		}
	}
}
