using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HidSharp;
using MuniaInput;
using OpenTK.Graphics.OpenGL;

namespace MUNIA {
	public partial class MainForm : Form {
		private readonly Stopwatch _stopwatch = new Stopwatch();
		private SvgController _controller;
		private double _timestamp;
		private int _frames;
		private readonly bool _skipUpdateCheck;
		private int _fps;
		
		private MainForm() {
			InitializeComponent();
		}

		public MainForm(bool skipUpdateCheck) : this() {
			glControl.Resize += OnResize;
			UsbNotification.DeviceArrival += (sender, args) => { BuildMenu(); };
			UsbNotification.DeviceRemovalComplete += (sender, args) => { BuildMenu(); };
			_skipUpdateCheck = skipUpdateCheck;
			Text += " - v" + Assembly.GetEntryAssembly().GetName().Version;
		}

		private void MainForm_Shown(object sender, EventArgs e) {
			Settings.Load();
			SkinManager.Load();
			// restore saved sizes
			foreach (var kvp in Settings.SkinWindowSizes) {
				var skin = SkinManager.Skins.FirstOrDefault(s => s.Controller.SvgPath == kvp.Key);
				skin.WindowSize = kvp.Value;
			}

			BuildMenu();
			var activeSkin = SkinManager.Skins.FirstOrDefault(s => s.Controller.SvgPath == Settings.ActiveSkinPath);
			if (activeSkin != null && activeSkin.LoadResult == SvgController.LoadResult.Ok)
				ActivateSkin(activeSkin);

			Application.Idle += OnApplicationOnIdle;
			if (!_skipUpdateCheck)
				PerformUpdateCheck();
			else
				UpdateStatus("not checking for newer version", 100);
		}

		private void BuildMenu() {
			int numOk = 0, numSkins = 0, numFail = 0;
			tsmiController.DropDownItems.Clear();

			foreach (var skin in SkinManager.Skins) {
				switch (skin.LoadResult) {
				case SvgController.LoadResult.Ok:
					numOk++;
					goto nocontroller;
				case SvgController.LoadResult.NoController:
					nocontroller:
					numSkins++;
					var tsmi = new ToolStripMenuItem($"{skin.Controller.SkinName} ({skin.Controller.DeviceName})");
					tsmi.Enabled = skin.LoadResult == SvgController.LoadResult.Ok;
					tsmi.Click += (sender, args) => ActivateSkin(skin);
					tsmiController.DropDownItems.Add(tsmi);
					break;
				case SvgController.LoadResult.Fail:
					numFail++;
					break;
				}
			}

			string skinText = $"Loaded {numSkins} skins ({numOk} devices available)";
			if (numFail > 0)
				skinText += $" ({numFail} failed to load)";
			lblSkins.Text = skinText;

			// if device wasn't available, but now it might be, then reevaluate the active skin
			if (SkinManager.ActiveSkin == null) {
				var activeSkin = SkinManager.Skins.FirstOrDefault(s => s.Controller.SvgPath == Settings.ActiveSkinPath);
				if (activeSkin != null && activeSkin.LoadResult == SvgController.LoadResult.Ok)
					ActivateSkin(activeSkin);
			}
		}

		private void ActivateSkin(SkinEntry skin) {
			SkinManager.ActiveSkin = skin;
			this._controller = skin.Controller;
			// find desired window size
			if (skin.WindowSize != Size.Empty)
				this.Size = skin.WindowSize - glControl.Size + this.Size;
			else
				skin.WindowSize = glControl.Size;
			this._controller?.Render(glControl.Width, glControl.Height);
			this._controller?.Activate(Handle);
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


		private void OnResize(object sender, EventArgs e) {
			if (SkinManager.ActiveSkin != null) {
				SkinManager.ActiveSkin.WindowSize = glControl.Size;
				SkinManager.ActiveSkin.Controller?.Render(glControl.Width, glControl.Height);
			}
			GL.Viewport(0, 0, glControl.Width, glControl.Height);
		}

		private void setWindowSizeToolStripMenuItem_Click(object sender, EventArgs e) {
			var frm = new WindowSizePicker(glControl.Size) {
				Parent = this,
				StartPosition = FormStartPosition.CenterParent
			};
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
			PerformUpdateCheck(true);
		}

		private void UpdateStatus(string text, int progressBarValue) {
			if (InvokeRequired) {
				Invoke((Action<string, int>)UpdateStatus, text, progressBarValue);
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

		private void PerformUpdateCheck(bool msgBox = false) {
			var uc = new UpdateChecker();
			uc.AlreadyLatest += (o, e) => {
				if (msgBox) MessageBox.Show("You are already using the latest version available", "Already latest", MessageBoxButtons.OK, MessageBoxIcon.Information);
				UpdateStatus("already latest version", 100);
				Task.Delay(2000).ContinueWith(task => Invoke((Action)delegate { pbProgress.Visible = false; }));
			};
			uc.Connected += (o, e) => UpdateStatus("connected", 10);
			uc.DownloadProgressChanged += (o, e) => { /* care, xml is small anyway */
			};
			uc.UpdateCheckFailed += (o, e) => {
				UpdateStatus("update check failed", 100);
				if (msgBox) MessageBox.Show("Update check failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Task.Delay(2000).ContinueWith(task => Invoke((Action)delegate { pbProgress.Visible = false; }));
			};
			uc.UpdateAvailable += (o, e) => {
				UpdateStatus("update available", 100);

				var dr = MessageBox.Show($"An update to version {e.Version.ToString()} released on {e.ReleaseDate.ToShortDateString()} is available. Release notes: \r\n\r\n{e.ReleaseNotes}\r\n\r\nUpdate now?", "Update available", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
			wc.DownloadProgressChanged += (sender, args) => BeginInvoke((Action)delegate { UpdateStatus($"downloading, {args.ProgressPercentage * 95 / 100}%", args.ProgressPercentage * 95 / 100); });

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

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			Settings.Save();
		}

		private void tsmiMuniaSettings_Click(object sender, EventArgs e) {
			var dev = MuniaController.GetConfigInterface();
			if (dev == null) {
				MessageBox.Show("MUNIA config interface not found. This typically has one of three causes:\r\n  MUNIA isn't plugged in\r\n  MUNIA is currently in bootloader mode\r\n  Installed firmware doesn't implement this feature yet. Upgrade using the instructions at http://munia.io/fw",
					"Not possible", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			HidStream stream;
			if (dev.TryOpen(out stream)) {
				try {
					byte[] buff = new byte[9];
					buff[0] = 0;
					buff[1] = 0x47;  // CFG_CMD_READ
					stream.Write(buff, 0, 9);
					stream.Read(buff, 0, 9);
					var settings = new MuniaSettings();
					if (settings.Parse(buff)) {
						var dlg = new MuniaSettingsDialog(stream, settings);
						dlg.ShowDialog(this);
					}
					else throw new InvalidOperationException("Invalid settings container received from MUNIA device");
				}

				catch (Exception exc) {
					MessageBox.Show("An error occurred while retrieving information from the MUNIA device:\r\n\r\n" + exc,
						"Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void tsmiFirmware_Click(object sender, EventArgs e) {
			var frm = new BootloaderForm {
				Parent = this,
				StartPosition = FormStartPosition.CenterParent
			};
			frm.ShowDialog(this);
		}
	}

}