using picThingy.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for AddImageWindow.xaml
    /// </summary>
    public partial class AddImageWindow : Window {
        public ImageData ImgData { private get; set; } = null;

        public AddImageWindow () {
            InitializeComponent ();
        }

        private void InitializeWindow () {
            if (ImgData != null) {
                if (!ImageDataList.Images.Contains (ImgData))
                    throw new ArgumentException ("The ImageData passed to ImgData isn't in the image list.");

                textBoxPath.IsReadOnly = true;
                buttonBrowse.IsEnabled = false;
                buttonCancel.Content = "Discard";
                buttonAdd.Content = "Save";

                textBoxPath.Text = ImgData.path;
                textBoxDescription.Text = ImgData.description;
                foreach (string tag in ImgData.tags)
                    listBoxTags.Items.Add (tag);
                foreach (string source in ImgData.sources)
                    listBoxTags.Items.Add (source);
            } else {
                textBoxPath.IsReadOnly = false;
                buttonBrowse.IsEnabled = true;
                buttonCancel.Content = "Cancel";
                buttonAdd.Content = "Add";
            }
        }

        public new void Show () {
            InitializeWindow ();
            base.Show ();
        }
        public new bool? ShowDialog () {
            InitializeWindow ();
            return base.ShowDialog ();
        }

        private void buttonCancel_Click (object sender, RoutedEventArgs e) {
            this.Close ();
        }

        private void buttonAdd_Click (object sender, RoutedEventArgs e) {
            if (String.IsNullOrWhiteSpace (textBoxPath.Text)) {
                MessageBox.Show ("You must select an image to add.", "No image path", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } else if (!File.Exists (textBoxPath.Text)) {
                MessageBox.Show ("The specified file does not exist.", "Invalid path", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ImageData img = new ImageData ();
            img.path = textBoxPath.Text;

            using (var stream = new FileStream (img.path, FileMode.Open)) {
                try {
                    using (var bmp = System.Drawing.Image.FromStream (stream)) {
                        img.width = bmp.Width;
                        img.height = bmp.Height;
                    }
                } catch (ArgumentException ex) {
                    MessageBox.Show ("The specified file is not an image.", "Invalid file", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                stream.Seek (0, SeekOrigin.Begin);
                using (var hasher = new System.Security.Cryptography.SHA1Cng ()) {
                    img.sha1Hash = BitConverter.ToString (hasher.ComputeHash (stream)).Replace ("-", "");
                    hasher.Clear ();
                }

                stream.Seek (0, SeekOrigin.Begin);
                img.thumbnailMD5 = Thumbnails.Add (stream, img.path);
            }

            if (ImgData == null) {
                ImageData otherImg = null;
                for (int i = 0; i < ImageDataList.Images.Count; i++) {
                    otherImg = ImageDataList.Images [i];
                    if (otherImg == null) // Wat?
                        continue;

                    if (string.Equals (otherImg.path, img.path, StringComparison.CurrentCultureIgnoreCase)) {
                        MessageBox.Show ("There is already an entry with the specified path in the database.", "Duplicate image", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    } else if (string.Equals (otherImg.sha1Hash, img.sha1Hash, StringComparison.CurrentCultureIgnoreCase)) {
                        MessageBox.Show ("The specified image matches the SHA1 hash of another entry in the database.", "Duplicate image", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }

            img.description = textBoxDescription.Text;
            img.size = (new FileInfo (img.path)).Length;

            List<string> tags = new List<string> (), sources = new List<string> ();
            foreach (string tag in this.listBoxTags.Items) {
                if (tag.StartsWith ("source:")) // If the tag starts with "source:",
                    sources.Add (tag.Remove (0, 7)); // Remove the "source:" from the tag and add it to the sources list
                else // If not,
                    tags.Add (tag); // Add it to the tags list
            }
            img.sources = sources.ToArray (); sources.Clear (); sources = null;
            img.tags = tags.ToArray (); tags.Clear (); tags = null;

            if (ImgData != null) {
                for (int i = 0; i < ImageDataList.Images.Count; i++)
                    if (ImageDataList.Images [i] == this.ImgData)
                        ImageDataList.Images [i] = img;
            } else
                ImageDataList.Images.Add (img);

            this.Close ();
        }

        private void pathTextBox_TextChanged (object sender, TextChangedEventArgs e) {
            if (File.Exists (textBoxPath.Text)) {
                imagePreview.Source = null;
                var src = new BitmapImage ();
                src.BeginInit ();
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.UriSource = new Uri (textBoxPath.Text);
                src.EndInit ();
                imagePreview.Source = src;
            }
        }

        private void buttonBrowse_Click (object sender, RoutedEventArgs e) {
            object file = Dialogs.ShowOpenFileDialog ("Image files (*.jpg, *.jpeg, *.png, *.gif, *.bmp, *.tga, *.tif, *.tiff)|*.JPG;*.JPEG;*.PNG;*.GIF;*.BMP;*.TGA;*.TIF;*.TIFF|All files (*.*)|*.*", false);

            if (file != null && file.GetType () == typeof (string)) {
                if (File.Exists ((string) file))
                    textBoxPath.Text = (string) file;
                else
                    MessageBox.Show ("The specified file does not exist.", "Invalid path", MessageBoxButton.OK, MessageBoxImage.Error);
            } else if (file != null)
                MessageBox.Show ("Invalid image file selected.", "Invalid path", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void listBoxTags_KeyDown (object sender, KeyEventArgs e) {
            if (e.Key == Key.Delete && listBoxTags.SelectedIndex > -1 && listBoxTags.SelectedIndex < listBoxTags.Items.Count && listBoxTags.Items.Count > 0) {
                e.Handled = true;
                listBoxTags.Items.RemoveAt (listBoxTags.SelectedIndex);
            }
        }

        private void textBoxAddTag_KeyDown (object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Return) {
                e.Handled = true;
                string tag = textBoxAddTag.Text.Trim ();

                if (!String.IsNullOrWhiteSpace (tag)) {
                    if (!listBoxTags.Items.Contains (tag)) {
                        listBoxTags.Items.Add (tag);
                        textBoxAddTag.Text = "";
                    } else
                        MessageBox.Show ("The specified tag is already in the tag list.", "Invalid tag", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                } else
                    textBoxAddTag.Text = "";
            }
        }
    }
}
