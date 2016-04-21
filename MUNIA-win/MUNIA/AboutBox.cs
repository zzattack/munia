using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace MUNIA {
	partial class AboutBox : Form {
		public AboutBox() {
			InitializeComponent();
			Text = $"About {AssemblyTitle}";
			labelProductName.Text = AssemblyProduct;
			labelVersion.Text = $"Version {AssemblyVersion}";
			labelCopyright.Text = AssemblyCopyright;
			labelCompanyName.Text = AssemblyCompany;

			textBoxDescription.SelectedText = "MUNIA input viewer\r\n\r\n";
			textBoxDescription.InsertLink("munia.io", "http://munia.io");
			textBoxDescription.SelectedText = "\r\n\r\nProgram by Frank Razenberg - ";
			textBoxDescription.InsertLink("frank@zzattack.org", "mailto:frank@zzattack.org");
			textBoxDescription.SelectedText = "\r\nSkins by dutchj - ";
			textBoxDescription.InsertLink("twitter.com/ldutchjl", "http://twitter.com/ldutchjl");
		}

		#region Assembly Attribute Accessors

		public string AssemblyTitle {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0) {
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (titleAttribute.Title != "") {
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion {
			get {
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public string AssemblyDescription {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public string AssemblyProduct {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public string AssemblyCopyright {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public string AssemblyCompany {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}
		#endregion

		private void okButton_Click(object sender, EventArgs e) {
			Close();
		}

		private void textBoxDescription_LinkClicked(object sender, LinkClickedEventArgs e) {
			try {
				System.Diagnostics.Process.Start(e.LinkText.IndexOf("#") > 0 ? e.LinkText.Substring(e.LinkText.IndexOf("#")+1) : e.LinkText);
			}
			catch (Win32Exception) { }
		}
	}
}
