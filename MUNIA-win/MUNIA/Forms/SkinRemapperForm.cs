using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MUNIA.Skinning;
using MUNIA.Util;
using Svg;

namespace MUNIA.Forms {
	public partial class SkinRemapperForm : Form {
		private readonly SvgSkin _skin;
		public ColorRemap Remap { get; private set; }

		private Color _bkpStroke;
		private Color _bkpFill;
		private Color _selStroke;
		private Color _selFill;
		private bool _isHighlight;
		private bool _selectingGroup = false;

		private GroupedSvgElems _selectedGroup;
		private readonly List<GroupedSvgElems> _highlights = new List<GroupedSvgElems>();
		private readonly List<GroupedSvgElems> _nonHighlights = new List<GroupedSvgElems>();
		private readonly List<GroupedSvgElems> _base = new List<GroupedSvgElems>();
		private readonly CheckBox cbFlash;

		private SkinRemapperForm() {
			InitializeComponent();

			colorPicker.PrimaryColorPicked += (sender, args) => {
				_selFill = pnlFill.BackColor = colorPicker.PrimaryColor;

				if (!_selectingGroup) {
					HighlightGroup(false);
					if (cbFlash.Checked) { timer.Stop(); timer.Start(); }
				}
			};

			colorPicker.SecondaryColorPicked += (sender, args) => {
				_selStroke = pnlStroke.BackColor = colorPicker.SecondaryColor;

				// instantly unhighlight and resync timer
				if (!_selectingGroup) {
					HighlightGroup(false);
					if (cbFlash.Checked) { timer.Stop(); timer.Start(); }
				}
			};

			cbFlash = new CheckBox();
			cbFlash.Text = "Flash selected group";
			cbFlash.Checked = true;
			cbFlash.CheckStateChanged += (s, ex) => {
				timer.Enabled = cbFlash.Checked;
				if (!cbFlash.Checked) HighlightGroup(false);
			};
			ToolStripControlHost host = new ToolStripControlHost(cbFlash);
			toolStrip1.Items.Insert(0, host);
		}

		public SkinRemapperForm(string path, ColorRemap remap) : this() {
			_skin = new SvgSkin();
			_skin.Load(path);
			Remap = remap;
			remap.ApplyToSkin(this._skin);

			var highlightElems = _skin.Buttons.Where(b => b.Pressed != null).Select(b => b.Pressed)
				.Union(_skin.Sticks.Where(s => s.Pressed != null).Select(s => s.Pressed));

			var nonHighlightElems = _skin.Buttons.Where(b => b.Element != null).Select(b => b.Element)
				.Union(_skin.Sticks.Where(s => s.Element != null).Select(s => s.Element))
				.Union(_skin.Triggers.Where(t => t.Element != null).Select(t => t.Element));

			// split remap groups into highlight/non-highlight/base categories
			foreach (var group in remap.Groups) {
				var grpHl = new GroupedSvgElems(group.Fill, group.Stroke);
				var grpNonHl = new GroupedSvgElems(group.Fill, group.Stroke);
				var grpBase = new GroupedSvgElems(group.Fill, group.Stroke);

				foreach (var elem in group) {
					bool hl = GroupContains(elem, highlightElems);
					bool nonHl = GroupContains(elem, nonHighlightElems);
					if (hl) grpHl.Add(elem);
					if (nonHl) grpNonHl.Add(elem);
					if (!hl && !nonHl) grpBase.Add(elem);
				}

				if (grpNonHl.Count == 0 && grpBase.Count == 0) {
					// this is entirely a HL group
					_highlights.Add(group);
				}
				else if (grpHl.Count == 0 && grpBase.Count == 0) {
					// this is entirely a non-HL group
					_nonHighlights.Add(group);
				}
				else if (grpHl.Count == 0 && grpNonHl.Count == 0) {
					_base.Add(group);
				}
				else {
					// split up
					if (grpHl.Any()) _highlights.Add(grpHl);
					if (grpNonHl.Any()) _nonHighlights.Add(grpNonHl);
					if (grpBase.Any()) _base.Add(grpBase);
				}
			}

			tbSkinName.DataBindings.Add("Text", Remap, "Name");
			lbGroups.DisplayMember = nameof(GroupedSvgElems.Name);
			PopulateListbox();
		}
		private void SkinRemapperForm_Load(object sender, EventArgs e) {
			SetVisibility();
			Render();
		}

