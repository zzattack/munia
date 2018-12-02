using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MUNIA.Controllers;
using MUNIA.Interop;
using MUNIA.Properties;
using MUNIA.Skinning;
using MUNIA.Util;
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

			tsmiBackgroundTransparent.Checked = ConfigManager.BackgroundColor.A == 0;
			BuildMenu();
			ActivateConfig(ConfigManager.GetActiveController(), ConfigManager.ActiveSkin);

			ConfigManager.ControllerMappings.ListChanged += (o, args) => ScheduleBuildMenu();

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
				_buildMenuTask = Task.Delay(300).ContinueWith(t => {
					if (IsHandleCreated && !IsDisposed) {
						try {
							BeginInvoke((Action)delegate {
								ConfigManager.LoadControllers();
								BuildMenu();
							});
						}
						catch { }
					}
				});
				await _buildMenuTask;
				_buildMenuTask = null;
			}
		}

		private void BuildMenu() {
			_buildMenuTask = null;
			Debug.WriteLine("Building menu");

			tsmiControllers.DropDownItems.Clear();
			var allControllers = ConfigManager.Controllers.ToList();

			foreach (ControllerType controllerType in Enum.GetValues(typeof(ControllerType))) {
				if (controllerType == ControllerType.None || controllerType == ControllerType.Unknown)
					continue;

				var controllers = new List<ToolStripMenuItem>();
				// first show the controllers which are available
				foreach (var ctrlr in allControllers.Where(c => c.Type == controllerType && c.IsAvailable).OrderBy(c => c.Name)) {
					var tsmiController = new ToolStripMenuItem(ctrlr.Name);

					var pref = new[] { typeof(SvgSkin), typeof(NintendoSpySkin), typeof(PadpyghtSkin) };
					foreach (var skinGroup in ConfigManager.Skins
						.Where(s => s.Controllers.Contains(ctrlr.Type))
						.OrderBy(s => s.Name)
						.GroupBy(s => s.GetType())
						.OrderBy(g => Array.IndexOf(pref, g.Key))) {

						foreach (var skin in skinGroup) {
							var tsmiSkin = new ToolStripMenuItem($"{skin.Name}");
							tsmiSkin.Enabled = true;
							tsmiSkin.Click += (sender, args) => ActivateConfig(ctrlr, skin);
							tsmiSkin.Image = GetSkinImage(skin);
							HookHoverEvents(tsmiSkin, skin);
							tsmiController.DropDownItems.Add(tsmiSkin);
						}
					}

					tsmiController.Image = GetControllerImage(ctrlr);
					controllers.Add(tsmiController);
				}

				if (controllers.Count > 1) {
					// make a menu item entry for this controller type
					var tsmiControllerType = new ToolStripMenuItem(controllerType.ToString());
					tsmiControllerType.Image = GetControllerImage(controllerType);
					foreach (var c in controllers)
						tsmiControllerType.DropDownItems.Add(c);
					tsmiControllers.DropDownItems.Add(tsmiControllerType);
				}
				else if (controllers.Count == 1) {
					// prefer the controller image over skin
					controllers[0].Image = GetControllerImage(controllerType);

					// skip the dropdown item with only a single entry
					tsmiControllers.DropDownItems.Add(controllers[0]);
				}
				else {
					// allow skin to be previewed
					var tsmiSkin = new ToolStripMenuItem(controllerType.ToString());
					var preview = tsmiSkin.DropDownItems.Add("No controllers - preview only");
					preview.Enabled = false;

					foreach (var skin in ConfigManager.Skins.Where(s => s.Controllers.Contains(controllerType))) {
						var skinPrev = tsmiSkin.DropDownItems.Add(skin.Name);
						skinPrev.Enabled = true;
						skinPrev.Click += (sender, args) => ActivateConfig(null, skin);
						skinPrev.Image = GetSkinImage(skin);
						HookHoverEvents(skinPrev, skin);
					}
				}
			}

			tsmiControllers.DropDownItems.Add(new ToolStripSeparator());
			var tsmiMapGeneric = tsmiSkinFolders.DropDownItems.Add("Add controller mapping");
			tsmiMapGeneric.Click += TsmiMapGenericOnClick;
			tsmiMapGeneric.Image = Resources.x360;
			tsmiControllers.DropDownItems.Add(tsmiMapGeneric);

			string skinText = $"Loaded {ConfigManager.Skins.Count} skins ({ConfigManager.Controllers.Count} devices available)";
			int numFail = ConfigManager.Skins.Count(s => s.LoadResult != SkinLoadResult.Ok);
			if (numFail > 0)
				skinText += $" ({numFail} skins failed to load)";
			lblSkins.Text = skinText;
		}

		#region Skin preview timer

		private System.Threading.Timer _previewDelayTimer;
		private PreviewParams _previewParams = new PreviewParams();
		struct PreviewParams {
			public ToolStripItem tsmi;
			public Skin skin;
		}
		private void HookHoverEvents(ToolStripItem tsmi, Skin skin) {
			if (_previewDelayTimer == null) {
				_previewDelayTimer = new System.Threading.Timer(CreateSkinPreview);
			}
			tsmi.MouseMove += (sender, args) => {
				_previewParams.tsmi = tsmi;
				_previewParams.skin = skin;
				_previewDelayTimer.Change(50, Timeout.Infinite); // slight delay
			};
			tsmi.MouseLeave += (sender, args) => EndSkinPreview();
		}
		#endregion

		private void CreateSkinPreview(object state) {
			BeginInvoke((Action)delegate {
				HoverSkinPreview(_previewParams.tsmi, _previewParams.skin);
			});
		}


		SkinPreviewWindow _activeSkinPreview;
		private bool _busyRendering;
		private void HoverSkinPreview(object sender, Skin skin) {
			if (skin == null || skin.Path == _activeSkinPreview?.Skin?.Path || _busyRendering)
				return;

			try {
				// create a clone of requested skin
				var clonedSkin = Skin.Clone(skin);
				if (clonedSkin.LoadResult != SkinLoadResult.Ok)
					return;

				_activeSkinPreview?.Skin?.Deactivate();
				_activeSkinPreview?.Hide();

				Point loc = this.PointToClient(Cursor.Position);
				if (sender is ToolStripMenuItem tsmi) {
					// make sure preview is at least to the right of the toolstrip item
					int r = menu.Left;
					int t = menu.Bottom;
					ToolStripItem item = tsmi;
					while (item != null) {
						if (item.Owner != menu)
							r += item.Bounds.Width;

						// add vertical offset
						var parent = item.OwnerItem as ToolStripMenuItem;
						int i = 0;
						while (parent != null && i < parent.DropDownItems.Count) {
							if (parent.DropDownItems[i] == item) break;
							t += parent.DropDownItems[i].Height;
							i++;
						}
						item = parent;
					}
					loc = new Point(r, t);
					loc.Offset(5, 5); // small margin
				}

				if (_activeSkinPreview == null) {
					_activeSkinPreview = new SkinPreviewWindow(clonedSkin);
					_activeSkinPreview.Location = loc;
					Controls.Add(_activeSkinPreview);
					_activeSkinPreview.BringToFront();
				}
				else {
					_activeSkinPreview.Location = loc;
					_activeSkinPreview.ChangeSkin(clonedSkin);
				}

				ControllerState state = new ControllerState();
				skin.GetNumberOfElements(out int numButtons, out int numAxes);
				state.Buttons.EnsureSize(numButtons);
				state.Axes.EnsureSize(numAxes);
				_activeSkinPreview.RenderSkin(state);

				_activeSkinPreview.Show();
			}
			finally {
				_busyRendering = false;
			}
		}
		private void EndSkinPreview() {
			if (_previewDelayTimer != null) {
				_previewDelayTimer.Change(Timeout.Infinite, Timeout.Infinite); // disable
			}
			if (_activeSkinPreview != null) {
				_activeSkinPreview?.Hide();
				_activeSkinPreview.Dispose();
				_activeSkinPreview = null;
			}
		}

		private void tsmiControllers_DropDownClosed(object sender, EventArgs e) {
			EndSkinPreview();
		}

		private static Image GetControllerImage(IController ctrlr) {
			if (ctrlr is ArduinoController) return Resources.arduino;
			if (ctrlr is MappedController m) return GetControllerImage(m.SourceController);
			else return GetControllerImage(ctrlr.Type);
		}

		private static Image GetControllerImage(ControllerType ctrlrType) {
			switch (ctrlrType) {
			case ControllerType.SNES:
				return Resources.snes;
			case ControllerType.N64:
				return Resources.n64;
			case ControllerType.NGC:
				return Resources.ngc;
			case ControllerType.PS2:
				return Resources.ps;
			case ControllerType.Generic:
			case ControllerType.XInput:
				return Resources.generic;
			}
			return null;
		}

		private static Image GetSkinImage(Skin skin) {
			if (skin is SvgSkin) return Resources.svg;
			else if (skin is NintendoSpySkin) return Resources.nspy;
			else if (skin is PadpyghtSkin) return Resources.padpy;
			else return null;
		}

		private void ActivateConfig(IController ctrlr, Skin skin) {
			EndSkinPreview();
			if (skin?.LoadResult != SkinLoadResult.Ok) return;

			skin.Activate();
			// apply remap if it was previously selected
			if (skin is SvgSkin svg && ConfigManager.SelectedRemaps[svg] is ColorRemap rmp) {
				rmp.ApplyToSkin(svg);
			}
			else if (skin is NintendoSpySkin nspySkin && ConfigManager.SelectedNSpyBackgrounds.ContainsKey(nspySkin)) {
				nspySkin.SelectBackground(ConfigManager.SelectedNSpyBackgrounds[nspySkin]);
			}
			
			ConfigManager.ActiveSkin = skin;
			ConfigManager.SetActiveController(ctrlr);
			if (ctrlr != null) {
				skin.UpdateState(ctrlr.GetState()); // initial read
				lblStatus.Text = $"Controller {ctrlr.Name} activated with skin {skin.Name}";
			}

			// find desired window size
			if (ConfigManager.WindowSizes.ContainsKey(skin)) {
				var wsz = ConfigManager.WindowSizes[skin];
				if (wsz != Size.Empty)
					this.Size = wsz - glControl.Size + this.Size;
				else
					ConfigManager.WindowSizes[skin] = glControl.Size;
			}

			ConfigManager.Save();
			UpdateController();
			Render();
		}

		private void OnControllerDetached(object sender, EventArgs e) {
			if (sender is IController c && ConfigManager.GetActiveController() == c) {
				c.Deactivate();
				lblStatus.Text = $"Controller {c.Name ?? ""} detached";
			}
		}

		private void glControl_Load(object sender, EventArgs e) {
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
			var activeController = ConfigManager.GetActiveController();
			if (activeController == null) return false;
			return ConfigManager.ActiveSkin.UpdateState(activeController);
		}


		private void Render() {
			if (ConfigManager.ActiveSkin == null) return;

			glControl.MakeCurrent();
			GL.ClearColor(Color.FromArgb(0, ConfigManager.BackgroundColor));
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, glControl.Width, glControl.Height, 0, 0.0, 4.0);

			ConfigManager.ActiveSkin.Render(glControl.Width, glControl.Height);
			glControl.SwapBuffers();
		}


		private void OnResize(object sender, EventArgs e) {
			if (ConfigManager.ActiveSkin != null) {
				ConfigManager.WindowSizes[ConfigManager.ActiveSkin] = glControl.Size;
				ConfigManager.ActiveSkin?.Render(glControl.Width, glControl.Height, true);
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
				ConfigManager.Save();
			}
		}
		private void tsmiSkinFolders_Click(object sender, EventArgs e) {
			(new SkinFoldersForm(ConfigManager.SkinFolders)).ShowDialog(this);

			ConfigManager.Save();
			ReloadAll();
		}

		private void ReloadAll() {
			ConfigManager.Skins.Clear();
			ConfigManager.SkinFolders.Clear();
			ConfigManager.WindowSizes.Clear();
			ConfigManager.SelectedRemaps.Clear();
			ConfigManager.AvailableRemaps.Clear();
			ConfigManager.Load();
			BuildMenu();
			ActivateConfig(ConfigManager.GetActiveController(), ConfigManager.ActiveSkin);
		}

		private void tsmiAbout_Click(object sender, EventArgs e) {
			new AboutBox().Show(this);
		}

		private void OnDeviceArrival(object sender, UsbNotificationEventArgs args) {
			ScheduleBuildMenu();
			// see if this was our active controller and we can reactivate it
			if (!string.IsNullOrEmpty(args.Name))
				ReactivateController(args.Name);
		}

		private void ReactivateController(string devicePath) {
			var ac = ConfigManager.GetActiveController();
			// use string contains to also match mapped controllers
			bool reactivated = false;
			if (ac != null) {
				if (ac.DevicePath.ToLowerInvariant().Contains(devicePath.ToLowerInvariant())) {
					reactivated = ac.Activate();
				}
				else if (devicePath.Contains("IG_") && (
							ac.Type == ControllerType.XInput ||
							(ac is MappedController mc && mc.SourceController is XInputController))) {
					// xinput device, maybe it can be activated
					reactivated = ac.Activate();
				}
			}
			if (reactivated) {
				var ctrlr = ConfigManager.GetActiveController();
				var skin = ConfigManager.ActiveSkin;
				lblStatus.Text = $"Controller {ctrlr.Name} reactivated with skin {skin.Name}";
			}
		}

		private void OnDeviceRemoval(object sender, UsbNotificationEventArgs args) {
			ScheduleBuildMenu();
			// if active device is no longer available, deactivate the controller for it
			var c = ConfigManager.GetActiveController();
			if (c != null && !c.IsAvailable) c.Deactivate();
		}

		#region update checking/performing

		private void tsmiCheckUpdates_Click(object sender, EventArgs e) {
			status.Visible = true;
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
					if (InvokeRequired && IsHandleCreated) BeginInvoke((Action)delegate { pbProgress.Visible = false; });
				});
			};
			uc.Connected += (o, e) => UpdateStatus("connected", 10);
			uc.DownloadProgressChanged += (o, e) => { /* care, xml is small anyway */
			};
			uc.UpdateCheckFailed += (o, e) => {
				UpdateStatus("update check failed", 100);
				if (msgBox) MessageBox.Show("Update check failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Task.Delay(2000).ContinueWith(task => BeginInvoke((Action)delegate { pbProgress.Visible = false; }));
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
			foreach (var ctrlr in ConfigManager.Controllers.ToList())
				ctrlr.Dispose();
			ConfigManager.Save();
		}

		private void tsmiMuniaSettings_Click(object sender, EventArgs e) {
			var devs = MuniaController.GetConfigInterfaces().ToList();

			if (!devs.Any()) {
				MessageBox.Show("No config interfaces not found. This typically has one of three causes:\r\n  MUNIA isn't plugged in\r\n  MUNIA is currently in bootloader mode\r\n  Installed firmware doesn't implement this feature yet. Upgrade using the instructions at http://munia.io/fw",
					"Not possible", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			ConfigInterface intf = null;
			if (devs.Count() == 1) intf = devs.First();
			else {
				var pickerDlg = new DevicePicker(devs);
				if (pickerDlg.ShowDialog() == DialogResult.OK)
					intf = pickerDlg.ChosenDevice;
			}
			try {
				if (intf is MuniaConfigInterface mnci) {
					var dlg = new MuniaSettingsDialog(mnci);
					dlg.ShowDialog(this);
				}
				else if (intf is MusiaConfigInterface msci) {
					var dlg = new MusiaSettingsDialog(msci);
					dlg.ShowDialog(this);
				}
			}
			catch (InvalidOperationException exc) {
				MessageBox.Show(this, "An error occurred while retrieving information from the MUNIA device:\r\n\r\n",
					"Invalid error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (TimeoutException exc) {
				MessageBox.Show(this, "A timeout occurred while retrieving information from the MUNIA device:\r\n\r\n",
					"Timeout", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception exc) {
				MessageBox.Show(this, "An unknown error occurred while retrieving information from the MUNIA device:\r\n\r\n" + exc.InnerException,
					"Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void tsmiFirmware_Click(object sender, EventArgs e) {
			var frm = new BootloaderForm {
				StartPosition = FormStartPosition.CenterParent
			};
			frm.ShowDialog(this);
		}

		private void tsmiMapArduinoDevices_Click(object sender, EventArgs args) {
			var frm = new ArduinoMapperForm(ConfigManager.ArduinoMapping) {
				StartPosition = FormStartPosition.CenterParent
			};
			if (frm.ShowDialog(this) == DialogResult.OK) {
				foreach (var e in frm.Mapping)
					ConfigManager.ArduinoMapping[e.Key] = e.Value;
				ConfigManager.LoadControllers();
				BuildMenu();
				ConfigManager.Save();
			}
		}

		private void tsmiSetLagCompensation_Click(object sender, EventArgs args) {
			var frm = new DelayValuePicker(ConfigManager.Delay) {
				StartPosition = FormStartPosition.CenterParent
			};
			if (frm.ShowDialog(this) == DialogResult.OK) {
				ConfigManager.Delay = frm.ChosenDelay;
				ConfigManager.Save();
			}
		}

		private void glControl_MouseClick(object sender, MouseEventArgs args) {
			if (args.Button == MouseButtons.Right) {
				popup.Show(glControl, args.Location);
			}
		}

		private void tsmiBackgroundChange_Click(object sender, EventArgs e) {
			var dlg = new ColorDialog2 { Color = ConfigManager.BackgroundColor };
			var colorBackup = ConfigManager.BackgroundColor;
			dlg.ColorChanged += (o, eventArgs) => {
				// retain transparency
				ConfigManager.BackgroundColor = Color.FromArgb(colorBackup.A, dlg.Color);
				Render();
			};
			if (dlg.ShowDialog(this) != DialogResult.OK) {
				ConfigManager.BackgroundColor = colorBackup;
			}
		}

		private void tsmiBackgroundTransparent_Click(object sender, EventArgs e) {
			// flip transparency
			ConfigManager.BackgroundColor = Color.FromArgb(255 - ConfigManager.BackgroundColor.A, ConfigManager.BackgroundColor);
			tsmiBackgroundTransparent.Checked = ConfigManager.BackgroundColor.A == 0;
			Render();
		}

		private void tsmiManageThemes_Click(object sender, EventArgs e) {
			if (ConfigManager.ActiveSkin is SvgSkin svg) {
				var managerForm = new SkinRemapManagerForm(svg, ConfigManager.SelectedRemaps[svg],
					ConfigManager.AvailableRemaps[svg.Path]);

				managerForm.SelectedRemapChanged += (o, args) =>
					SelectRemap(managerForm.SelectedRemap);

				managerForm.ShowDialog();
				ConfigManager.Save();
				SelectRemap(managerForm.SelectedRemap);
			}
		}

		private void TsmiMapGenericOnClick(object sender, EventArgs e) {
			new ControllerMapperForm().ShowDialog();
		}

		private void testControllerToolStripMenuItem_Click(object sender, EventArgs e) {
			new GamepadTesterForm().ShowDialog();
		}

		private void popup_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
			tsmiManageThemes.Enabled = ConfigManager.ActiveSkin is SvgSkin;

			// populate the available themes list
			tsmiApplySkinTheme.DropDownItems.Clear();
			tsmiApplyCustomTheme.DropDownItems.Clear();
			if (ConfigManager.ActiveSkin is SvgSkin svg) {
				var selectedRemap = ConfigManager.SelectedRemaps[svg];

				IList<ColorRemap> remaps = svg.EmbeddedRemaps;
				tsmiApplySkinTheme.Enabled = remaps.Any();
				foreach (var remap in remaps) {
					var tsmiSkin = new ToolStripMenuItem(remap.Name, null, (_, __) => SelectRemap(remap));
					// put a checkmark in front if this is the selected remap
					tsmiSkin.Checked = remap.Equals(selectedRemap);
					tsmiSkin.Click += (o, args) => svg.ApplyRemap(remap);
					tsmiApplySkinTheme.DropDownItems.Add(tsmiSkin);
				}

				remaps = ConfigManager.AvailableRemaps[svg.Path];
				tsmiApplyCustomTheme.Enabled = remaps.Any(r => !r.IsSkinDefault);
				foreach (var remap in ConfigManager.AvailableRemaps[svg.Path].Where(r => !r.IsSkinDefault)) {
					var tsmiSkin = new ToolStripMenuItem(remap.Name, null, (_, __) => SelectRemap(remap));
					// put a checkmark in front if this is the selected remap
					tsmiSkin.Checked = remap.Equals(selectedRemap);
					tsmiSkin.Click += (o, args) => svg.ApplyRemap(remap);
					tsmiApplyCustomTheme.DropDownItems.Add(tsmiSkin);
				}
				tsmiBackground.Visible = true;
			}
			else {
				tsmiApplyCustomTheme.Enabled = false;
				tsmiBackground.Visible = false;
			}

			if (ConfigManager.ActiveSkin is NintendoSpySkin nspy) {
				tsmiBackground.Visible = false;
				tsmiBackgroundNSpy.Visible = true;
				tsmiBackgroundNSpy.DropDownItems.Clear();
				foreach (var bg in nspy.Backgrounds) {
					ToolStripMenuItem item = new ToolStripMenuItem(bg.Key);
					item.Click += (o, args) => nspy.SelectBackground(bg.Key);
					item.Checked = bg.Value == nspy.SelectedBackground;
					tsmiBackgroundNSpy.DropDownItems.Add(item);
				}
			}
			else {
				tsmiBackgroundNSpy.Visible = false;
			}
		}

		private void SelectRemap(ColorRemap remap) {
			if (ConfigManager.ActiveSkin is SvgSkin svg) {
				svg.ApplyRemap(remap);
				ConfigManager.SelectedRemaps[svg] = remap;
				// force redraw of the base
				ConfigManager.ActiveSkin?.Render(glControl.Width, glControl.Height, true);
				Render();
			}
		}
	}

}