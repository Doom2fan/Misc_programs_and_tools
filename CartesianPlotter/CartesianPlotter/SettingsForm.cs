using CartesianPlotter.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CartesianPlotter {
    public partial class SettingsForm : Form {
        Settings oldOpt;
        public SettingsForm () {
            oldOpt = Program.Options;
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
    }
}
