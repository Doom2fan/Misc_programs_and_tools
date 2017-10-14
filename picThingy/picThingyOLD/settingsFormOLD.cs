using System;
using System.Windows.Forms;
using picThingyOLD.Properties;

namespace picThingyOLD {
    public partial class settingsFormOLD : Form {
        Settings tmpOpt { get; set; }
        public settingsFormOLD () {
            tmpOpt = Settings.Default;
            InitializeComponent ();
        }

        private void buttonOK_Click (object sender, EventArgs e) {
            Program.Options.Save ();
            this.Close ();
        }

        private void buttonCancel_Click (object sender, EventArgs e) {
            Program.Options.Reload ();
            this.Close ();
        }

        private void settingsForm_FormClosing (object sender, FormClosingEventArgs e) {
            Console.WriteLine (sender);
        }

        private void buttonDataSave_Click (object sender, EventArgs e) {
            ImageDataList.Images.SaveDatabase (tmpOpt.DataFile);
        }

        private void buttonDataLoad_Click (object sender, EventArgs e) {
            ImageDataList.Images.LoadDatabase (tmpOpt.DataFile);
        }

        private void buttonDataPathBrowse_Click (object sender, EventArgs e) {
            if (openFileDialogJSON.ShowDialog () == DialogResult.OK)
                textBoxDataPath.Text = openFileDialogJSON.FileName;
        }
    }
}
