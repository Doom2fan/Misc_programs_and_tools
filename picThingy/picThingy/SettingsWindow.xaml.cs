using picThingy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace picThingy {
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window {
        public SettingsWindow () {
            InitializeComponent ();
        }

        private void buttonOK_Click (object sender, RoutedEventArgs e) {
            Program.Options.Save ();
            this.Close ();
        }

        private void buttonCancel_Click (object sender, RoutedEventArgs e) {
            Program.Options.Reload ();
            this.Close ();
        }

        private void buttonBrowseDataFile_Click (object sender, RoutedEventArgs e) {
            object file = Dialogs.ShowOpenFileDialog ("JSON files (*.json)|*.JSON|All files (*.*)|*.*", false, "*.json");

            if (file != null && file.GetType () == typeof (string)) {
                if (System.IO.File.Exists ((string) file))
                    textBoxDBPath.Text = (string) file;
                else
                    MessageBox.Show ("The specified database file does not exist", "Invalid path", MessageBoxButton.OK, MessageBoxImage.Error);
            } else if (file != null)
                MessageBox.Show ("Invalid data file selected.", "Invalid selection", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void buttonLoadDatabase_Click (object sender, RoutedEventArgs e) {
            ImageDataList.Images.LoadDatabase (textBoxDBPath.Text);
        }

        private void buttonSaveDatabase_Click (object sender, RoutedEventArgs e) {
            ImageDataList.Images.SaveDatabase (textBoxDBPath.Text);
        }
    }
}
