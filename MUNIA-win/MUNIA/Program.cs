
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace MUNIA {

    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        public static void Main() {
            Application.Run(new MainForm());

        }
    }
}
