using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HidSharp;
using MUNIA.Controllers;
using MUNIA.Interop;
using MUNIA.Skins;
using OpenTK.Graphics.OpenGL;

namespace MUNIA.Forms {
	public partial class MainForm : Form {
		private double _timestamp;
		private int _frames;
		private readonly bool _skipUpdateCheck;
		private int _fps;

		private MainForm() {
			InitializeComponent();
		}

		public MainForm(bool skipUpdateCheck) : this() {
			glControl.Resize += OnResize;
			glControl.Paint += (sender, args) => Render();
			UsbNotification.DeviceArrival += OnDeviceArrival;
			UsbNotification.DeviceRemovalComplete += OnDeviceRemoval;
			//UsbNotification.SetFilter(G)
			_skipUpdateCheck = skipUpdateCheck;
		}

		private void MainForm_Shown(object sender, EventArgs e) {
			ConfigManager.Load();
			BuildMenu();
			ActivateConfig(ConfigManager.GetActiveController(), ConfigManager.ActiveSkin);

			Application.Idle += OnApplicationOnIdle;
			if (!_skipUpdateCheck)
				PerformUpdateCheck();
			else
				UpdateStatus("not checking for newer version", 100);
		}

		private Task _buildMenuTask;
		private async void ScheduleBuildMenu() {
			// buffers multiple BuildMenu calls
			if (_buildMenuTask == null) {
				_buildMenuTask = Task.Delay(300).ContinueWith(t => Invoke((Action)delegate {
					ConfigManager.LoadControllers();
					BuildMenu();
				}));
				await _buildMenuTask;
				_buildMenuTask = null;
			}
		}

		private void BuildMenu() {
			_buildMenuTask = null;
			Debug.WriteLine("Building menu");

			tsmiControllers.DropDownItems.Clear();

			foreach (var ctrlr in ConfigManager.Controllers) {
				var tsmiController = new ToolStripMenuItem(ctrlr.Name);

				foreach (var skin in ConfigManager.Skins.Where(s => s.Controllers.Contains(ctrlr.Type))) {
					var tsmiSkin = new ToolStripMenuItem($"{skin.Name}");
					tsmiSkin.Enabled = true;
					tsmiSkin.Click += (sender, args) => ActivateConfig(ctrlr, skin);
					tsmiController.DropDownItems.Add(tsmiSkin);
				}

				tsmiControllers.DropDownItems.Add(tsmiController);
			}

			string skinText = $"Loaded {ConfigManager.Skins.Count} skins ({ConfigManager.Controllers.Count} devices available)";
			int numFail = ConfigManager.Skins.Count(s => s.LoadResult != SkinLoadResult.Ok);
			if (numFail > 0)
				skinText += $" ({numFail} skins failed to load)";
			lblSkins.Text = skinText;
		}

		private void ActivateConfig(IController ctrlr, Skin skin) {
			if (skin?.LoadResult != SkinLoadResult.Ok) return;
			if (ctrlr == null) return;

			ConfigManager.ActiveSkin = skin;
			ConfigManager.SetActiveController(ctrlr);

			// find desired window size
			if (ConfigManager.WindowSizes.ContainsKey(skin)) {
				var wsz = ConfigManager.WindowSizes[skin];
				if (wsz != Size.Empty)
					this.Size = wsz - glControl.Size + this.Size;
				else
					ConfigManager.WindowSizes[skin] = glControl.Size;
			}

			UpdateController();
			Render();
		}

		private void glControl_Load(object sender, EventArgs e) {
			glControl.MakeCurrent();
			glControl.VSync = true;
		}

		private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
		private const int maxFPS = 60;
		long prevTicks = 0;
		private void OnApplicationOnIdle(object s, EventArgs a) {
			while (glControl.IsIdle) {
				glControl.MakeCurrent();
				_stopwatch.Restart();
				if (UpdateController() || Environment.TickCount - prevTicks > 200) {
					Render();
					prevTicks = Environment.TickCount; // Redraw at least every 200ms
				}
				Thread.Sleep((int)(Math.Max(1000f / maxFPS - _stopwatch.Elapsed.TotalMilliseconds, 0)));
			}
		}

