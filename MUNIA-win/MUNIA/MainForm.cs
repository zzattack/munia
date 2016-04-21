using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using SharpLib.Win32;
using SPAA05.Shared.USB;

namespace MUNIA {
    public partial class MainForm : Form {

        private readonly Stopwatch _stopwatch = new Stopwatch();
        private SvgController _controller;
		private double _timestamp;
		private int _frames;
        private int _fps;

        public Point MouseLocation { get; private set; }
        public Point MouseClickLocation { get; set; }
        public bool MouseClicked { get; private set; }
        
        public MainForm() {
            InitializeComponent();
            glControl.Resize += GlControl1OnResize;
			UsbNotification.DeviceArrival += (sender, args) => BuildMenu();
			UsbNotification.DeviceRemovalComplete += (sender, args) => BuildMenu();
		}
        
        private void MainForm_Shown(object sender, EventArgs e) {
			BuildMenu();

			Application.Idle += OnApplicationOnIdle;

#if DEBUG
			tsmiController.DropDownItems[0].PerformClick();
			GlControl1OnResize(this, EventArgs.Empty);
#endif
		}

		private void BuildMenu() {
			tsmiController.DropDownItems.Clear();
			foreach (string svgPath in Directory.GetFiles("./skins", "*.svg")) {
				var svgc = new SvgController();
				var res = svgc.Load(svgPath);
				if (res != SvgController.LoadResult.Fail) {
					var tsmi = new ToolStripMenuItem($"{svgc.SkinName} ({svgc.DeviceName})");
					tsmi.Enabled = res == SvgController.LoadResult.Ok;
					tsmi.Click += delegate {
						_controller = svgc;
						_controller?.Render(glControl.Width, glControl.Height);
						svgc.Activate(Handle);
					};
					tsmiController.DropDownItems.Add(tsmi);
				}
			}
		}

		private void glControl_Load(object sender, EventArgs e) {
            glControl.MakeCurrent();
            glControl.VSync = true;
        }

        private void OnApplicationOnIdle(object s, EventArgs a) {
            while (glControl.IsIdle) {
                _stopwatch.Restart();
                Render();
                _frames++;

                // Every second, update the frames_per_second count
                double now = _stopwatch.Elapsed.TotalSeconds;
                if (now - _timestamp >= 1.0) {
                    _fps = _frames;
                    _frames = 0;
                    _timestamp = now;
                }

                _stopwatch.Stop();
                // Thread.Sleep((int)Math.Max(1000 / 60.0 - _stopwatch.Elapsed.TotalSeconds, 0));
            }
        }

        /// <summary>
        /// Hook in HID handler.
        /// </summary>
        /// <param name="message"></param>
        protected override void WndProc(ref Message message) {
            switch (message.Msg) {
                case Const.WM_INPUT:
                    // Returning zero means we processed that message
                    message.Result = IntPtr.Zero;
                    _controller?.WndProc(ref message);
                    break;
            }
            base.WndProc(ref message);
        }

        private void Render() {
            glControl.MakeCurrent();
            GL.ClearColor(Color.FromArgb(0, glControl.BackColor));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, glControl.Width, glControl.Height, 0, 0.0, 4.0);
            _controller?.Render();

            glControl.SwapBuffers();
        }


        private void GlControl1OnResize(object sender, EventArgs e) {
            _controller?.Render(glControl.Width, glControl.Height);
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
        }
        
    }
}
