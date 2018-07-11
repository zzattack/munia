using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MUNIA.skins;
using MUNIA.Skins;
using Svg;

namespace MUNIA.Forms {
	public partial class SkinRemapperForm : Form {
		private SvgSkin _skin;
		public ColorRemap Remap { get; private set; }

		private Color _bkpStroke;
		private Color _bkpFill;
		private Color _selStroke;
		private Color _selFill;
		private GroupedSvgElems _selectedGroup;
		private bool _isHighlight;

		private List<SvgElement> _highlights = new List<SvgElement>();
		private List<SvgElement> _nonHighlights = new List<SvgElement>();
		private List<SvgElement> _base = new List<SvgElement>();

		private SkinRemapperForm() {
			InitializeComponent();

			colorPicker.PrimaryColorPicked += (sender, args) => {
				_selFill = colorPicker.SelectedPrimaryColor;
				// instantly unhighlight and resync timer
				HighlightGroup(false);
				timer.Stop(); timer.Start();
			};

			colorPicker.SecondaryColorPicked += (sender, args) => {
				_selStroke = colorPicker.SelectedSecondaryColor;
				// instantly unhighlight and resync timer
				HighlightGroup(false);
				timer.Stop(); timer.Start();
			};

		}
		public SkinRemapperForm(string path) : this() {
			this._skin = new SvgSkin();
			this._skin.Load(path);
			this.Remap = ColorRemap.CreateFromSkin(_skin);

			_highlights.AddRange(_skin.Buttons.Where(b => b.Pressed != null).Select(b => b.Pressed));
			_highlights.AddRange(_skin.Sticks.Where(s => s.Pressed != null).Select(s => s.Pressed));
			_nonHighlights.AddRange(_skin.Buttons.Where(b => b.Element != null).Select(b => b.Element));
			_nonHighlights.AddRange(_skin.Sticks.Where(s => s.Element != null).Select(s => s.Element));
			_nonHighlights.AddRange(_skin.Triggers.Where(t => t.Element != null).Select(t => t.Element));

			Action<SvgElement> recurseGet = null;
			recurseGet = element => {
				if (!GroupContains(element, _highlights) && !GroupContains(element, _nonHighlights)) {
					_base.Add(element);
					foreach (var c in element.Children)
						recurseGet(c);
				}
			};
			recurseGet(_skin.SvgDocument);

			lbGroups.DisplayMember = nameof(GroupedSvgElems.Name);
			PopulateListbox();
		}
		private void SkinRemapperForm_Load(object sender, EventArgs e) {
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
			foreach (var group in Remap.Groups) {
				if (GroupContains(group, _highlights)) {
					if (rbHighlights.Checked) list.Add(group);
				}
				else if (GroupContains(group, _nonHighlights)) {
					if (rbNonHighlights.Checked) list.Add(group);
				}
				else if (ckbBase.Checked) list.Add(group);
			}
			lbGroups.DataSource = list;
		}

		private void lbGroups_SelectedIndexChanged(object sender, EventArgs e) {
			var group = lbGroups.SelectedItem as GroupedSvgElems;
			SelectGroup(group);
		}

		private void SelectGroup(GroupedSvgElems group) {
			if (_selectedGroup == group) return;

			if (_selectedGroup != null) RestoreGroup();
			_selectedGroup = group;

			var strk = _selectedGroup.FirstOrDefault(e => e.Stroke is SvgColourServer);
			if (strk != null) _bkpStroke = (strk.Stroke as SvgColourServer).Colour;

			var fill = _selectedGroup.FirstOrDefault(e => e.Fill is SvgColourServer);
			if (fill != null) _bkpFill = (fill.Fill as SvgColourServer).Colour;

			pnlStroke.BackColor = _bkpStroke;
			pnlFill.BackColor = _bkpFill;
			colorPicker.SelectedPrimaryColor = _bkpFill;
			colorPicker.SelectedSecondaryColor = _bkpStroke;

			// instantly highlight and resync timer
			HighlightGroup(true);
			timer.Stop(); timer.Start();
		}

		private void HighlightGroup(bool isHighlighted) {
			_isHighlight = isHighlighted;
			foreach (var item in _selectedGroup) {
				if (item.Stroke is SvgColourServer stroke) {
					Color hl = ColorDistance(_selStroke, Color.Red) > 150 ? Color.Red : Color.White;
					stroke.Colour = isHighlighted ? hl : _selStroke;
				}
				if (item.Fill is SvgColourServer fill) {
					Color hl = ColorDistance(_selFill, Color.Red) > 150 ? Color.Red : Color.White;
					fill.Colour = isHighlighted ? hl : _selFill;
				}
			}
			Render();
		}

		private void RestoreGroup() {
			foreach (var item in _selectedGroup) {
				if (item.Stroke is SvgColourServer stroke) stroke.Colour =  _bkpStroke;
				if (item.Fill is SvgColourServer fill) fill.Colour = _bkpFill;
			}
		}

		private static int ColorDistance(Color c1, Color c2) {
			return Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B);
		}

		private void timer_Tick(object sender, EventArgs e) {
			HighlightGroup(!_isHighlight);
		}

		private void Render() {
			_skin.SvgDocument.Width = pbSvg.Width;
			_skin.SvgDocument.Height = pbSvg.Height;
			SetVisibility();
			pbSvg.Image = _skin.SvgDocument.Draw();
		}

		private void SetVisibility() {
			bool hl = rbHighlights.Checked;
			SvgSkin.SetVisibleRecursive(_skin.SvgDocument, ckbBase.Checked);

			foreach (var b in _skin.Buttons) {
				if (b.Element != null) {
					SvgSkin.SetVisibleRecursive(b.Element, !hl);
					if (!hl) SvgSkin.SetVisibleToRoot(b.Element, true);
				}
				if (b.Pressed != null) {
					SvgSkin.SetVisibleRecursive(b.Pressed, hl);
					if (hl) SvgSkin.SetVisibleToRoot(b.Pressed, true);
				}
			}

			foreach (var s in _skin.Sticks) {
				if (s.Element != null) {
					SvgSkin.SetVisibleRecursive(s.Element, !hl);
					SvgSkin.SetVisibleToRoot(s.Element, !hl);
				}
				if (s.Pressed != null) {
					SvgSkin.SetVisibleRecursive(s.Pressed, hl);
					SvgSkin.SetVisibleToRoot(s.Pressed, hl);
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
			Render();
		}

		private void rbHighlights_CheckedChanged(object sender, EventArgs e) {
			PopulateListbox();
			Render();
		}

		private void ckbBase_CheckedChanged(object sender, EventArgs e) {
			PopulateListbox();
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

	}
}
