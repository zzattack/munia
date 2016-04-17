using System.Collections.Generic;
using System.Drawing;
using Svg;
using System.Linq;
using System;
using System.Windows.Forms;
using MuniaInput;
using OpenTK.Graphics.OpenGL;

namespace MUNIA {
    public class SvgController {

        private SvgDocument _svgDocument;
        private MuniaController _inputDevice;

        public string DeviceName { get; set; }
        public List<Button> Buttons = new List<Button>();
        public List<Stick> Sticks = new List<Stick>();
        public List<Trigger> Triggers = new List<Trigger>();
        private SizeF _dimensions;
        public int BaseTexture { get; set; }

        public enum LoadResult {
            Ok,
            NoController,
            Fail
        }
        public LoadResult Load(string svgPath) {
            try {
                _svgDocument = SvgDocument.Open(svgPath);
                _dimensions = _svgDocument.GetDimensions();

                // load button/stick/trigger mapping from svg
                Buttons.Clear();
                Sticks.Clear();
                Triggers.Clear();
                DeviceName = string.Empty;
                RecursiveGetElements(_svgDocument);

                // find input device
                _inputDevice?.Dispose();
                _inputDevice = MuniaController.ListDevices().FirstOrDefault(d => d.HidDevice.Product == DeviceName);
                return _inputDevice == null ? LoadResult.NoController : LoadResult.Ok;
            }
            catch { return LoadResult.Fail; }
        }

        public void Activate(IntPtr wnd) {
            _inputDevice.Activate(wnd);
            _inputDevice.StateUpdated += InputDeviceOnStateUpdated;
        }

        private void RecursiveGetElements(SvgElement e) {
            foreach (var c in e.Children) {
                if (c.ElementName == "info") {
                    DeviceName = c.CustomAttributes["device-name"];
                }

                if (c.ContainsAttribute("button-id")) {
                    var b = c as SvgVisualElement;
                    int id = int.Parse(c.CustomAttributes["button-id"]);
                    bool pressed = c.CustomAttributes["button-state"] == "pressed";
                    Buttons.EnsureSize(id);

                    if (pressed)
                        Buttons[id].Pressed = b;
                    else
                        Buttons[id].Released = b;
                }
                else if (c.ContainsAttribute("stick-id")) {
                    var s = c as SvgVisualElement;
                    int id = int.Parse(c.CustomAttributes["stick-id"]);
                    Sticks.EnsureSize(id);
                    Sticks[id].Element = s;
                    Sticks[id].HorizontalAxis = int.Parse(c.CustomAttributes["axis-h"]);
                    Sticks[id].VerticalAxis = int.Parse(c.CustomAttributes["axis-v"]);
                    Sticks[id].OffsetScale = float.Parse(c.CustomAttributes["offset-scale"]);
                }
                else if (c.ContainsAttribute("trigger-id")) {
                    var s = c as SvgVisualElement;
                    int id = int.Parse(c.CustomAttributes["trigger-id"]);
                    Triggers.EnsureSize(id);
                    Triggers[id].Element = s;
                    Triggers[id].Axis = int.Parse(c.CustomAttributes["trigger-axis"]);
                    Triggers[id].OffsetScale = float.Parse(c.CustomAttributes["offset-scale"]);
                }
                RecursiveGetElements(c);
            }
        }
        
        internal void Render(int width, int height) {
            if (_svgDocument == null || width == 0 || height == 0) return;
            if (_svgDocument.Height != height || _svgDocument.Width != width) {
                RenderBase(width, height);
            }
        }

        private void RenderBase(int width, int height) {
            _svgDocument.Height = height;
            _svgDocument.Width = width;
            
            // hide changable elements first
            foreach (var b in Buttons) {
                if (b.Pressed != null) b.Pressed.Visible = false;
                if (b.Released != null) b.Released.Visible = false;
            }
            foreach (var s in Sticks) {
                s.Element.Visible = false;
            }
            foreach (var t in Triggers) {
                t.Element.Visible = false;
            }

            var img = _svgDocument.Draw();
            BaseTexture = GLGraphics.CreateTexture(img);
        }