		private bool GroupContains(GroupedSvgElems search, IEnumerable<SvgElement> elems) {
			return elems.Any(e => GroupContains(search, e));
		}
		private bool GroupContains(SvgElement search, IEnumerable<SvgElement> elems) {
			return elems.Any(e => GroupContains(search, e));
		}
		private bool GroupContains(GroupedSvgElems search, SvgElement elem) {
			if (search.Any(i => i == elem)) return true;
			foreach (var i in elem.Children) {
				if (GroupContains(search, i)) return true;
			}
			return false;
		}
		private bool GroupContains(SvgElement search, SvgElement group) {
			if (group == search) return true;
			foreach (var c in group.Children) {
				if (GroupContains(search, c)) return true;
			}
			return false;
		}

		private void PopulateListbox() {
			List<GroupedSvgElems> list = new List<GroupedSvgElems>();
			if (rbHighlights.Checked) list.AddRange(_highlights);
			if (rbNonHighlights.Checked) list.AddRange(_nonHighlights);
			if (ckbBase.Checked) list.AddRange(_base);
			lbGroups.DataSource = list;
		}

		private void lbGroups_SelectedIndexChanged(object sender, EventArgs e) {
			if (_selectedGroup != null && (_bkpFill.ToArgb() != _selFill.ToArgb() || _bkpStroke.ToArgb() != _selStroke.ToArgb())) {
				var dr = MessageBox.Show("You have unsaved color changes in this group. Press Yes to apply them or No to discard.",
					"Save changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				if (dr == DialogResult.Yes) {
					btnSave_Click(null, null);
				}
			}

			var group = lbGroups.SelectedItem as GroupedSvgElems;
			SelectGroup(group);
		}

		private void SelectGroup(GroupedSvgElems group) {
			if (_selectedGroup == group) return;
			_selectingGroup = true;

			if (_selectedGroup != null) RestoreGroup();
			_selectedGroup = group;

			colorPicker.Mode = ColorPickerControl.ColorSelectMode.None;
			colorPicker.PrimaryEnabled = pnlFill.Visible = group.Fill is SvgColourServer;
			colorPicker.SecondaryEnabled = pnlStroke.Visible = group.Stroke is SvgColourServer;

			Color cFill = Color.Empty;
			if (group.Fill is SvgColourServer cf)
				cFill = cf.Colour;
			colorPicker.PrimaryColor = _bkpFill = _selFill = cFill;

			Color cStroke = Color.Empty;
			if (group.Stroke is SvgColourServer sf)
				cStroke = sf.Colour;
			colorPicker.SecondaryColor = _bkpStroke = _selStroke = cStroke;

			// instantly highlight and resync timer
			if (cbFlash.Checked) {
				HighlightGroup(true);
				timer.Stop();
				timer.Start();
			}
			_selectingGroup = false;
		}

		private void HighlightGroup(bool isHighlighted) {
			if (_selectedGroup == null) return;
			_isHighlight = isHighlighted;
			foreach (var item in _selectedGroup) {
				if (item.Fill is SvgColourServer fill) {
					Color hl = ColorDistance(_selFill, Color.Red) > 150 ? Color.Red : Color.White;
					fill.Colour = isHighlighted ? hl : _selFill;
				}
				if (item.Stroke is SvgColourServer stroke) {
					Color hl = ColorDistance(_selStroke, Color.Red) > 150 ? Color.Red : Color.White;
					stroke.Colour = isHighlighted ? hl : _selStroke;
				}
			}
			Render();
		}

		private void RestoreGroup() {
			foreach (var item in _selectedGroup) {
				if (item.Fill is SvgColourServer fill)
					fill.Colour = _bkpFill;
				if (item.Stroke is SvgColourServer stroke)
					stroke.Colour = _bkpStroke;
			}
		}

		private static int ColorDistance(Color c1, Color c2) {
			return Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B);
		}

		private void timer_Tick(object sender, EventArgs e) {
			HighlightGroup(!_isHighlight);
		}

		private bool renderInProgress = false;
		private bool renderScheduled = false;
		private async void Render() {
			if (renderInProgress) {
				renderScheduled = true;
				return;
			}

			renderInProgress = true;
			Image img = null;
			await Task.Run(() => {
				_skin.SvgDocument.Width = pbSvg.Width;
				_skin.SvgDocument.Height = pbSvg.Height;
				img = _skin.SvgDocument.Draw();
			});
			pbSvg.Image = img;

			renderInProgress = false;
			if (renderScheduled) {
				renderScheduled = false;
				Render();
			}
		}


