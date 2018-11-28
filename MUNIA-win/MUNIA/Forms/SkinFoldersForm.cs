using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace MUNIA.Forms {
	public partial class SkinFoldersForm : Form {
		private readonly BindingList<SkinDirectoryEntry> _folders;
		private SkinDirectoryEntry _currentEntry;
		private readonly CommonOpenFileDialog fbd = new CommonOpenFileDialog();

		private SkinFoldersForm() {
			InitializeComponent();

			fbd.IsFolderPicker = true;
			fbd.Multiselect = true;
		}
		public SkinFoldersForm(BindingList<SkinDirectoryEntry> folders) : this() {
			_folders = folders;
			list.DisplayMember = nameof(SkinDirectoryEntry.Path);
			list.DataSource = folders;
			UpdateUI();
		}

		private void btnAdd_Click(object sender, EventArgs e) {
			fbd.Multiselect = true;
			if (fbd.ShowDialog() == CommonFileDialogResult.Ok) {
				foreach (string path in fbd.FileNames)
					_folders.Add(new SkinDirectoryEntry { Path = path, Types = SkinType.Svg });
			}
		}

		private void btnRemove_Click(object sender, EventArgs e) {
			if (list.SelectedItem is SkinDirectoryEntry s)
				_folders.Remove(s);
		}

		private void list_SelectedIndexChanged(object sender, EventArgs e) {
			_currentEntry = list.SelectedItem as SkinDirectoryEntry;
			UpdateUI();
		}

		private void UpdateUI() {
			if (_currentEntry != null) {
				btnMoveUp.Enabled = list.Items.Count > 1 && list.SelectedIndex > 0;
				btnMoveDown.Enabled = list.Items.Count > 1 && list.SelectedIndex < list.Items.Count - 1;
				ckbSvgSkins.Enabled = ckbNintendoSpy.Enabled = ckbPadPyght.Enabled = true;

				ckbSvgSkins.Checked = (_currentEntry.Types & SkinType.Svg) != 0;
				ckbNintendoSpy.Checked = (_currentEntry.Types & SkinType.NintendoSpy) != 0;
				ckbPadPyght.Checked = (_currentEntry.Types & SkinType.PadPyght) != 0;
			}
			else {
				btnMoveUp.Enabled = btnMoveDown.Enabled = ckbSvgSkins.Enabled = ckbNintendoSpy.Enabled = ckbPadPyght.Enabled = false;
			}
		}

		private void ckbSvgSkins_CheckedChanged(object sender, EventArgs e) {
			if (_currentEntry is SkinDirectoryEntry s) {
				if (ckbSvgSkins.Checked) s.Types |= SkinType.Svg;
				else s.Types &= ~SkinType.Svg;
			}
		}

		private void ckbNintendoSpy_CheckedChanged(object sender, EventArgs e) {
			if (_currentEntry is SkinDirectoryEntry s) {
				if (ckbNintendoSpy.Checked) s.Types |= SkinType.NintendoSpy;
				else s.Types &= ~SkinType.NintendoSpy;
			}
		}

		private void ckbPadPyght_CheckedChanged(object sender, EventArgs e) {
			if (_currentEntry is SkinDirectoryEntry s) {
				if (ckbPadPyght.Checked) s.Types |= SkinType.PadPyght;
				else s.Types &= ~SkinType.PadPyght;
			}
		}

		private void btnMoveUp_Click(object sender, EventArgs e) {
			var upper = _folders[list.SelectedIndex - 1];
			_folders[list.SelectedIndex - 1] = _currentEntry;
			_folders[list.SelectedIndex] = upper;
			list.SelectedIndex--;
		}

		private void btnMoveDown_Click(object sender, EventArgs e) {
			var lower = _folders[list.SelectedIndex + 1];
			_folders[list.SelectedIndex + 1] = _currentEntry;
			_folders[list.SelectedIndex] = lower;
			list.SelectedIndex++;
		}

		private void list_MouseDoubleClick(object sender, MouseEventArgs e) {
			int index = list.IndexFromPoint(e.Location);
			if (index == ListBox.NoMatches) return;

			fbd.Multiselect = false;
			fbd.RestoreDirectory = false;
			if (list.Items[index] is SkinDirectoryEntry sde) {
				fbd.InitialDirectory = sde.Path;
				
				if (fbd.ShowDialog() == CommonFileDialogResult.Ok) {
					sde.Path = fbd.FileName;
					list.Refresh();
				}
			}
		}

	}

	[Flags]
	public enum SkinType {
		None = 0,
		Svg = 1,
		PadPyght = 2,
		NintendoSpy = 4
	}
	public class SkinDirectoryEntry {
		public string Path { get; set; }
		public SkinType Types { get; set; }

	}
}
