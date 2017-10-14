using picThingyOLD.Properties;
using System;
using System.Windows.Forms;

namespace picThingyOLD {
    public static class Program {
        //public static OptionsClass Options = new OptionsClass ();
        internal static Settings Options = Settings.Default;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main () {
            //Options = OptionsClass.Load ();
            Options.Reload ();
            if (String.IsNullOrWhiteSpace (Options.DataFile))
                Options.DataFile = Constants.DefaultDataFile;
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);
            mainFormOLD main = new mainFormOLD ();
            Application.Run (main);
        }
    }
}