		private bool UpdateController() {
			if (ConfigManager.ActiveSkin == null) return false;
			return ConfigManager.ActiveSkin.UpdateState(ConfigManager.GetActiveController());
		}

		private void UpdateRender() {
			if (UpdateController()) {
				Render();
			}
		}

		private void Render() {
			glControl.MakeCurrent();
			GL.ClearColor(Color.FromArgb(0, ConfigManager.BackgroundColor));
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, glControl.Width, glControl.Height, 0, 0.0, 4.0);

			ConfigManager.ActiveSkin?.Render(glControl.Width, glControl.Height);
			glControl.SwapBuffers();
		}


		private void OnResize(object sender, EventArgs e) {
			if (ConfigManager.ActiveSkin != null) {
				ConfigManager.WindowSizes[ConfigManager.ActiveSkin] = glControl.Size;
				ConfigManager.ActiveSkin?.Render(glControl.Width, glControl.Height);
			}
			GL.Viewport(0, 0, glControl.Width, glControl.Height);
			Render();
		}

		private void tsmiSetWindowSize_Click(object sender, EventArgs e) {
			var frm = new WindowSizePicker(glControl.Size) {
				StartPosition = FormStartPosition.CenterParent
			};
			if (frm.ShowDialog() == DialogResult.OK) {
				this.Size = frm.ChosenSize - glControl.Size + this.Size;
			}
		}

		private void tsmiAbout_Click(object sender, EventArgs e) {
			new AboutBox().Show(this);
		}

		private void OnDeviceArrival(object sender, UsbNotificationEventArgs args) {
			ScheduleBuildMenu();
			// see if this was our active controller and we can reactivate it
			var ac = ConfigManager.GetActiveController();
			if (string.Compare(ac?.DevicePath, args.Name, StringComparison.OrdinalIgnoreCase) == 0) {
				ac?.Activate();
			}
		}

		private void OnDeviceRemoval(object sender, UsbNotificationEventArgs args) {
			ScheduleBuildMenu();
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
				Task.Delay(2000).ContinueWith(task => {
					if (InvokeRequired && IsHandleCreated) Invoke((Action)delegate { pbProgress.Visible = false; });
				});
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
			ConfigManager.Save();
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

					var settings = new MUNIA.MuniaDeviceInfo();
					if (settings.Parse(buff)) {
						if (settings.IsLegacy) {
							var dlg = new MuniaSettingsDialog(stream, settings);
							dlg.ShowDialog(this);
						}
						else {
							var dlg = new MuniaSettingsDialog(stream, settings);
							dlg.ShowDialog(this);
						}
					}
					else throw new InvalidOperationException("Invalid settings container received from MUNIA device: " + string.Join(" ", buff.Select(x => x.ToString("X2"))));
				}

				catch (Exception exc) {
					MessageBox.Show("An error occurred while retrieving information from the MUNIA device:\r\n\r\n" + exc,
						"Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void tsmiFirmware_Click(object sender, EventArgs e) {
			var frm = new BootloaderForm {
				StartPosition = FormStartPosition.CenterParent
			};
			frm.ShowDialog(this);
		}

		private void tsmiMapArduinoDevicesClick(object sender, EventArgs args) {
			var frm = new ArduinoMapperForm(ConfigManager.ArduinoMapping) {
				StartPosition = FormStartPosition.CenterParent
			};
			if (frm.ShowDialog() == DialogResult.OK) {
				foreach (var e in frm.Mapping)
					ConfigManager.ArduinoMapping[e.Key] = e.Value;
				ConfigManager.LoadControllers();
				BuildMenu();
			}
		}

		private void tsmiSetLagCompensation(object sender, EventArgs args) {
			var frm = new DelayValuePicker(ConfigManager.Delay) {
				StartPosition = FormStartPosition.CenterParent
			};
			if (frm.ShowDialog() == DialogResult.OK) {
				ConfigManager.Delay = frm.ChosenDelay;
			}
		}

		private void glControl_MouseClick(object sender, MouseEventArgs args) {
			if (args.Button == MouseButtons.Right) {
				var dlg = new ColorDialog();
				if (dlg.ShowDialog() == DialogResult.OK)
					ConfigManager.BackgroundColor = dlg.Color;
			}
		}

	}

}