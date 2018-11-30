using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MUNIA.Skinning;

namespace MUNIA.Forms {
	public partial class SkinRemapManagerForm : Form {
		private readonly SvgSkin _skin;
		private readonly BindingList<ColorRemap> _remaps;
		private readonly ColorRemap _defaultRemap;
		public ColorRemap SelectedRemap { get; private set; }
		public event EventHandler SelectedRemapChanged;

		private SkinRemapManagerForm() {
			InitializeComponent();
		}
		public SkinRemapManagerForm(SvgSkin skin, ColorRemap selectedRemap, BindingList<ColorRemap> remaps) : this() {
			_skin = skin;
			_remaps = remaps;
			if (_remaps.Count(r => r.IsSkinDefault) == 0) {
				_defaultRemap = ColorRemap.CreateFromSkin(skin);
				_remaps.Add(_defaultRemap);
			}
			else {
				_defaultRemap = _remaps.First(r => r.IsSkinDefault);
			}

			// initialize listbox
			list.DataSource = _remaps;
			list.SelectedItem = SelectedRemap = selectedRemap;

			lblSkinType.Text = $@"Color scheme for {string.Join(" ", skin.Controllers)}";
		}
		private void btnFinish_Click(object sender, EventArgs e) {
			Close();
		}

		private void btnNew_Click(object sender, EventArgs e) {
			var remap = _defaultRemap.Clone();
			_remaps.Add(remap);
			(new SkinRemapperForm(_skin.Path, remap)).ShowDialog(this);
		}

		private void btnEdit_Click(object sender, EventArgs e) {
			(new SkinRemapperForm(_skin.Path, list.SelectedItem as ColorRemap)).ShowDialog(this);
		}

		private void btnClone_Click(object sender, EventArgs e) {
			if (list.SelectedItem is ColorRemap curr)
				_remaps.Add(curr.Clone());
		}

		private void btnDelete_Click(object sender, EventArgs e) {
			_remaps.Remove((ColorRemap)list.SelectedItem);
		}

		private void list_SelectedIndexChanged(object sender, EventArgs e) {
			btnNew.Enabled = true;

			var curr = list.SelectedItem as ColorRemap;
			if (curr != null) {
				SelectedRemap = curr;
				SelectedRemapChanged?.Invoke(this, EventArgs.Empty);
			}

			btnEdit.Enabled = btnDelete.Enabled = curr != null && !curr.IsSkinDefault;
			btnClone.Enabled = curr != null;
			btnExport.Enabled = !curr?.IsSkinDefault ?? false;
		}

		private void btnExport_Click(object sender, EventArgs e) {
			if (SelectedRemap == null) return;

			var newSkin = new SvgSkin();
			newSkin.Load(_skin.Path);
			newSkin.ApplyRemap(SelectedRemap);

			sfd.FileName = $@"{Path.GetFileNameWithoutExtension(_skin.Path)}_{SelectedRemap.Name}.svg";
			if (sfd.ShowDialog() == DialogResult.OK) {
				newSkin.SvgDocument.Write(sfd.FileName);
			}
		}

		private void list_MouseDoubleClick(object sender, MouseEventArgs e) {
			int index = list.IndexFromPoint(e.Location);
			if (index != ListBox.NoMatches)
				btnEdit_Click(null, null);
		}
	}

}
