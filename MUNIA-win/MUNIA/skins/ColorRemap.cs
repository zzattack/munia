using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;
using MUNIA.Skins;
using Svg;

namespace MUNIA.Skins {
	public class ColorRemap {
		public string Name { get; set; }
		private List<GroupedSvgElems> _groups = new List<GroupedSvgElems>();
		public List<GroupedSvgElems> Groups => _groups;
		public readonly Dictionary<string, Tuple<Color, Color>> Elements = new Dictionary<string, Tuple<Color, Color>>();

		private void InitGroups(SvgSkin skin) {
			// groups svg elements by fill and stroke
			var all = skin.SvgDocument.Children.FindSvgElementsOf<SvgElement>()
				.Where(e => e.ContainsAttribute("fill") || e.ContainsAttribute("stroke"));
			var groups = all.GroupBy(e => new { e.Fill, e.Stroke }).ToList();
			_groups.AddRange(groups.Select(g => new GroupedSvgElems(g.AsEnumerable())));
		}

		public static ColorRemap CreateFromSkin(SvgSkin skin) {
			var ret = new ColorRemap();
			ret.InitGroups(skin);

			// extract fills and strokes from skin
			foreach (var group in ret._groups) {
				foreach (var elem in group) {
					// ensure element has an internal id which we can use to refer to it from our dict
					if (string.IsNullOrEmpty(elem.ID))
						elem.SetAndForceUniqueID(elem.ElementName, true, null);

					Color fill = Color.Empty;
					Color stroke = Color.Empty;
					if (elem.Fill is SvgColourServer cf) fill = cf.Colour;
					if (elem.Fill is SvgColourServer cs) stroke = cs.Colour;
					ret.Elements[elem.ID] = Tuple.Create(fill, stroke);
				}
			}
			return ret;
		}

		public void AttachToSkin(SvgSkin skin) {
			InitGroups(skin);

			foreach (var group in _groups) {
				foreach (var elem in group) {
					if (string.IsNullOrEmpty(elem.ID)) elem.SetAndForceUniqueID("elem", true, null);

					Color fill = Color.Empty;
					Color stroke = Color.Empty;
					if (elem.Fill is SvgColourServer cf) fill = cf.Colour;
					if (elem.Fill is SvgColourServer cs) stroke = cs.Colour;
					Elements[elem.ID] = Tuple.Create(fill, stroke);
				}
			}
		}


		public static ColorRemap LoadFrom(XmlNode xRemap) {
			ColorRemap ret = new ColorRemap();
			foreach (XmlNode x in xRemap.ChildNodes) {
				string id = x.Attributes["id"].Value;
				Color fill = ColorTranslator.FromHtml(x.Attributes["fill"].Value);
				Color stroke = ColorTranslator.FromHtml(x.Attributes["stroke"].Value);

				ret.Elements[id] = Tuple.Create(fill, stroke);
			}
			return ret;
		}

		public void Saveto(XmlTextWriter xw) {
			xw.WriteStartElement("remap");
			xw.WriteAttributeString("name", Name);
			foreach (var kvp in Elements) {
				xw.WriteStartElement("entry");
				xw.WriteAttributeString("id", kvp.Key);
				xw.WriteAttributeString("fill", ColorTranslator.ToHtml(kvp.Value.Item1));
				xw.WriteAttributeString("id", ColorTranslator.ToHtml(kvp.Value.Item2));
			}
			
			xw.WriteEndElement(); // remap
		}

	}

	public class GroupedSvgElems : List<SvgElement> {
		public GroupedSvgElems(IEnumerable<SvgElement> svgElements) {
			this.AddRange(svgElements);
		}


		public string Name => this.ToString();
		public override string ToString() {
			return string.Join(", ", this.Select(e => e.ID));
		}
	}

}
