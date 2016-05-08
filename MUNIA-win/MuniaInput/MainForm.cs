using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MUNIA.Controllers;
using SharpLib.Hid;
using SharpLib.Win32;

namespace MuniaInput {
    public partial class MainForm : Form {
        MuniaController _activeController;
        
        public delegate void OnHidEventDelegate(object aSender, Event aHidEvent);

        public MainForm() {
            InitializeComponent();

            comboBox1.Items.AddRange(MuniaController.ListDevices().ToArray());
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e) {
            Register(comboBox1.SelectedItem as MuniaController);
        }

        private void Register(MuniaController c) {
            _activeController = c;
            c.Activate();
            c.StateUpdated += ControllerStateUpdated;
        }

        private void ControllerStateUpdated(object sender, EventArgs args) {
            if (InvokeRequired) Invoke((Action<object, EventArgs>)ControllerStateUpdated, sender, args);
            else {
                textBox1.AppendText("Buttons: ");
                textBox1.AppendText(string.Join("", _activeController.Buttons.Select(x => x ? "Y" : "N")));
                textBox1.AppendText("\r\nAxes:");
                textBox1.AppendText(string.Join("  ", _activeController.Axes));
                textBox1.AppendText("\r\n");
            }
        }
		
    }

}
