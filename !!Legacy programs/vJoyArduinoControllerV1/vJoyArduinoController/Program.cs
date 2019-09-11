using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace vJoyArduinoController {
    static class Program {
        internal static FormMain form;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main () {
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);
            form = new FormMain ();
            Application.Run (form);
        }
    }
}
