using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Svg;
using GlControlBasics;
using System.Diagnostics;
using System.Linq;
using System;

namespace MUNIA {
	public class SvgController {
	    public SvgController() {
	        Axes = new List<int>();
	        ButtonPressed = new List<bool>();
	        Hats = new List<int>();
	    }

        public List<SvgElement> Buttons = new List<SvgElement>();


        public Dictionary<SvgElement, bool> Toggled = new Dictionary<SvgElement, bool>();
        public Dictionary<SvgElement, Tuple<RectangleF, int>> ToggledTextures = new Dictionary<SvgElement, Tuple<RectangleF, int>>();

        private SvgDocument _svgDocument;

        public int TextureHandle { get; set; } 

	    public void Load(string svgPath) {
            _svgDocument = SvgDocument.Open(svgPath);
            Dimensions = _svgDocument.GetDimensions();
            RecursiveGetButtons(_svgDocument);
            Buttons.ForEach(x => {
                ((SvgGroup)x).Visible = false;
                Toggled[x] = false;
            });
        }

        private void RecursiveGetButtons(SvgElement e) {
            foreach (var c in e.Children) {
                if (c?.ID == null) continue;
                if (c.ID.Contains("Pressed")) {
                    var b = c as SvgGroup;
                    if (b == null) continue;
                    Buttons.Add(b);
                    var r = GetBounds(b);
                    Debug.WriteLine("Bounding box for button '{0}': [({1},{2}),({3},{4})]", c.ID, r.X, r.Y, r.Right, r.Bottom);
                }
                RecursiveGetButtons(c);
            }           
        }

        internal void Render(int width, int height) {
            if (_svgDocument == null || width == 0 || height == 0) return;
            _svgDocument.Height = height;
            _svgDocument.Width = width;

            Buttons.ForEach(x => ((SvgGroup)x).Visible = false);

            var img = _svgDocument.Draw();
            TextureHandle = GLGraphics.CreateTexture(img);

            foreach (SvgGroup g in Buttons) {
            
                RenderToggledButton(g);
        
            }
        }

        private void RenderToggledButton(SvgGroup g) {
            g.Visible = true;
            var b = GetBounds(g);
            var l = Unproject(b.Location);
            var s = Unproject(new PointF(b.X + b.Width, b.Y + b.Height));
            var r = new RectangleF(l.X, l.Y, s.X - l.X, s.Y - l.Y);
            var img = _svgDocument.Draw();
            var t = GLGraphics.CreateTexture(img.Clone(r, img.PixelFormat));
            ToggledTextures[g] = new Tuple<RectangleF, int>(r, t);
            g.Visible = false;
        }

        internal PointF Project(PointF p, float width, float height) {
            float svgAR = Dimensions.Width / Dimensions.Height;
            float imgAR = width / height;
            int dx = 0, dy = 0;
            if (svgAR > imgAR) {
                // compensate for black box
                p.Y -= ((height - width / svgAR) / 2f);
                // adjust ratio
                height = width / svgAR;
            }
            else {
                // compensate for black box
                p.X -= ((width - height * svgAR) / 2f);
                // adjust ratio
                width = height * svgAR;
            }

            var x = p.X / width * Dimensions.Width;
            var y = p.Y / height * Dimensions.Height;
            return new PointF(x, y);
        }

        internal PointF Unproject(PointF p) {
            float svgAR = Dimensions.Width / Dimensions.Height;
            float imgAR = _svgDocument.Width / _svgDocument.Height;
            var width = _svgDocument.Width;
            var height = _svgDocument.Height;
            if (svgAR > imgAR)
                height = width / svgAR;
            else
                width = height * svgAR;

            var x = p.X / Dimensions.Width * width;
            var y = p.Y / Dimensions.Height * height;

            if (svgAR > imgAR)
                p.Y += ((_svgDocument.Height - _svgDocument.Width / svgAR) / 2f);
            else
                p.X -= ((_svgDocument.Width - _svgDocument.Height * svgAR) / 2f);

            return new PointF(x, y);
        }

        public RectangleF GetBounds(SvgElement x) {
            var b = (x as ISvgBoundable).Bounds;
            // x = x.Parent;
            while (x is SvgVisualElement) {
                var m = x.Transforms.GetMatrix();
                b.Offset(m.OffsetX, m.OffsetY);
                x = x.Parent;
            }
            return b;
        }

        public List<bool> ButtonPressed { get; private set; }
        public List<int> Axes { get; private set; } 
        public List<int> Hats { get; private set; }
        public SizeF Dimensions { get; private set; }

        /*public static IEnumerable<Device> ListDevices() {
            RAWINPUTDEVICELIST[] ridList = null;
            uint deviceCount = 0;
            // first call to determine number of devices available
            int res = Function.GetRawInputDeviceList(ridList, ref deviceCount, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
            if (res != 0) yield break;
            // allocate and fetch
            ridList = new RAWINPUTDEVICELIST[deviceCount];
            res = Function.GetRawInputDeviceList(ridList, ref deviceCount, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
            if (res != deviceCount) yield break;

            //For each our device add a node to our treeview
            foreach (RAWINPUTDEVICELIST device in ridList) {
                Device hidDevice;

                //Try create our HID device.
                try {
                    hidDevice = new Device(device.hDevice);
                }
                catch {
                    //Just skip that device then
                    continue;
                }
                if (hidDevice.Product == "zzattack.org") {
                    yield return hidDevice;
                }
            }
        }*/

	}
}
