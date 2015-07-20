using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MUNIA {
    public partial class MainForm : Form {

        private const int maxFPS = 60;
        private Stopwatch _stopwatch = new Stopwatch();
        private OpenTK.GLControl glControl1 = new OpenTK.GLControl(new GraphicsMode(32, 24, 8, 8));

        public MainForm() {
            InitializeComponent();
            Controls.Add(glControl1);
            glControl1.Dock = DockStyle.Fill;
            glControl1.Resize += GlControl1OnResize;
        }

        private void MainForm_Shown(object sender, EventArgs e) {
            Application.Idle += (s, a) => {
                while (glControl1.IsIdle) {
                    glControl1.MakeCurrent();
                    _stopwatch.Restart();
                    GL.ClearColor(Color.CornflowerBlue);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();
                    GL.MatrixMode(MatrixMode.Projection);
                    _stopwatch.Stop();
                    glControl1.SwapBuffers();
                    Thread.Sleep((int)(Math.Max(1000f / maxFPS - _stopwatch.Elapsed.TotalMilliseconds, 0)));
                }
            };
        }

        private void GlControl1OnResize(object sender, EventArgs e) {          
        }
    }
}
