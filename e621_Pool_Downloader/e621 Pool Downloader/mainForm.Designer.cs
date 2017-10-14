namespace e621_Pool_Downloader {
    partial class mainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent () {
            this.components = new System.ComponentModel.Container();
            this.textBoxPID = new System.Windows.Forms.TextBox();
            this.labelPID = new System.Windows.Forms.Label();
            this.labelPath = new System.Windows.Forms.Label();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.labelFileMask = new System.Windows.Forms.Label();
            this.textBoxNameMask = new System.Windows.Forms.TextBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.labelPreview = new System.Windows.Forms.Label();
            this.checkBoxStartOne = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel = new System.Windows.Forms.Panel();
            this.checkBoxDownToSubfolder = new System.Windows.Forms.CheckBox();
            this.buttonBrowsePath = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1.SuspendLayout();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxPID
            // 
            this.textBoxPID.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPID.Location = new System.Drawing.Point(12, 25);
            this.textBoxPID.Name = "textBoxPID";
            this.textBoxPID.Size = new System.Drawing.Size(96, 20);
            this.textBoxPID.TabIndex = 0;
            // 
            // labelPID
            // 
            this.labelPID.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPID.AutoSize = true;
            this.labelPID.Location = new System.Drawing.Point(12, 9);
            this.labelPID.Name = "labelPID";
            this.labelPID.Size = new System.Drawing.Size(45, 13);
            this.labelPID.TabIndex = 1;
            this.labelPID.Text = "Pool ID:";
            // 
            // labelPath
            // 
            this.labelPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(12, 48);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(82, 13);
            this.labelPath.TabIndex = 2;
            this.labelPath.Text = "Download path:";
            // 
            // textBoxPath
            // 
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.Location = new System.Drawing.Point(12, 64);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(399, 20);
            this.textBoxPath.TabIndex = 3;
            // 
            // labelFileMask
            // 
            this.labelFileMask.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileMask.AutoSize = true;
            this.labelFileMask.Location = new System.Drawing.Point(12, 87);
            this.labelFileMask.Name = "labelFileMask";
            this.labelFileMask.Size = new System.Drawing.Size(80, 13);
            this.labelFileMask.TabIndex = 4;
            this.labelFileMask.Text = "Filename mask:";
            // 
            // textBoxNameMask
            // 
            this.textBoxNameMask.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNameMask.Location = new System.Drawing.Point(12, 103);
            this.textBoxNameMask.Name = "textBoxNameMask";
            this.textBoxNameMask.Size = new System.Drawing.Size(457, 20);
            this.textBoxNameMask.TabIndex = 5;
            this.textBoxNameMask.TextChanged += new System.EventHandler(this.textBoxNameMask_TextChanged);
            // 
            // buttonGo
            // 
            this.buttonGo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGo.Location = new System.Drawing.Point(12, 175);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(460, 23);
            this.buttonGo.TabIndex = 6;
            this.buttonGo.Text = "Download";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // labelPreview
            // 
            this.labelPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPreview.AutoSize = true;
            this.labelPreview.Location = new System.Drawing.Point(12, 126);
            this.labelPreview.Name = "labelPreview";
            this.labelPreview.Size = new System.Drawing.Size(51, 13);
            this.labelPreview.TabIndex = 7;
            this.labelPreview.Text = "Preview: ";
            // 
            // checkBoxStartOne
            // 
            this.checkBoxStartOne.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxStartOne.AutoSize = true;
            this.checkBoxStartOne.Location = new System.Drawing.Point(12, 152);
            this.checkBoxStartOne.Name = "checkBoxStartOne";
            this.checkBoxStartOne.Size = new System.Drawing.Size(81, 17);
            this.checkBoxStartOne.TabIndex = 9;
            this.checkBoxStartOne.Text = "Start at one";
            this.toolTip1.SetToolTip(this.checkBoxStartOne, "Starts numbering the images at one instead of zero");
            this.checkBoxStartOne.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.toolStripStatusLabel});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 203);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(481, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 10;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(150, 16);
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Ready";
            // 
            // panel
            // 
            this.panel.Controls.Add(this.checkBoxDownToSubfolder);
            this.panel.Controls.Add(this.buttonBrowsePath);
            this.panel.Controls.Add(this.checkBoxStartOne);
            this.panel.Controls.Add(this.labelPreview);
            this.panel.Controls.Add(this.buttonGo);
            this.panel.Controls.Add(this.textBoxNameMask);
            this.panel.Controls.Add(this.labelFileMask);
            this.panel.Controls.Add(this.textBoxPath);
            this.panel.Controls.Add(this.labelPath);
            this.panel.Controls.Add(this.labelPID);
            this.panel.Controls.Add(this.textBoxPID);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(481, 225);
            this.panel.TabIndex = 11;
            // 
            // checkBoxDownToSubfolder
            // 
            this.checkBoxDownToSubfolder.AutoSize = true;
            this.checkBoxDownToSubfolder.Checked = true;
            this.checkBoxDownToSubfolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDownToSubfolder.Location = new System.Drawing.Point(99, 152);
            this.checkBoxDownToSubfolder.Name = "checkBoxDownToSubfolder";
            this.checkBoxDownToSubfolder.Size = new System.Drawing.Size(131, 17);
            this.checkBoxDownToSubfolder.TabIndex = 10;
            this.checkBoxDownToSubfolder.Text = "Download in subfolder";
            this.toolTip1.SetToolTip(this.checkBoxDownToSubfolder, "Downloads the pool to a subfolder in the specified path with the pool\'s name");
            this.checkBoxDownToSubfolder.UseVisualStyleBackColor = true;
            // 
            // buttonBrowsePath
            // 
            this.buttonBrowsePath.Location = new System.Drawing.Point(417, 64);
            this.buttonBrowsePath.Name = "buttonBrowsePath";
            this.buttonBrowsePath.Size = new System.Drawing.Size(52, 20);
            this.buttonBrowsePath.TabIndex = 0;
            this.buttonBrowsePath.Text = "Browse";
            this.buttonBrowsePath.UseVisualStyleBackColor = true;
            this.buttonBrowsePath.Click += new System.EventHandler(this.buttonBrowsePath_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Select a folder to save the pool\'s images in";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 225);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel);
            this.MinimumSize = new System.Drawing.Size(497, 264);
            this.Name = "mainForm";
            this.Text = "e621 Pool Downloader";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPID;
        private System.Windows.Forms.Label labelPID;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Label labelFileMask;
        private System.Windows.Forms.TextBox textBoxNameMask;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.Label labelPreview;
        private System.Windows.Forms.CheckBox checkBoxStartOne;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button buttonBrowsePath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBoxDownToSubfolder;
    }
}

