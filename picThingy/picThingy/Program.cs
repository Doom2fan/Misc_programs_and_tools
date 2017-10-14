using picThingy.Properties;
using System;

namespace picThingy {
    public static class Program {
        internal static Settings Options = Settings.Default;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main () {
            Options.Reload ();
            if (String.IsNullOrWhiteSpace (Options.DataFile))
                Options.DataFile = Constants.DefaultDataFile;
            if (String.IsNullOrWhiteSpace (Options.ThumbsPath))
                Options.ThumbsPath = Constants.DefaultThumbsPath;

            var app = new App ();
            app.InitializeComponent ();
            app.Run ();
        }
    }
}
