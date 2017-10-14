using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace picThingyOLD {
    public partial class mainFormOLD : Form {
        private string searchQuery = "";

        public mainFormOLD () {
            InitializeComponent ();
            ImageDataList.Images.OnChange += ImageList_OnChange;

            ImageDataList.Images.LoadDatabase ();
        }

        private void ImageList_OnChange (object sender, EventArgs e) {
            if (Program.Options.SaveOnChange)
                ImageDataList.Images.SaveDatabase ();
            if (Program.Options.RefreshOnChange)
                this.SearchGo ();
        }

        private void buttonSearchGo_Click (object sender, EventArgs e) {
            this.searchQuery = this.textBoxSearch.Text;
            this.SearchGo ();
        }

        private void SearchGo () {
            toolStripStatusLabel.Text = "Searching images";
            string [] queryItems = searchQuery.Split (',');
            foreach (string query in queryItems) {
                if (query.StartsWith ("sha1:") && queryItems.Length > 1) {
                    MessageBox.Show ("Invalid search query string", "Invalid search", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripStatusLabel.Text = "Ready";
                    return;
                }
            }
            
            this.picListControl.ClearImages ();
            List<ImageData> images = new List<ImageData> ();
            if (String.IsNullOrWhiteSpace (this.searchQuery.Trim ())) {
                for (int i = 0; i < ImageDataList.Images.Count; i++)
                    images.Add (ImageDataList.Images [i]);
            } else {
                for (int i = 0; i < ImageDataList.Images.Count; i++) {
                    ImageData img = ImageDataList.Images [i];
                    bool add = true;
                    foreach (string query in queryItems) {
                        if ((query.StartsWith ("source:") && !img.sources.Contains (query.Remove (0, 7).Trim (), StringComparer.OrdinalIgnoreCase)) ||
                            (!img.tags.Contains (query.Trim (), StringComparer.OrdinalIgnoreCase))) {
                                add = false;
                                break;
                        }
                    }
                    if (add)
                        images.Add (img);
                }
            }
            
            toolStripStatusLabel.Text = this.picListControl.AddImages (images.ToArray ()).ToString ();
        }

        private void exitToolStripMenuItem_Click (object sender, EventArgs e) {
            Application.Exit ();
        }

        private void addImageToolStripMenuItem_Click (object sender, EventArgs e) {
            editImageFormOLD form = new editImageFormOLD ();
            form.ShowDialog ();
            form.Dispose ();
            this.SearchGo ();
        }

        private void saveDatabaseToolStripMenuItem_Click (object sender, EventArgs e) {
            ImageDataList.Images.SaveDatabase ();
        }

        private void textBoxSearch_KeyDown (object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Return)
                this.buttonSearchGo.PerformClick ();
        }

        private void listViewPics_ItemActivate (object sender, EventArgs e) {
            
        }

        private void settingsToolStripMenuItem_Click (object sender, EventArgs e) {
            settingsFormOLD f = new settingsFormOLD ();
            f.ShowDialog ();
        }
    }
}
