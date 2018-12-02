using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MUNIA.Controllers;
using MUNIA.Skinning;

namespace MUNIA.Forms {
	public partial class ControllerMapperManager : UserControl {
		private bool _ignoreSelectedIndexChanged;

		public ControllerMapperManager() {
			InitializeComponent();

			_ignoreSelectedIndexChanged = true;
			lbControllerSource.DataSource = GenericController.ListDevices().ToList();
			lbControllerSource.SelectedIndex = -1;
			UpdateFilter();
			lbMappings.SelectedIndex = -1;
			_ignoreSelectedIndexChanged = false;

			UpdateUI();
		}

		private void UpdateUI() {
			GenericController controller = null;
			ControllerType target = ControllerType.Unknown;
			Skin skin = null;
			ControllerMapping mapping = null;

			if (lbControllerSource.SelectedItem is GenericController g) {
				controller = g;
				if (lbTargetControllerType.SelectedItem is ControllerType tgt) {
					target = tgt;
					if (lbSkins.SelectedItem is Skin s) {
						skin = s;
					}
				}
			}
			// mappings can be deleted without selecting in step 1-3 first
			if (lbMappings.SelectedItem is ControllerMapping m) {
				mapping = m;
			}

			lblStep1.Font = new Font(lblStep1.Font, controller == null ? FontStyle.Bold : FontStyle.Regular);
			lblPickController.Visible = controller == null;

			lblStep2.Font = new Font(lblStep2.Font, controller != null && target == ControllerType.Unknown ? FontStyle.Bold : FontStyle.Regular);
			lbTargetControllerType.Enabled = controller != null;
			lblPickTarget.Visible = controller != null && target == ControllerType.Unknown;

			lblStep3.Font = new Font(lblStep3.Font, target != ControllerType.Unknown && skin == null ? FontStyle.Bold : FontStyle.Regular);
			lbSkins.Enabled = target != ControllerType.Unknown;
			lblPickSkin.Visible = target != ControllerType.Unknown && skin == null;

			lblStep4.Font = new Font(lblStep4.Font, skin != null && mapping == null ? FontStyle.Bold : FontStyle.Regular);
			lblPickMapping.Visible = btnAddMapping.Enabled = skin != null;

			bool applies = mapping?.AppliesTo(controller) ?? false;
			btnClone.Enabled = mapping != null;
			btnRemoveMapping.Enabled = mapping != null && !mapping.IsBuiltIn;
			btnContinue.Enabled = applies && !mapping.IsBuiltIn;

			if (applies && mapping.IsBuiltIn)
				lblMappingBuiltIn.Visible = true;
			else
				lblMappingIncompatible.Visible = mapping != null && !applies;
		}

		private void lbControllerSource_SelectedIndexChanged(object sender, EventArgs e) {
			if (_ignoreSelectedIndexChanged) return;
			if (lbControllerSource.SelectedItem is GenericController) {
				var controllerTypes = Enum.GetValues(typeof(ControllerType)).Cast<ControllerType>().ToList();
				controllerTypes.Remove(ControllerType.Unknown);
				controllerTypes.Remove(ControllerType.None);
				_ignoreSelectedIndexChanged = true;
				lbTargetControllerType.DataSource = controllerTypes;
				lbTargetControllerType.SelectedIndex = -1;
				_ignoreSelectedIndexChanged = false;
			}
			UpdateUI();
		}

		private void lbTargetControllerType_SelectedIndexChanged(object sender, EventArgs e) {
			if (_ignoreSelectedIndexChanged) return;
			if (lbTargetControllerType.SelectedItem is ControllerType t) {
				_ignoreSelectedIndexChanged = true;
				lbSkins.DataSource = ConfigManager.Skins.Where(s => s.Controllers.Contains(t)).ToList();
				lbSkins.SelectedIndex = -1;
				_ignoreSelectedIndexChanged = false;
			}
			UpdateUI();
		}

		private void lbSkins_SelectedIndexChanged(object sender, EventArgs e) {
			if (_ignoreSelectedIndexChanged) return;
			if (lbSkins.SelectedItem is Skin s) {
				UpdateFilter();
			}
			UpdateUI();
		}

		private void UpdateFilter() {
			_ignoreSelectedIndexChanged = true;
			if (lbTargetControllerType.SelectedItem is ControllerType target) {
				lbMappings.DataSource = ConfigManager.ControllerMappings.Where(mapping => mapping.MappedType == target).ToList();
			}
			else {
				lbMappings.DataSource = ConfigManager.ControllerMappings;
			}
			_ignoreSelectedIndexChanged = false;
		}

		private void lbMappings_SelectedIndexChanged(object sender, EventArgs e) {
			UpdateUI();
		}

		private void lbControllerSource_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right && sender is ListBox lb)
				lb.SelectedIndex = -1;
		}

		private void btnAddMapping_Click(object sender, EventArgs e) {
			if (lbTargetControllerType.SelectedItem is ControllerType target) {
				if (lbControllerSource.SelectedItem is GenericController controller) {
					ConfigManager.ControllerMappings.Add(new ControllerMapping(controller, target));
					UpdateFilter();
				}
			}
		}

		private void btnRemoveMapping_Click(object sender, EventArgs e) {
			if (lbMappings.SelectedItem is ControllerMapping mapping) {
				ConfigManager.ControllerMappings.Remove(mapping);
				UpdateFilter();
			}
		}

		private void btnClone_Click(object sender, EventArgs e) {
			if (lbMappings.SelectedItem is ControllerMapping mapping) {
				ConfigManager.ControllerMappings.Add(mapping.Clone());
				UpdateFilter();
			}
		}

		private void btnContinue_Click(object sender, EventArgs e) {
			if (lbControllerSource.SelectedItem is GenericController g) {
				if (lbTargetControllerType.SelectedItem is ControllerType tgt) {
					if (lbSkins.SelectedItem is Skin s) {
						if (lbMappings.SelectedItem is ControllerMapping m) {
							OnMappingSelected(new MappingSelectionArgs(g, tgt, s, m));
						}
					}
				}
			}
		}

		public event EventHandler<MappingSelectionArgs> MappingSelected;

		protected virtual void OnMappingSelected(MappingSelectionArgs e) {
			MappingSelected?.Invoke(this, e);
		}

		private void lbMappings_MouseDoubleClick(object sender, MouseEventArgs e) {
			int index = lbMappings.IndexFromPoint(e.Location);
			// button is only enabled if selected item actually applies to given mapping
			if (index != ListBox.NoMatches && btnContinue.Enabled)
				btnContinue_Click(null, null);
		}
	}

	public class MappingSelectionArgs : EventArgs {
		public GenericController Controller;
		public ControllerType Target;
		public Skin PreviewSkin;
		public ControllerMapping Mapping;

		public MappingSelectionArgs(GenericController controller, ControllerType target, Skin previewSkin, ControllerMapping mapping) {
			Controller = controller;
			Target = target;
			PreviewSkin = previewSkin;
			Mapping = mapping;
		}
	}
}
