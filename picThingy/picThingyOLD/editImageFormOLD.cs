using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace picThingyOLD {
    public partial class editImageFormOLD : Form {
        private ImageData _image = null;
        public bool EditMode = false;
        public ImageData data {
            set {
                this.textBoxPath.Text = value.path;
                this.textBoxDescription.Text = value.description;
                this.listBoxTags.Items.Clear ();
                this.listBoxTags.Items.AddRange (value.tags);
                this.listBoxTags.Items.AddRange (value.sources);
                this._image = value;
            }
        }

        public editImageFormOLD () {
            InitializeComponent ();
        }

        private void buttonCancel_Click (object sender, EventArgs e) {
            this.Close ();
        }

        private void buttonAdd_Click (object sender, EventArgs e) {
            if (String.IsNullOrWhiteSpace (textBoxPath.Text)) {
                MessageBox.Show ("The path textbox must not be empty!", "Invalid path", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show a message
                return; // Return
            }
            string imgPath = Path.GetFullPath (textBoxPath.Text);
            if (!File.Exists (imgPath)) { // If the specified path does not exist,
                MessageBox.Show ("The specified image does not exist!", "Invalid path", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show a message
                return; // Return
            }

            ImageData image = new ImageData ();
            image.path = imgPath; // Set the image's path
            SHA1 hasher = null;
            FileStream stream = null;
            string sha1Hash = "";
            FileInfo fileInf = new FileInfo (image.path); // Set the file info
            try {
                hasher = new SHA1Cng (); // Create a new hasher
                stream = new FileStream (image.path, FileMode.Open); // Create a new stream
                sha1Hash = BitConverter.ToString (hasher.ComputeHash (stream)).Replace ("-", ""); // Compute the image's hash
                hasher.Clear (); // Clear and dispose of the hasher
                stream.Close (); // Close and dispose of the stream
            } catch (Exception ex) {
                MessageBox.Show (ex.Message + "\nInnerException:" + ex.InnerException + "\nSource:" + ex.Source + "\nTargetSite: " + ex.TargetSite, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            image.sha1Hash = sha1Hash; // Set the SHA1 hash
            if (!EditMode) {
                for (int i = 0; i < ImageDataList.Images.Count; i++) {
                    ImageData otherImg = ImageDataList.Images [i];
                    if (otherImg.path == image.path) {
                        MessageBox.Show ("There is already an entry with the specified path in the database", "Duplicate image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    } else if (otherImg.sha1Hash == image.sha1Hash) {
                        MessageBox.Show ("The specified image matches the SHA1 hash of another entry in the database", "Duplicate image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            image.description = textBoxDescription.Text; // Set the description
            image.size = fileInf.Length; // Set the size in bytes
            int w = 0, h = 0;
            Image img = null;
            try {
                img = Image.FromFile (image.path); // Load the image
                w = img.Width; h = img.Height; // Set the width and height
                img.Dispose (); // Dispose of the image
            } catch (Exception ex) {
                MessageBox.Show (ex.Message + "\nInnerException:" + ex.InnerException + "\nSource:" + ex.Source + "\nTargetSite: " + ex.TargetSite, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            image.width = w; // Set the width
            image.height = h; // Set the height
            List<string> tags = new List<string> (), sources = new List<string> ();
            foreach (string tag in this.listBoxTags.Items) {
                if (tag.StartsWith ("source:")) // If the tag starts with "source:",
                    sources.Add (tag.Remove (0, 7)); // Remove the "source:" from the tag and add it to the sources list
                else // If not,
                    tags.Add (tag); // Add it to the tags list
            }
            image.sources = sources.ToArray (); // Set the sources
            image.tags = tags.ToArray (); // Set the tags

            if (EditMode) {
                for (int i = 0; i < ImageDataList.Images.Count; i++)
                    if (ImageDataList.Images [i] == this._image || ImageDataList.Images [i].path == this.textBoxPath.Text)
                        ImageDataList.Images [i] = image;
            } else
                ImageDataList.Images.Add (image);
            this.Close ();
        }

        private void textBoxAddTag_Click (object sender, EventArgs e) {
            listBoxTags.Items.Add (textBoxAddTag.Text);
            textBoxAddTag.Text = "";
        }

        private void listBoxTags_KeyDown (object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete && listBoxTags.SelectedIndex > -1 && listBoxTags.SelectedIndex < listBoxTags.Items.Count && listBoxTags.Items.Count > 0) {
                listBoxTags.Items.RemoveAt (listBoxTags.SelectedIndex);
                e.SuppressKeyPress = true;
            }
        }

        private void textBoxAddTag_KeyDown (object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Return) {
                e.SuppressKeyPress = true;
                if (String.IsNullOrWhiteSpace (textBoxAddTag.Text.Trim ()) && !listBoxTags.Items.Contains (textBoxAddTag.Text.Trim ())) {
                    listBoxTags.Items.Add (textBoxAddTag.Text.Trim ());
                    textBoxAddTag.Text = "";
                } else if (String.IsNullOrWhiteSpace (textBoxAddTag.Text.Trim ())) {
                } else
                    MessageBox.Show ("The specified tag is already in the tag list.", "Invalid tag", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonBrowse_Click (object sender, EventArgs e) {
            if (openFileDialog.ShowDialog () == DialogResult.OK) {
                textBoxPath.Text = openFileDialog.FileName;
            }
        }

        private void textBoxPath_TextChanged (object sender, EventArgs e) {
            if (File.Exists (textBoxPath.Text)) {
                if (pictureBox.Image != null) // If the current pictureBox image is not null,
                    pictureBox.Image.Dispose (); // Dispose of it

                Stream stream = new FileStream (textBoxPath.Text, FileMode.Open); // Create a new stream
                pictureBox.Image = Image.FromStream (stream); // Read the image data from the stream
                stream.Close (); // Close and dispose of the stream
            }
        }

        private void addImageForm_FormClosing (object sender, FormClosingEventArgs e) {
            if (pictureBox.Image != null)
                pictureBox.Image.Dispose ();
        }

        private void editImageForm_Load (object sender, EventArgs e) {
            if (EditMode) {
                textBoxPath.ReadOnly = true;
                buttonBrowse.Enabled = false;
                buttonCancel.Text = "Discard";
                buttonAdd.Text = "Save";
            } else {
                textBoxPath.ReadOnly = false;
                buttonBrowse.Enabled = true;
                buttonCancel.Text = "Cancel";
                buttonAdd.Text = "Add";
            }
        }

        private void textBoxAddTag_Enter (object sender, EventArgs e) { this.AcceptButton = null; }
        private void textBoxAddTag_Leave (object sender, EventArgs e) { this.AcceptButton = this.buttonAdd; }
    }
}
