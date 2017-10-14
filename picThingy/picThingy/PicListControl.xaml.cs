using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Reflection;
using Draw = System.Drawing;
using System.Text;

namespace picThingy {
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class PicListControl : UserControl {
        private Dictionary<ImageData, Image> images = new Dictionary<ImageData, Image> ();
        private Size _thumbSize = new Size (100, 100);
        public bool ResizeThumbnails = true;

        public Size ThumbSize {
            get { return this._thumbSize; }
            set {
                this._thumbSize = value;
                // ArrangePics ();
            }
        }

        public PicListControl () {
            InitializeComponent ();
            _thumbSize.Width = _thumbSize.Height = Program.Options.ThumbSize;
            Program.Options.PropertyChanged += Options_PropertyChanged;
            ImageDataList.Images.OnChange += Images_OnChange;
        }

        private void Images_OnChange (object sender, ImageDataChangeEventArgs e) {
            ImageData [] keys = new ImageData [images.Count];
            Image [] values = new Image [images.Count];
            int i = 0;
            foreach (KeyValuePair<ImageData, Image> kvp in images) {
                keys [i] = kvp.Key;
                values [i] = kvp.Value;
                i++;
            }

            for (i = 0; i < keys.Length; i++) {
                KeyValuePair<ImageData, Image> kvp = new KeyValuePair<ImageData, Image> (keys [i], values [i]);
                ImageData newData;
                if ((newData = ImageDataList.Images.FromPath (kvp.Key.path)) != null) {
                    StringBuilder tags = new StringBuilder ();

                    foreach (string tag in newData.tags) {
                        if (tags.Length > 0)
                            tags.Append (", ");

                        tags.Append (tag);
                    }

                    kvp.Value.ToolTip = String.Format ("Path: {0}\nDimensions: {1}x{2}\nSize: {3} MB\nTags: {4}", newData.path, newData.width, newData.height, newData.size / 1024.0f / 1024.0f, tags);

                    if (newData != kvp.Key) {
                        images.Add (newData, kvp.Value);
                        images.Remove (kvp.Key);
                    }
                } else
                    kvp.Value.ToolTip = String.Format ("REMOVED IMAGE\nPath: {0}", kvp.Key.path);
            }
        }

        private void Options_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (Program.Options.ThumbSize != _thumbSize.Width)
                PerformResize ();
        }

        private void PicListControl_SizeChanged (object sender, SizeChangedEventArgs e) {
            PerformResize ();
        }

        private void PerformResize () {
            if (this.panel.RenderSize == Size.Empty || this.Visibility != Visibility.Visible)
                return;
            double newWH = Program.Options.ThumbSize;

            if (Program.Options.ThumbSize <= 0) {
                newWH = (this.panel.RenderSize.Width - 8 * 6) / 5;
                if (newWH > Program.Options.ThumbMaxDynSize)
                    newWH = Program.Options.ThumbMaxDynSize;
            }

            this._thumbSize = new Size (newWH, newWH);
            ArrangePics ();
        }

        public void ArrangePics () {
            double rowItemMax = Math.Floor (this.panel.RenderSize.Width / this._thumbSize.Width),
                columnSpacing = Math.Ceiling ((this.panel.RenderSize.Width % this._thumbSize.Width) / (rowItemMax + 1)),
                columnSpacing2 = Math.Ceiling (columnSpacing + this._thumbSize.Width),
                rowSpacing = this._thumbSize.Height + 8; // This might seem like a lot of vars, but making 100% sure these aren't recalculated for each pictureBox should help performance
            int i = 0;

            foreach (Image pic in images.Values) {
                pic.HorizontalAlignment = HorizontalAlignment.Left; // Alignment
                pic.VerticalAlignment = VerticalAlignment.Top;

                pic.Width = this._thumbSize.Width; // Size
                pic.Height = this._thumbSize.Height;

                pic.Margin = new Thickness ( // Position
                    columnSpacing + columnSpacing2 * (i % rowItemMax), // X pos
                    rowSpacing * (int) (i / rowItemMax), // Y pos
                    0.0, 0.0);
                i++;
            }
        }

