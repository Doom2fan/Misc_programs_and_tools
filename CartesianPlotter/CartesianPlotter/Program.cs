using CartesianPlotter.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CartesianPlotter {
    static class Program {
        public static Properties.Settings Options = new Properties.Settings ();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main () {
            Options.Reload ();
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);
            Application.Run (new FormMain ());
        }
    }
}
