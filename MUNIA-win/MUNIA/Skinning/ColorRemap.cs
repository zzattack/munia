using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using MUNIA.Properties;
using MUNIA.Util;
using Svg;

namespace MUNIA.Skinning {
	public class ColorRemap : INotifyPropertyChanged {
		private string _name;

		private List<GroupedSvgElems> _groups = new List<GroupedSvgElems>();
		public bool IsSkinDefault;
		public Guid UUID { get; private set; } = Guid.Empty;

		public List<GroupedSvgElems> Groups => _groups;
		public readonly Dictionary<string, Tuple<Color, Color>> Elements = new Dictionary<string, Tuple<Color, Color>>();

		public ColorRemap(bool skinDefault = false) {
			IsSkinDefault = skinDefault;
			if (!IsSkinDefault) UUID = Guid.NewGuid();
		}

		private void InitGroups(SvgSkin skin) {
			// groups svg elements by fill and stroke
			var all = skin.SvgDocument.Children.FindSvgElementsOf<SvgVisualElement>()
				.Where(e => e.ContainsAttribute("fill") || e.ContainsAttribute("stroke"));

			_groups.Clear();
			var grouped = all.GroupBy(e => new { e.Fill, e.Stroke }).ToList();
			foreach (var group in grouped) {
				var g = new GroupedSvgElems(group.Key.Fill, group.Key.Stroke);
				g.AddRange(group.AsEnumerable());
				_groups.Add(g);
			}
		}

		public static ColorRemap CreateFromSkin(SvgSkin skin) {
			var ret = new ColorRemap(true);
			ret.InitGroups(skin);
			ret.Name = "Default theme";

			// extract fills and strokes from skin
			foreach (var group in ret._groups) {
				foreach (var elem in group) {
					// ensure element has an internal id which we can use to refer to it from our dict
					if (string.IsNullOrEmpty(elem.ID))
						elem.SetAndForceUniqueID(elem.ElementName, true, null);

					Color fill = Color.Empty;
					Color stroke = Color.Empty;
					if (elem.Fill is SvgColourServer cf) fill = cf.Colour;
					if (elem.Stroke is SvgColourServer cs) stroke = cs.Colour;
					ret.Elements[elem.ID] = Tuple.Create(fill, stroke);
				}
			}
			return ret;
		}

		public void ApplyToSkin(SvgSkin skin) {
			InitGroups(skin);

			foreach (var group in _groups) {
				foreach (var elem in group) {
					if (string.IsNullOrEmpty(elem.ID)) elem.SetAndForceUniqueID("elem", true, null);

					if (Elements.ContainsKey(elem.ID)) {
						var tup = Elements[elem.ID];
						if (elem.Fill is SvgColourServer cf) cf.Colour = tup.Item1;
						if (elem.Stroke is SvgColourServer cs) cs.Colour = tup.Item2;
					}
				}
			}
		}


		public static ColorRemap LoadFrom(XmlNode xRemap) {
			ColorRemap ret = new ColorRemap();
			ret.Name = xRemap.Attributes["name"].Value;
			ret.UUID = Guid.Parse(xRemap.Attributes["UUID"].Value);

			foreach (XmlNode x in xRemap.ChildNodes) {
				string id;
				if (x.Attributes["tgt-id"] != null) id = x.Attributes["tgt-id"].Value;
				else if (x.Attributes["id"] != null) id = x.Attributes["id"].Value; // legacy but can only contain one remap per skin if using id
				else continue;

				Color fill = ColorTranslator.FromHtml(x.Attributes["fill"].Value);
				Color stroke = ColorTranslator.FromHtml(x.Attributes["stroke"].Value);

				ret.Elements[id] = Tuple.Create(fill, stroke);
			}
			return ret;
		}
		public static ColorRemap LoadFrom(SvgElement xRemap) {
			ColorRemap ret = new ColorRemap();
			ret.Name = xRemap.CustomAttributes["name"];
			ret.UUID = Guid.Parse(xRemap.CustomAttributes["UUID"]);

			foreach (SvgElement x in xRemap.Children) {
				string id;
				if (x.CustomAttributes.ContainsKey("tgt-id")) id = x.CustomAttributes["tgt-id"];
				else id = x.ID; // legacy thing

				Color fill = new Color(), stroke = new Color();
				if (x.Fill is SvgColourServer cf) fill = cf.Colour;
				if (x.Stroke is SvgColourServer cs) stroke = cs.Colour;
				ret.Elements[id] = Tuple.Create(fill, stroke);
			}
			return ret;
		}

		public void SaveTo(XmlTextWriter xw) {
			// there's no point in saving the default as it
			// should be recreated at application load and may not be edited
			if (IsSkinDefault) return;

			xw.WriteStartElement("remap");
			xw.WriteAttributeString("name", Name);
			xw.WriteAttributeString("UUID", UUID.ToString());

			foreach (var kvp in Elements) {
				xw.WriteStartElement("entry");
				xw.WriteAttributeString("tgt-id", kvp.Key);
				xw.WriteAttributeString("fill", kvp.Value.Item1.ToHexValue());
				xw.WriteAttributeString("stroke", kvp.Value.Item2.ToHexValue());
				xw.WriteEndElement(); // entry
			}
			xw.WriteEndElement(); // remap
		}

		public ColorRemap Clone() {
			var ret = new ColorRemap(false);
			ret.Name = this.Name + " (copy)";

			foreach (var kvp in this.Elements)
				ret.Elements[kvp.Key] = kvp.Value;
			ret.IsSkinDefault = false;

			return ret;
		}
		public string Name {
			get => _name;
			set {
				if (value == _name) return;
				_name = value;
				OnPropertyChanged();
			}
		}

		protected bool Equals(ColorRemap other) {
			return UUID.Equals(other.UUID);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((ColorRemap)obj);
		}

		public override int GetHashCode() {
			return UUID.GetHashCode();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public override string ToString() {
			return $"{(IsSkinDefault?"[D] ":"")}{Name}";
		}
	}

	public class GroupedSvgElems : List<SvgElement> {
		public GroupedSvgElems(SvgPaintServer fill, SvgPaintServer stroke) {
			Fill = fill;
			Stroke = stroke;
		}

		public SvgPaintServer Fill { get; private set; } = SvgPaintServer.None;
		public SvgPaintServer Stroke { get; private set; } = SvgPaintServer.None;

		public string Name => this.ToString();
		public override string ToString() {
			return string.Join(", ", this.Select(e => e.ID));
		}
	}

}