        /// <summary>
        /// Clears the control's image list
        /// </summary>
        public void ClearImages () {
            this.images.Clear ();
            this.panel.Children.Clear ();
        }

        /// <summary>
        /// Adds an image to the control's image list
        /// </summary>
        /// <param name="img">The image to be added</param>
        /// <param name="arrange">If true, the ArrangePics method is called</param>
        /// <returns>Returns true if the image was successfully added</returns>
        public bool AddImage (ImageData img, bool arrange = true) {
            if (this.images.ContainsKey (img) || !File.Exists (img.path))
                return false;

            Image pic = new Image ();
            pic.Stretch = System.Windows.Media.Stretch.Uniform;
            pic.HorizontalAlignment = HorizontalAlignment.Left;
            pic.VerticalAlignment = VerticalAlignment.Top;
            pic.Height = this._thumbSize.Height;
            pic.Width = this._thumbSize.Width;
            pic.Tag = img.path;

            StringBuilder tags = new StringBuilder ();

            foreach (string tag in img.tags) {
                if (tags.Length > 0)
                    tags.Append (", ");

                tags.Append (tag);
            }

            pic.ToolTip = String.Format ("Path: {0}\nDimensions: {1}x{2}\nSize: {3} MB\nTags: {4}", img.path, img.width, img.height, (double) img.size / 1024.0 / 1024.0, tags.ToString ());

            pic.MouseLeftButtonDown += Pic_MouseLeftButtonDown;
            ContextMenu cMenu = new ContextMenu ();
            MenuItem itemEdit = new MenuItem (), itemDelete = new MenuItem ();
            itemEdit.Header = "Edit"; itemEdit.Click += Pic_Edit; itemEdit.Tag = img.path;
            itemDelete.Header = "Delete"; itemDelete.Click += Pic_Delete; itemDelete.Tag = img.path;
            cMenu.Items.Add (itemEdit);
            cMenu.Items.Add (itemDelete);
            pic.ContextMenu = cMenu;

            string thumbnail;

            if (img.thumbnailMD5 == null || !File.Exists (thumbnail = Thumbnails.GetThumbnailPath (img.thumbnailMD5))) {
                string md5 = Thumbnails.KeyFromPath (img.path);

                if (md5 == null || !File.Exists (thumbnail = Thumbnails.GetThumbnailPath (md5)))
                    thumbnail = Thumbnails.GetThumbnailPath (Thumbnails.Add (img.path));
            }
            
            BitmapImage image = new BitmapImage ();
            image.BeginInit ();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri (Path.GetFullPath (thumbnail));
            image.EndInit ();
            pic.Source = image;

            this.images.Add (img, pic);
            this.panel.Children.Add (pic);

            if (arrange)
                ArrangePics ();

            return true;
        }

        private void Pic_MouseLeftButtonDown (object sender, MouseButtonEventArgs e) {
            System.Diagnostics.Process.Start ((string) ((Image) sender).Tag);
        }

        private void Pic_Edit (object sender, EventArgs e) {
            string path = (string) (((MenuItem) sender).Tag);
            AddImageWindow window = new AddImageWindow ();
            ImageData img = ImageDataList.Images.FromPath (path);

            if (img != null) {
                window.ImgData = img;
                window.ShowDialog ();
            }

            window = null;
        }

        private void Pic_Delete (object sender, EventArgs e) {
            string path = (string) (((MenuItem) sender).Tag);
            ImageDataList.Images.RemoveAll (i => String.Equals (i.path, path, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Adds an array of images to the control's image list
        /// </summary>
        /// <param name="imgList">The images to be added</param>
        /// <param name="arrange">If true, the ArrangePics method is called</param>
        /// <param name="pauseLayout">If true, it'll call the control's Suspend/ResumeLayout methods</param>
        /// <returns>Returns the amount of images successfully added</returns>
        public int AddImages (ImageData [] imgList, bool arrange = true) {
            int i = 0;

            foreach (ImageData img in imgList)
                i += (AddImage (img, false) ? 1 : 0);

            if (arrange && i > 0)
                ArrangePics ();

            return i;
        }
    }
}
