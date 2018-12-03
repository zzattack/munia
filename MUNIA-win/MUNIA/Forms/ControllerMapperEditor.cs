using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MUNIA.Controllers;
using MUNIA.Skinning;
using MUNIA.Util;

namespace MUNIA.Forms {
	public partial class ControllerMapperEditor : UserControl {
		private GenericController _realController;
		private MappedController _mappedController;
		private Skin _skin;
		private ControllerMapping _mapping;

		public ControllerMapperEditor() {
			InitializeComponent();
			this.Load += OnControllerStateUpdate;
			Task.Delay(3000).ContinueWith((t) => HideHint());
		}


		public ControllerMapperEditor(GenericController realController, ControllerType target, Skin skin, ControllerMapping mapping) : this() {
			System.Diagnostics.Debug.Assert(mapping.AppliesTo(realController));
			System.Diagnostics.Debug.Assert(skin.Controllers.Contains(target));

			_realController = realController;
			_skin = skin = Skin.Clone(skin); // do not let main skin be affected
			_mapping = mapping;

			IController iController = realController.RequiresPolling ? new PollingController(realController, 1000 / 16) : (IController)realController;
			iController.StateUpdated += OnControllerStateUpdate;
			gamepadViewer.StartTesting(iController);
			skinPreview.Skin = skin;
			_mappedController = new MappedController(mapping, iController);

			// determine number of buttons/axes on controller and number of buttons/axes in skin
			var state = realController.GetState();
			int numControllerButtons = state.Buttons.Count;
			int numControllerAxes = state.Axes.Count;
			skin.GetNumberOfElements(out int numSkinButtons, out int numSkinAxes);

			List<ControllerMapping.Button> sourceButtons = new List<ControllerMapping.Button>();
			List<ControllerMapping.Axis> sourceAxes = new List<ControllerMapping.Axis>();

			// ensure mapping contains entries for all buttons and axes on controller
			for (int i = 0; i < numControllerButtons; i++) {
				if (_mapping.ButtonMaps.All(b => b.Source != (ControllerMapping.Button)i)) {
					_mapping.ButtonMaps.Add(new ControllerMapping.ButtonMap {
						Source = (ControllerMapping.Button)i,
						Target = ControllerMapping.Button.Unmapped,
					});
				}
				sourceButtons.Add((ControllerMapping.Button)i);
			}
			for (int i = 0; i < numControllerAxes; i++) {
				if (_mapping.AxisMaps.All(b => b.Source != (ControllerMapping.Axis)i)) {
					// default 1:1 map
					_mapping.AxisMaps.Add(new ControllerMapping.AxisMap {
						Source = (ControllerMapping.Axis)i,
						Target = (ControllerMapping.Axis)i,
						IsTrigger = realController.IsAxisTrigger(i)
					});
				}
				sourceAxes.Add((ControllerMapping.Axis)i);
			}
			// remove invalid entries
			_mapping.ButtonMaps.RemoveAll(b => (int)b.Source >= numControllerButtons || (int)b.Target >= numSkinButtons);
			_mapping.AxisMaps.RemoveAll(a => (int)a.Source >= numControllerAxes || (int)a.Target >= numSkinAxes);
			// make sure they appear in logical order
			_mapping.ButtonMaps.Sort((self, other) => self.Source.CompareTo(other.Source));
			_mapping.AxisMaps.Sort((self, other) => self.Source.CompareTo(other.Source));

			dgvcSourceButton.DataSource = sourceButtons;
			cbButtonToAxisSource.DataSource = sourceButtons;
			cbAxisToButtonTarget.DataSource = sourceButtons;

			dgvcSourceAxis.DataSource = sourceAxes;
			cbAxisToButtonSource.DataSource = sourceAxes;
			cbButtonToAxisTarget.DataSource = sourceAxes;
			
			List<ControllerMapping.Button> targetButtons = new List<ControllerMapping.Button> { ControllerMapping.Button.Unmapped };
			for (int i = 0; i < numSkinButtons; i++)
				targetButtons.Add((ControllerMapping.Button)i);

			List<ControllerMapping.Axis> targetAxes = new List<ControllerMapping.Axis> { ControllerMapping.Axis.Unmapped };
			for (int i = 0; i < numSkinAxes; i++) 
				targetAxes.Add((ControllerMapping.Axis)i);

			

			// enable dataGridViews only if there are actual elements in both controller and axis
			if (numSkinButtons > 0 && numControllerButtons > 0) {
				dgvcTargetButton.DataSource = targetButtons;
				bsButtons.DataSource = new BindingList<ControllerMapping.ButtonMap>(_mapping.ButtonMaps);
				dgvButtons.DataSource = bsButtons;
			}
			else {
				tpButtons.Visible = dgvButtons.Visible = lblButtons.Visible = false;
			}

			tpAxesToButtons.Visible = numControllerAxes > 0;
			tpButtonsToAxes.Visible = numControllerButtons > 0;

			bsAxesToButtons.DataSource = new BindingList<ControllerMapping.AxisToButtonMap>(_mapping.AxisToButtonMaps);
			bsButtonsToAxes.DataSource = new BindingList<ControllerMapping.ButtonToAxisMap>(_mapping.ButtonToAxisMaps);

			if (numSkinAxes > 0 && numControllerAxes > 0) {
				dgvcTargetAxis.DataSource = targetAxes;
				bsAxes.DataSource = new BindingList<ControllerMapping.AxisMap>(_mapping.AxisMaps);
				dgvAxes.DataSource = bsAxes;

			}
			else {
				tpAxes.Visible = false;
				tpAxesToButtons.Visible = false;
			}
		}

