using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
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
			tsmiController.DropDownItems[0].Enabled = true;
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

		private void setWindowSizeToolStripMenuItem_Click(object sender, EventArgs e) {
			var frm = new WindowSizePicker(glControl.Size);
			if (frm.ShowDialog() == DialogResult.OK) {
				this.Size = frm.ChosenSize - glControl.Size + this.Size;
			}
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			new AboutBox().Show(this);
		}

		#region update checking/performing
		private void tsmiCheckUpdates_Click(object sender, EventArgs e) {
			statusStrip1.Visible = true;
			PerformUpdateCheck();
		}

		private void UpdateStatus(string text, int progressBarValue) {
			if (InvokeRequired) {
				Invoke((Action<string,int>)UpdateStatus, text, progressBarValue);
				return;
			}
			if (progressBarValue < pbProgress.Value && pbProgress.Value != 100) {
				return;
			}

			lblStatus.Text = "Status: " + text;
			if (progressBarValue < 100)
				// forces 'instant update'
				pbProgress.Value = progressBarValue + 1;
			pbProgress.Value = progressBarValue;
		}

		private void PerformUpdateCheck() {
			var uc = new UpdateChecker();
			uc.AlreadyLatest += (o, e) => UpdateStatus("already latest version", 100);
			uc.Connected += (o, e) => UpdateStatus("connected", 10);
			uc.DownloadProgressChanged += (o, e) => { /* care, xml is small anyway */ };
			uc.UpdateCheckFailed += (o, e) => UpdateStatus("update check failed", 100);
			uc.UpdateAvailable += (o, e) => {
				UpdateStatus("update available", 100);

				var dr =
					MessageBox.Show(
						string.Format(
							"An update to version {0} released on {1} is available. Release notes: \r\n\r\n{2}\r\n\r\nUpdate now?",
							e.Version.ToString(), e.ReleaseDate.ToShortDateString(), e.ReleaseNotes), "Update available",
						MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
				if (dr == DialogResult.Yes)
					DownloadAndUpdate(e.DownloadUrl);
			};
			uc.CheckVersion();
		}
		private void DownloadAndUpdate(string url) {
			UpdateStatus("downloading new program version", 0);
			var wc = new WebClient();
			wc.Proxy = null;

			var address = new Uri(UpdateChecker.UpdateCheckHost + url);
			wc.DownloadProgressChanged += (sender, args) => BeginInvoke((Action)delegate {
				UpdateStatus(string.Format("downloading, {0}%", args.ProgressPercentage * 95 / 100), args.ProgressPercentage * 95 / 100);
			});

			wc.DownloadDataCompleted += (sender, args) => {
				UpdateStatus("download complete, running installer", 100);
				string appPath = Path.GetTempPath();
				string dest = Path.Combine(appPath, "MUNIA_update");

				int suffixNr = 0;
				while (File.Exists(dest + (suffixNr > 0 ? suffixNr.ToString() : "") + ".exe"))
					suffixNr++;

				dest += (suffixNr > 0 ? suffixNr.ToString() : "") + ".exe";
				File.WriteAllBytes(dest, args.Result);
				// invoke 
				var psi = new ProcessStartInfo(dest);
				psi.Arguments = "/Q";
				Process.Start(psi);
				Close();
			};

			// trigger it all
			wc.DownloadDataAsync(address);
		}


		#endregion
	}
}
