using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MUNIA.Forms;
using MUNIA.Util;

namespace MUNIA {

	internal static class Program {
		private static MainForm _mf;
		internal static OptionSet Options;
		private static string _cmdLine;

		[STAThread]
		private static void Main(string[] args) {
			try {
				AppDomain.CurrentDomain.UnhandledException += TempHandler;
				Application.ThreadException += TempHandler;
				TaskScheduler.UnobservedTaskException += TempHandler;
				
				InnerMain(args);
			}
			catch (Exception exc) {
				MessageBox.Show("Fatal: program failed to start. Exception; " + exc);
			}
		}
		private static void TempHandler(object sender, UnobservedTaskExceptionEventArgs e) {
			MessageBox.Show(e.Exception.ToString());
		}
		private static void TempHandler(object sender, ThreadExceptionEventArgs e) {
			MessageBox.Show(e.Exception.ToString());
		}
		private static void TempHandler(object sender, UnhandledExceptionEventArgs e) {
			MessageBox.Show(e.ExceptionObject.ToString());
		}
		private static void InnerMain(string[] args) {
			_cmdLine = string.Join(" ", args);
			bool showHelp = false;
			bool skipUpdateCheck = false;
			Options = new OptionSet {
				{"h|help", "Show this short help text", v => showHelp = true},
				{"k|killpid=", "Kill calling (old) process (to be used by updater)", KillDanglingProcess},
				{"c|cleanupdate=", "Delete (old) executable (to be used by updater)", RemoveOldExecutable},
				{"s|skip-update-check", "Skip update check)", v => skipUpdateCheck = true},
			};
			Options.Parse(args);
			if (showHelp) {
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write("Usage: ");
				Console.WriteLine("");
				var sb = new StringBuilder();
				var sw = new StringWriter(sb);
				Options.WriteOptionDescriptions(sw);
				Console.WriteLine(sb.ToString());
				return;
			}
			
#if DEBUG
			// redirect console output to parent process;
			// must be before any calls to Console.WriteLine()
			AttachConsole(ATTACH_PARENT_PROCESS);
#endif
			
			AppDomain.CurrentDomain.UnhandledException -= TempHandler;
			Application.ThreadException -= TempHandler;
			TaskScheduler.UnobservedTaskException -= TempHandler;

			Application.SetCompatibleTextRenderingDefault(false);
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

			Application.EnableVisualStyles();

			_mf = null;

			try {
				// HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
				// see if the auto-updater invoked the program
				_mf = new MainForm(skipUpdateCheck);
				Application.Run(_mf);
			}
			catch (Exception exc) {
				AskBugReport(exc);
				ConfigManager.Save();
			}
		}

		#region Updater cleanup

		private static void KillDanglingProcess(string pid) {
			try {
				var proc = Process.GetProcessById(int.Parse(pid));
				proc.CloseMainWindow();
				if (!proc.WaitForExit(100)) proc.Kill();
			}
			catch (FormatException) { }
			catch (ArgumentException) { }
		}

		private static void RemoveOldExecutable(string path) {
			try {
				Stopwatch sw = Stopwatch.StartNew();
				bool success = false;
				while (sw.ElapsedMilliseconds < 10000) {
					try {
						File.Delete(path);
						success = true;
						break;
					}
					catch (UnauthorizedAccessException) {
						Thread.Sleep(10); // keep trying for a while
					}
				}
				if (!success)
					MessageBox.Show($"Tried to remove old file {path} but failed. Try to delete it manually.");
			}
			catch (FormatException) { }
			catch (ArgumentException) { }
			catch (Win32Exception) { }
		}

		#endregion

		#region Exception and bugreport handling

		private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs args) {
			args.SetObserved();
		}
		private static void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e) {
			if (e.ExceptionObject is AggregateException)
				return;

			AskBugReport(e.ExceptionObject as Exception);
			ConfigManager.Save();
		}

		private static void AskBugReport(Exception exc) {
			var form = new SubmitBugForm();
			form.Email = ConfigManager.Email;
			if (form.ShowDialog() == DialogResult.OK) {
				//if (!string.IsNullOrWhiteSpace(form.Email))
				//	Settings.Default.email = form.Email;
				SubmitBugReport(form.Email, exc);
			}
		}
		
		private static void SubmitBugReport(string email, Exception exc) {
			try {
				const string url = UpdateChecker.UpdateCheckHost + "tool/report_bug";
				WebClient wc = new WebClient();
				wc.Proxy = null;
				var data = new NameValueCollection();
				data.Set("program_version", typeof(Program).Assembly.GetName().Version.ToString());
				data.Set("exception", exc?.ToString() ?? "");
				if (ConfigManager.ActiveSkin!= null) {
					data.Set("skin_name", ConfigManager.ActiveSkin.Name);
				}
				data.Set("command_line", _cmdLine);
				data.Set("email", email);

				wc.UploadValuesCompleted += (o, args) => {
					if (args.Cancelled || args.Error != null)
						BugReportFailed();
					else
						MessageBox.Show("Bug report sent successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
				};

				wc.UploadValuesAsync(new Uri(url), "POST", data);
			}
			catch {
				BugReportFailed();
			}
		}

		private static void BugReportFailed() {
			MessageBox.Show("Submitting bug report failed. Please send a manual bug report to frank@zzattack.org including your skin and error log",
				"Bug report submission failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		#endregion

		#region P/Invoke declarations

		[DllImport("USER32.DLL")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("kernel32.dll")]
		private static extern bool AttachConsole(int dwProcessId);

		private const int ATTACH_PARENT_PROCESS = -1;

		#endregion
	}
}