		private bool _busyRendering = false;
		private void OnControllerStateUpdate(object sender, EventArgs e) {
			if (IsHandleCreated && !IsDisposed && !_busyRendering) {
				try {
					_busyRendering = true;
					BeginInvoke((Action)delegate {
						try {
							// update mapping
							if (_seqMapPos >= 0)
								SequentialMappingUpdate(_realController.GetState());
							else // simply show mapped controller state
								skinPreview.RenderSkin(_mappedController.GetState());
						}
						finally {
							_busyRendering = false;
						}
					});
				}
				catch (InvalidOperationException) { }
			}
		}

		private void skinPreview_Resize(object sender, EventArgs e) {
			OnControllerStateUpdate(null, null);
		}

		#region Sequential button mapper

		private int _seqMapPos = -1;
		private int _seqMapPrevState;
		private List<ControllerMapping.Button> _seqMapTargets = new List<ControllerMapping.Button>();
		private List<ControllerMapping.ButtonMap> _mapBackup = new List<ControllerMapping.ButtonMap>();
		private bool _seqMapFlashToggle;

		private void btnMapSequentially_Click(object sender, EventArgs e) {
			// see if map in progress
			if (_seqMapPos > 0) {
				var dr = MessageBox.Show("Mapping is already in progress, abort?", "Confirm", MessageBoxButtons.OKCancel);
				if (dr == DialogResult.OK)
					CancelSequentialMapping();
			}
			else if (_seqMapPos == 0) {
				// abort, but don't bother confirming as there's nothing mapped yet
				CancelSequentialMapping();
			}
			else {
				if (_mapping.ButtonMaps.Any(m => m.Target != ControllerMapping.Button.Unmapped)) {
					// confirm
					var dr = MessageBox.Show("This will overwrite your current mapping. Continue?", "Confirm", MessageBoxButtons.OKCancel);
					if (dr == DialogResult.Cancel) return;
				}
				StartSequentialMapping();
			}
		}

		private void StartSequentialMapping() {
			// backup current mapping
			_mapBackup.Clear();
			_mapBackup.AddRange(_mapping.ButtonMaps.Select(m => m.Clone()));

			// set all buttons to unmapped
			_mapping.ButtonMaps.ForEach(b => b.Target = ControllerMapping.Button.Unmapped);

			// update gridviewe
			dgvButtons.ResetBindings();
			dgvButtons.Refresh();

			// figure out how much to map
			_skin.GetNumberOfElements(out int buttons, out int axes);
			_seqMapTargets = Enumerable.Range(0, buttons).Cast<ControllerMapping.Button>().ToList();

			// ui preparation
			btnSeqMapSkip.Visible = true;
			btnMapSequentially.Text = "Abort sequential mapping";

			// off we go
			_seqMapPrevState = 0;
			_seqMapPos = -1;
			SequentialMapNext(null);
		}
		private void btnSeqMapSkip_Click(object sender, EventArgs e) {
			SequentialMapNext(null);
		}

