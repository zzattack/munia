using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using System.Linq;
using Svg;
using System.Threading;

namespace MUNIA {
    public partial class MainForm : Form {

        private readonly Stopwatch _stopwatch = new Stopwatch();
        private SvgController _controller = new SvgController();

        private string gcpath = @"./svg/gc.svg";
        private string snespath = @"./svg/gc.svg";
        private string n64path = @"./svg/gc.svg";

        double timestamp;
        int frames;
        int fps;

        public Point MouseLocation { get; private set; }
        public Point MouseClickLocation { get; set; }
        public bool MouseClicked { get; private set; }


        public MainForm() {
            InitializeComponent();
            glControl.Resize += GlControl1OnResize;
            glControl.MouseClick += GlControl_Click;
            glControl.MouseMove += GlControl_MouseMove;
        }

        private void GlControl_MouseMove(object sender, MouseEventArgs e) {
            MouseLocation = e.Location;
        }

        private void GlControl_Click(object sender, MouseEventArgs e) {
            MouseClicked = true;
            MouseClickLocation = e.Location;
        }

        private void ResolveMouseClicks() {
            if (!MouseClicked) return;

            var pc = _controller.Project(MouseClickLocation, glControl.Width, glControl.Height);
            Debug.WriteLine("Clicked @ ({0},{1})", pc.X, pc.Y);

            var b = _controller.Buttons.FirstOrDefault(x => _controller.GetBounds((SvgGroup)x).Contains(pc)) as SvgGroup;

            if (b != null) {
                b.Visible = !b.Visible;
                _controller.Render(glControl.Width, glControl.Height);
            }

            MouseClicked = false;
        }

        private void MainForm_Shown(object sender, EventArgs e) {
            _controller.Load(gcpath);
            Application.Idle += OnApplicationOnIdle;
            GlControl1OnResize(this, EventArgs.Empty);
        }

        private void OnApplicationOnIdle(object s, EventArgs a) {
            while (glControl.IsIdle) {
                Render();
                frames++;

                ResolveMouseClicks();

                // Every second, update the frames_per_second count
                double now = _stopwatch.Elapsed.TotalSeconds;
                if (now - timestamp >= 1.0) {
                    fps = frames;
                    frames = 0;
                    timestamp = now;
                }
                // Thread.Sleep((int)Math.Max(1000 / 60.0 - _stopwatch.Elapsed.TotalSeconds, 0));
            }
        }

        private void Render() {
            glControl.MakeCurrent();
            _stopwatch.Restart();
            GL.ClearColor(Color.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _controller.TextureHandle);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);   
            GL.Vertex2(-1.0f, 1.0f);
            GL.TexCoord2(1, 0);
            GL.Vertex2(1.0f, 1.0f);
            GL.TexCoord2(1, 1);
            GL.Vertex2(1.0f, -1.0f);
            GL.TexCoord2(0, 1);
            GL.Vertex2(-1.0f, -1.0f);
            GL.End();
            
            _stopwatch.Stop();
            glControl.SwapBuffers();
   
        }
        

        private void GlControl1OnResize(object sender, EventArgs e) {
            _controller.Render(glControl.Width, glControl.Height);
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
        }

        private void somsToolStripMenuItem_Click(object sender, EventArgs e) {
            _controller.Load(snespath);
            GlControl1OnResize(this, EventArgs.Empty);
        }

        private void gCToolStripMenuItem_Click(object sender, EventArgs e) {
            _controller.Load(gcpath);
            GlControl1OnResize(this, EventArgs.Empty);
        }

        private void n64ToolStripMenuItem_Click(object sender, EventArgs e) {
            _controller.Load(n64path);
            GlControl1OnResize(this, EventArgs.Empty);
        }
    }
}
