using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace picThingy.Utilities {
    public static class Dialogs {
        private static bool ShowDialog (FileDialog dialogType, string filter = "All files (*.*)|*.*", string defaultExt = "") {
            var dlg = dialogType;
            
            dlg.Filter = filter;
            dlg.DefaultExt = defaultExt;

            bool? dlgResult = dlg.ShowDialog ();
            if (dlgResult != null && dlgResult.Value)
                return true;
            else
                return false;

            
        }

        /// <summary>
        /// Opens an open file dialog.
        /// </summary>
        /// <param name="filter">The file filter</param>
        /// <param name="multiselect">Indicates whether the dialog allows users to select multiple files.</param>
        /// <param name="defaultExt">Specifies the default extension string to use to filter the list of files that are displayed.</param>
        /// <returns>Returns null if no files selected. Returns string if multiselect is false, string [] if true</returns>
        public static dynamic ShowOpenFileDialog (string filter = "All files (*.*)|*.*", bool multiselect = false, string defaultExt = "") {
            var dlg = new OpenFileDialog ();
            dlg.Multiselect = multiselect;

            bool dlgResult = ShowDialog (dlg, filter, defaultExt);
            dynamic result;
            if (dlgResult) {
                if (multiselect)
                    result = dlg.FileNames;
                else
                    result = dlg.FileName;
            } else
                result = null;
            
            return result;
        }

        /// <summary>
        /// Opens a save file dialog dialog.
        /// </summary>
        /// <param name="filter">The file filter</param>
        /// <param name="multiselect">Indicates whether the dialog allows users to select multiple files.</param>
        /// <param name="defaultExt">Specifies the default extension string to use to filter the list of files that are displayed.</param>
        /// <returns>Returns null if no files selected. Returns string if multiselect is false, string [] if true</returns>
        public static dynamic ShowSaveFileDialog (string filter = "All files (*.*)|*.*", bool multiselect = false, string defaultExt = "") {
            var dlg = new OpenFileDialog ();
            dlg.Multiselect = multiselect;

            bool dlgResult = ShowDialog (dlg, filter, defaultExt);
            dynamic result;
            if (dlgResult) {
                if (multiselect)
                    result = dlg.FileNames;
                else
                    result = dlg.FileName;
            } else
                result = null;
            
            return result;
        }
    }
}