		private void SequentialMapNext(ControllerMapping.Button? button) {
			string message = "";

			if (_seqMapPos >= 0) {
				// save in mapping
				if (button.HasValue && button.Value != ControllerMapping.Button.Unmapped) {
					_mapping.ButtonMaps[(int)button.Value].Target = _seqMapTargets[_seqMapPos];
					message = $"Button {_seqMapTargets[_seqMapPos]} mapped to {button.Value}.\r\n";
				}
				dgvButtons.ResetBindings();
				dgvButtons.Refresh();
			}

			_seqMapPos++;

			if (_seqMapPos >= _seqMapTargets.Count) {
				SequentialMapFinished();
			}
			else {
				// start animating with a highlight
				_seqMapFlashToggle = true;
				tmrSequentialMapFlasher.Stop();
				tmrSequentialMapFlasher_Tick(null, null);
				tmrSequentialMapFlasher.Start();

				message += "Press the flashing button on your controller";
				lblSeqMapHint.Text = message;
			}
		}

		private void CancelSequentialMapping() {
			// restore mapping
			_mapping.ButtonMaps.Clear();
			_mapping.ButtonMaps.AddRange(_mapBackup);
			dgvButtons.ResetBindings();
			dgvButtons.Refresh();
			SequentialMappingEnd();
			// restore original message
			btnMapSequentially.Text = "Map buttons sequentially";
			lblSeqMapHint.Text = "The button above will start a sequential button mapping sequence,\r\n"
									+ "allowing you to quickly defined a mapping for all buttons in the skin.";
		}

		private void SequentialMapFinished() {
			SequentialMappingEnd();
			btnMapSequentially.Text = "Redo mapping";
			lblSeqMapHint.Text = "Mapping complete!";
			ConfigManager.Save();
		}

		private void SequentialMappingEnd() {
			_seqMapPos = -1;
			btnSeqMapSkip.Visible = false;
			tmrSequentialMapFlasher.Enabled = false;
		}

		private void SequentialMappingUpdate(ControllerState state) {
			if (_seqMapPos < 0) return;
			var pressed = state.Buttons.Select((btn, idx) => new { btn, idx }).Where(p => p.btn);
			if (pressed.Count() != 1) return; // zero or more than 1, indecisive

			var buttonsHash = state.Buttons.GetSequenceHashCode();
			if (Equals(_seqMapPrevState, buttonsHash)) return; // same as last pressed, probably button bounce
			_seqMapPrevState = buttonsHash;

			SequentialMapNext((ControllerMapping.Button)pressed.First().idx);
		}

		#endregion

		private void btnFinish_Click(object sender, EventArgs e) {
			if (_seqMapPos >= 0) {
				var dr = MessageBox.Show("Sequential mapping is still in progress. Do you wish to abort and revert to previous mapping?",
					"Confirm", MessageBoxButtons.OKCancel);
				if (dr == DialogResult.Cancel)
					return;
			}
			OnFinish(EventArgs.Empty);
		}

		public event EventHandler FinishEditing;

		protected virtual void OnFinish(EventArgs e) {
			FinishEditing?.Invoke(this, e);
		}

		private void tmrSequentialMapFlasher_Tick(object sender, EventArgs e) {
			if (_seqMapPos >= 0) {
				var state = new ControllerState();
				_skin.GetNumberOfElements(out int buttons, out int axes);
				state.Buttons.EnsureSize(buttons);
				state.Axes.EnsureSize(axes);

				System.Diagnostics.Debug.WriteLine($"toggle btn {_seqMapPos} = {_seqMapFlashToggle}");

				_seqMapFlashToggle = !_seqMapFlashToggle;
				state.Buttons[(int)_seqMapTargets[_seqMapPos]] = _seqMapFlashToggle;
				skinPreview.RenderSkin(state);
			}
		}

		private void dgv_CurrentCellDirtyStateChanged(object sender, EventArgs e) {
			(sender as DataGridView)?.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}
		
		private void dgvButtons_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
			if (e.Control is ComboBox cb) {
				cb.Enter -= ctrl_Enter;
				cb.Enter +=ctrl_Enter;
			}
		}

		void ctrl_Enter(object sender, EventArgs e) {
			if (sender is ComboBox cb) cb.DroppedDown = true;
		}

		private void HideHint() {
			if (!IsDisposed && IsHandleCreated) {
				try { BeginInvoke((Action)delegate { lblHint.Visible = false; }); }
				catch { }
			}
		}

