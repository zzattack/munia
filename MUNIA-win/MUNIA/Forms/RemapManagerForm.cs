using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MUNIA.Skins;

namespace MUNIA.Forms {
	public partial class RemapManagerForm : Form {
		private readonly List<ColorRemap> _remaps;
		private RemapManagerForm() {
			InitializeComponent();
		}
		public RemapManagerForm(List<ColorRemap> remaps) : this() {
			_remaps = remaps;

		}
		private void btnFinish_Click(object sender, EventArgs e) {
			Close();
		}

	}
}
