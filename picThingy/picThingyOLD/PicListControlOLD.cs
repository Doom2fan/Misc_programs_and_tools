using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace picThingyOLD {
    public partial class PicListControlOLD : UserControl {
        private class CustomPicBox : PictureBox {
        }
        private Dictionary<ImageData, CustomPicBox> images = new Dictionary<ImageData, CustomPicBox> ();
        private Size _thumbSize = new Size (100, 100);
        public bool ResizeThumbnails = true;
        private BindingSource bs = new BindingSource ();

        public Size ThumbSize {
            get { return this._thumbSize; }
            set {
                this._thumbSize = value;
                this.SuspendLayout ();
                ArrangePics ();
                this.ResumeLayout ();
            }
        }

        public PicListControlOLD () {
            InitializeComponent ();
            bs.DataSource = typeof (Properties.Settings);
            bs.CurrentItemChanged += BS_CurrentItemChanged;
            bs.Add (Program.Options);
        }

        private void BS_CurrentItemChanged (object sender, EventArgs e) {
            if (Program.Options.ThumbSize != _thumbSize.Width)
                PerformResize ();
        }

        private void PicListControl_SizeChanged (object sender, EventArgs e) {
            PerformResize ();
        }

        private void PerformResize () {
            if (this.Size == Size.Empty || this.Visible == false)
                return;
            this.SuspendLayout ();
            int newWH = (int) Program.Options.ThumbSize;
            if (Program.Options.ThumbSize <= 0) {
                newWH = (this.Width - 8 * 6) / 5;
                if (newWH > 250) newWH = 250;
            }
            this._thumbSize = new Size (newWH, newWH);
            this.AutoScrollMinSize = this.Size;
            ArrangePics ();
            this.ResumeLayout ();
            this.PerformLayout ();
        }
        
        public void ArrangePics () {
            this.HorizontalScroll.Value = 0;
            this.VerticalScroll.Value = 0;
            int rowItemMax = this.Size.Width / this._thumbSize.Width,
                columnSpacing = this.Size.Width % this._thumbSize.Width / (rowItemMax + 1),
                columnSpacing2 = columnSpacing + this._thumbSize.Width,
                rowSpacing = this._thumbSize.Height + 8; // This might seem like a lot of vars, but making 100% sure these aren't recalculated for each pictureBox should help performance
            int i = 0;
            foreach (CustomPicBox pic in images.Values) {
                pic.Size = this._thumbSize;
                pic.Location = new Point (
                    columnSpacing + columnSpacing2 * (i % rowItemMax), // X pos
                    rowSpacing * (i / rowItemMax)); // Y pos
                i++;
            }
        }

        /// <summary>
        /// Clears the control's image list
        /// </summary>
        public void ClearImages () {
            this.images.Clear ();
            this.Controls.Clear ();
        }

        /// <summary>
        /// Adds an image to the control's image list
        /// </summary>
        /// <param name="img">The image to be added</param>
        /// <param name="arrange">If true, the ArragePics method is called</param>
        /// <returns>Returns true if the image was successfully added</returns>
        public bool AddImage (ImageData img, bool arrange = true) {
            if (this.images.ContainsKey (img) || !File.Exists (img.path))
                return false;

            CustomPicBox pic = new CustomPicBox ();
            ContextMenuStrip strip = new ContextMenuStrip ();
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            pic.Size = this._thumbSize;
            pic.MouseClick += Pic_MouseClick;
            pic.Name = img.path;
            strip.Items.Add (new ToolStripButton ("Edit", null, Pic_Edit, img.path));
            strip.Items.Add (new ToolStripButton ("Delete", null, Pic_Delete, img.path));
            pic.ContextMenuStrip = strip;

            if (pic.Image != null) // If the current pictureBox image is not null,
                pic.Image.Dispose (); // Dispose of it

            pic.WaitOnLoad = false;
            pic.LoadAsync (img.path);

            this.images.Add (img, pic);
            this.Controls.Add (pic);

            if (arrange)
                ArrangePics ();

            return true;
        }

        private void Pic_Edit (object sender, EventArgs e) {
            string path = ((ToolStripButton) sender).Name;
            editImageFormOLD form = new editImageFormOLD ();
            form.EditMode = true;
            ImageData img = null;
            for (int i = 0; i < ImageDataList.Images.Count; i++) {
                ImageData id = ImageDataList.Images [i];
                if (id.path == path) {
                    img = id;
                    break;
                }
            }
            if (img != null) {
                form.data = img;
                form.ShowDialog ();
            }
            form.Dispose ();
        }

        private void Pic_Delete (object sender, EventArgs e) {
            string path = ((ToolStripButton) sender).Name;
            for (int i = 0; i < ImageDataList.Images.Count; i++)
                if (ImageDataList.Images [i].path == path)
                    ImageDataList.Images.RemoveAt (i);
        }

        private void Pic_MouseClick (object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
                System.Diagnostics.Process.Start (((CustomPicBox) sender).Name);
        }

        /// <summary>
        /// Adds an array of images to the control's image list
        /// </summary>
        /// <param name="imgList">The images to be added</param>
        /// <param name="arrange">If true, the ArragePics method is called</param>
        /// <param name="pauseLayout">If true, it'll call the control's Suspend/ResumeLayout methods</param>
        /// <returns>Returns the amount of images successfully added</returns>
        public int AddImages (ImageData [] imgList, bool arrange = true, bool pauseLayout = true) {
            int i = 0;

            if (pauseLayout)
                this.SuspendLayout ();

            foreach (ImageData img in imgList)
                if (AddImage (img, false))
                    i++;

            if (pauseLayout)
                this.ResumeLayout ();
            if (arrange && i > 0)
                ArrangePics ();

            return i;
        }

        private void PicListControl_Load (object sender, EventArgs e) {
            PicListControl_SizeChanged (null, null);
        }
    }
}