		private void SetVisibility() {
			bool hl = rbHighlights.Checked;
			SvgSkin.SetVisibleRecursive(_skin.SvgDocument, ckbBase.Checked);

			foreach (var b in _skin.Buttons) {
				if (b.Pressed != null) {
					SvgSkin.SetVisibleRecursive(b.Pressed, hl);
					if (hl) SvgSkin.SetVisibleToRoot(b.Pressed, true);
				}
				if (b.Element != null) {
					SvgSkin.SetVisibleRecursive(b.Element, !hl);
					if (!hl) SvgSkin.SetVisibleToRoot(b.Element, true);
				}
			}

			foreach (var s in _skin.Sticks) {
				if (s.Pressed != null) {
					SvgSkin.SetVisibleRecursive(s.Pressed, hl);
					if (hl) SvgSkin.SetVisibleToRoot(s.Pressed, true);
				}
				if (s.Element != null) {
					SvgSkin.SetVisibleRecursive(s.Element, !hl);
					if (!hl) SvgSkin.SetVisibleToRoot(s.Element, true);
				}
			}

			foreach (var t in _skin.Triggers) {
				SvgSkin.SetVisibleRecursive(t.Element, !hl);
				SvgSkin.SetVisibleToRoot(t.Element, !hl);
			}
		}

		private void SkinRemapperForm_ResizeEnd(object sender, EventArgs e) {
			Render();
		}

		private void rbBaseItems_CheckedChanged(object sender, EventArgs e) {
			PopulateListbox();
			SetVisibility();
			Render();
		}

		private void rbHighlights_CheckedChanged(object sender, EventArgs e) {
			PopulateListbox();
			SetVisibility();
			Render();
		}

		private void ckbBase_CheckedChanged(object sender, EventArgs e) {
			PopulateListbox();
			SetVisibility();
			Render();
		}

		private void btnSave_Click(object sender, EventArgs e) {
			var tup = Tuple.Create(_selFill, _selStroke);

			foreach (var elem in _selectedGroup) {
				if (elem.Fill is SvgColourServer cf) cf.Colour = _bkpFill = _selFill;
				if (elem.Stroke is SvgColourServer cs) cs.Colour = _bkpStroke = _selStroke;
				Remap.Elements[elem.ID] = tup;
			}
		}

		private void btnRevert_Click(object sender, EventArgs e) {
			_selFill = _bkpFill;
			_selStroke = _bkpStroke;
		}

		private void pbSvg_MouseDown(object sender, MouseEventArgs e) {
			try {
				var clickedElems = FindElemsAt(e.Location);
				foreach (var elem in clickedElems) {
					// find the smallest clicked elem that belongs to any group
					var group = (lbGroups.DataSource as List<GroupedSvgElems>).FirstOrDefault(g => g.Contains(elem));

					// select
					if (group != null) {
						lbGroups.SelectedItem = group;
						return;
					}
				}
			}
			catch (Exception exc) { /* probably concurrency issue while skin is rendering on other thread */ }
		}

		private IEnumerable<SvgVisualElement> FindElemsAt(Point loc) {
			List<SvgVisualElement> candidates = new List<SvgVisualElement>();

			void containsRecurse(SvgElement elem) {
				if (elem is SvgVisualElement vis && vis.Visible) {
					var bounds = SvgSkin.CalcBounds(vis);
					var l = _skin.Unproject(bounds.Location);
					var s = _skin.Unproject(new PointF(bounds.Right, bounds.Bottom));
					var boundsScaled = RectangleF.FromLTRB(l.X, l.Y, s.X, s.Y);

					// if this contains the click point but is smaller yet
					if (boundsScaled.Contains(loc))
						candidates.Add(vis);

				}
				foreach (var child in elem.Children)
					containsRecurse(child);
			}

			containsRecurse(_skin.SvgDocument);

			// select innermost box
			return candidates.OrderBy(e => e.Bounds.Width * e.Bounds.Height);
		}

		private void btnSaveRemap_Click(object sender, EventArgs e) {
			Close();
		}

		protected override void OnFormClosed(FormClosedEventArgs e) {
			base.OnFormClosed(e);
			ConfigManager.SaveRemaps();
		}

	}
}