		private async void tabs_SelectedIndexChanged(object sender, EventArgs e) {
			if (tabs.SelectedTab == tpButtons) {
				lblHint.Visible = true;
				await Task.Delay(3000);
				lblHint.Visible = false;
			}
			else lblHint.Visible = false;
		}

		#region Button to axis
		private void btnAddButtonToAxis_Click(object sender, EventArgs e) {
			bsButtonsToAxes.AddNew();
		}

		private void btnRemoveButtonToAxis_Click(object sender, EventArgs e) {
			if (bsButtonsToAxes.Current != null) bsButtonsToAxes.RemoveCurrent();
		}

		private void lbButtonsToAxes_SelectedIndexChanged(object sender, EventArgs e) {
			if (lbButtonsToAxes.SelectedItem is ControllerMapping.ButtonToAxisMap map) {
				gbButtonToAxis.Enabled = true;
				LoadButtonToAxisMap(map);
			}
			else {
				gbButtonToAxis.Enabled = false;
			}
		}

		private void LoadButtonToAxisMap(ControllerMapping.ButtonToAxisMap map) {
			cbButtonToAxisSource.SelectedItem = map.Source;
			cbButtonToAxisTarget.SelectedItem = map.Target;
			tkbAxisOffset.Value = (int)(map.AxisValue * 100);
		}

		private void cbButtonToAxisSource_SelectedIndexChanged(object sender, EventArgs e) {
			if (lbButtonsToAxes.SelectedItem is ControllerMapping.ButtonToAxisMap map &&
				cbButtonToAxisSource.SelectedItem is ControllerMapping.Button btn)
				map.Source = btn;
		}

		private void cbButtonToAxisTarget_SelectedIndexChanged(object sender, EventArgs e) {
			if (lbButtonsToAxes.SelectedItem is ControllerMapping.ButtonToAxisMap map &&
				cbButtonToAxisTarget.SelectedItem is ControllerMapping.Axis axis)
				map.Target = axis;
		}

		private void tkbAxisOffset_Scroll(object sender, EventArgs e) {
			lblAxisOffset.Text = "Value: " + tkbAxisOffset.Value;
			if (lbButtonsToAxes.SelectedItem is ControllerMapping.ButtonToAxisMap map)
				map.AxisValue = tkbAxisOffset.Value / 100.0;
		}



		#endregion

		#region Axis to Button
		private void btnAddAxisToButtonMapping_Click(object sender, EventArgs e) {
			bsAxesToButtons.AddNew();
		}
		private void btnRemoveAxisToButtonMapping_Click(object sender, EventArgs e) {
			if (bsAxesToButtons.Current != null) bsAxesToButtons.RemoveCurrent();
		}

		private void lbAxesToButtons_SelectedIndexChanged(object sender, EventArgs e) {
			if (lbAxesToButtons.SelectedItem is ControllerMapping.AxisToButtonMap map)
				LoadAxisToButtonMap(map);
		}

		private void LoadAxisToButtonMap(ControllerMapping.AxisToButtonMap map) {
			cbAxisToButtonSource.SelectedItem = map.Source;
			cbAxisToButtonTarget.SelectedItem = map.Target;
			tkbButtonThreshold.Value = (int)(map.Threshold * 100);
		}
		
		private void cbAxisToButtonSource_SelectedIndexChanged(object sender, EventArgs e) {
			if (lbAxesToButtons.SelectedItem is ControllerMapping.AxisToButtonMap map &&
				cbAxisToButtonSource.SelectedItem is ControllerMapping.Axis axis)
				map.Source = axis;
		}

		private void cbAxisToButtonTarget_SelectedIndexChanged(object sender, EventArgs e) {
			if (lbAxesToButtons.SelectedItem is ControllerMapping.AxisToButtonMap map &&
				cbAxisToButtonTarget.SelectedItem is ControllerMapping.Button btn)
				map.Target = btn;
		}

		private void tkbButtonThreshold_Scroll(object sender, EventArgs e) {
			lblThresholdValue.Text = "Value: " + tkbButtonThreshold.Value;
			if (lbAxesToButtons.SelectedItem is ControllerMapping.AxisToButtonMap map) {
				map.Threshold = tkbButtonThreshold.Value / 100.0;
				map.Mode = tkbButtonThreshold.Value > 0
					? ControllerMapping.AxisToButtonMapMode.WhenAboveThreshold
					: ControllerMapping.AxisToButtonMapMode.WhenBelowThreshold;
			}
		}


		#endregion

	}
}
