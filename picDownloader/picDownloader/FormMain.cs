using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace picDownloader {
    public partial class FormMain : Form {
        public FormMain () {
            InitializeComponent ();
        }

        private void buttonBrowse_Click (object sender, EventArgs e) {
            if (folderBrowserDialog.ShowDialog () != DialogResult.OK)
                return;
            string newPath = folderBrowserDialog.SelectedPath;
            if (Directory.Exists (newPath))
                textBoxPath.Text = newPath;
        }

        int pos = 0;
        private List<string> links = new List<string> ();
        private void buttonGo_Click (object sender, EventArgs e) {
            string path = textBoxPath.Text;
            if (!Directory.Exists (path)) {
                MessageBox.Show ("Download folder does not exist!", "Invalid output folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            pos = 0;
            links.Clear ();
            links.AddRange (textBoxLinks.Lines);
            if (links.Count <= 0)
                return;

            panel.Enabled = false;
            using (WebClient client = new WebClient ()) {
                int p = pos++;
                toolStripStatusLabel.Text = "Downloading " + links [p];
                client.DownloadProgressChanged += Client_DownloadProgressChanged;
                client.DownloadFileCompleted += Client_DownloadFileCompleted;
                client.DownloadFileAsync (new Uri (links [p]), Path.Combine (path, Path.GetFileName (links [p])));
            }
        }

        private void Client_DownloadProgressChanged (object sender, DownloadProgressChangedEventArgs e) {
            toolStripStatusLabel.Text = String.Format ("Downloading {0}. {1}% ({2}/{3} MB)", links [pos - 1], e.ProgressPercentage, (float) e.BytesReceived / 1024 / 1024, (float) e.TotalBytesToReceive / 1024 / 1024);
            toolStripProgressBar.Value = e.ProgressPercentage;
        }

        private void Client_DownloadFileCompleted (object sender, AsyncCompletedEventArgs e) {
            if (pos >= links.Count) {
                pos = 0;
                links.Clear ();
                toolStripStatusLabel.Text = "Ready";
                panel.Enabled = true;
            } else {
                using (WebClient client = new WebClient ()) {
                    int p = pos++;
                    client.DownloadProgressChanged += Client_DownloadProgressChanged;
                    client.DownloadFileCompleted += Client_DownloadFileCompleted;
                    client.DownloadFileAsync (new Uri (links [p]), Path.Combine (textBoxPath.Text, Path.GetFileName (links [p])));
                }
            }
        }
    }
}