        internal void Render() {

            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, BaseTexture);

            RenderTexture(0, _svgDocument.Width, 0, _svgDocument.Height);

            // todo render buttons/sticks/triggers
            /*
            foreach (SvgGroup b in _controller.Buttons) {
                if (!_controller.Toggled[b]) continue;
                var t = _controller.ToggledTextures[b];
                GL.BindTexture(TextureTarget.Texture2D, t.Item2);

                RenderTexture(t.Item1.X, t.Item1.X + t.Item1.Width, 
                    t.Item1.Y, t.Item1.Y + t.Item1.Height);
            }*/
        }

        private void RenderToggledButton(SvgGroup g) {
            g.Visible = true;
            var b = CalcBounds(g);
            var l = Unproject(b.Location);
            var s = Unproject(new PointF(b.X + b.Width, b.Y + b.Height));
            var r = new RectangleF(l.X, l.Y, s.X - l.X, s.Y - l.Y);
            var img = _svgDocument.Draw();
            var t = GLGraphics.CreateTexture(img.Clone(r, img.PixelFormat));
            // ToggledTextures[g] = new Tuple<RectangleF, int>(r, t);
            g.Visible = false;
        }
        private static void RenderTexture(float l, float r, float t, float b) {
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex2(l, t);
            GL.TexCoord2(1, 0);
            GL.Vertex2(r, t);
            GL.TexCoord2(1, 1);
            GL.Vertex2(r, b);
            GL.TexCoord2(0, 1);
            GL.Vertex2(l, b);
            GL.End();
        }

        internal PointF Project(PointF p, float width, float height) {
            float svgAR = _dimensions.Width / _dimensions.Height;
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

            var x = p.X / width * _dimensions.Width;
            var y = p.Y / height * _dimensions.Height;
            return new PointF(x, y);
        }

        internal PointF Unproject(PointF p) {
            float svgAR = _dimensions.Width / _dimensions.Height;
            float imgAR = _svgDocument.Width / _svgDocument.Height;
            var width = _svgDocument.Width;
            var height = _svgDocument.Height;
            if (svgAR > imgAR)
                height = width / svgAR;
            else
                width = height * svgAR;

            var x = p.X / _dimensions.Width * width;
            var y = p.Y / _dimensions.Height * height;

            if (svgAR > imgAR)
                p.Y += ((_svgDocument.Height - _svgDocument.Width / svgAR) / 2f);
            else
                p.X -= ((_svgDocument.Width - _svgDocument.Height * svgAR) / 2f);

            return new PointF(x, y);
        }

        public RectangleF CalcBounds(SvgElement x) {
            var b = (x as ISvgBoundable).Bounds;
            // x = x.Parent;
            while (x is SvgVisualElement) {
                var m = x.Transforms.GetMatrix();
                b.Offset(m.OffsetX, m.OffsetY);
                x = x.Parent;
            }
            return b;
        }

        public void WndProc(ref Message message) {
            _inputDevice?.WndProc(ref message);
        }

        private void InputDeviceOnStateUpdated(object sender, EventArgs args) {
        }
    }

    public class Button {
        public SvgVisualElement Pressed;
        public SvgVisualElement Released;
        public int PressedTexture;
        public int PressedReleased;
    }
    public class Stick {
        public SvgVisualElement Element;
        public float OffsetScale;
        public int HorizontalAxis;
        public int VerticalAxis;
        public int Texture;
    }
    public class Trigger {
        public SvgVisualElement Element;
        public float OffsetScale;
        public int Axis;
        public int Texture;
    }

    static class ExtensionMethods {
        public static void EnsureSize<T>(this List<T> list, int count) {
            while (list.Count <= count) list.Add(Activator.CreateInstance<T>());
        }
    }

}
