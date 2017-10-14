namespace picThingyOLD {
    partial class settingsFormOLD {
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
            this.components = new System.ComponentModel.Container ();
            this.buttonOK = new System.Windows.Forms.Button ();
            this.buttonCancel = new System.Windows.Forms.Button ();
            this.tabControl = new System.Windows.Forms.TabControl ();
            this.tabPageGeneral = new System.Windows.Forms.TabPage ();
            this.buttonDataLoad = new System.Windows.Forms.Button ();
            this.buttonDataSave = new System.Windows.Forms.Button ();
            this.buttonDataPathBrowse = new System.Windows.Forms.Button ();
            this.textBoxDataPath = new System.Windows.Forms.TextBox ();
            this.labelDataPath = new System.Windows.Forms.Label ();
            this.checkBoxSaveChange = new System.Windows.Forms.CheckBox ();
            this.checkBoxRefreshChange = new System.Windows.Forms.CheckBox ();
            this.tabPagePagesOpt = new System.Windows.Forms.TabPage ();
            this.numericUpDownThumbSize = new System.Windows.Forms.NumericUpDown ();
            this.labelThumbSize = new System.Windows.Forms.Label ();
            this.labelPageMax = new System.Windows.Forms.Label ();
            this.numericUpDownPageMax = new System.Windows.Forms.NumericUpDown ();
            this.toolTip = new System.Windows.Forms.ToolTip (this.components);
            this.openFileDialogJSON = new System.Windows.Forms.OpenFileDialog ();
            this.tabControl.SuspendLayout ();
            this.tabPageGeneral.SuspendLayout ();
            this.tabPagePagesOpt.SuspendLayout ();
            ((System.ComponentModel.ISupportInitialize) (this.numericUpDownThumbSize)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize) (this.numericUpDownPageMax)).BeginInit ();
            this.SuspendLayout ();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point (416, 326);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size (75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler (this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point (497, 326);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size (75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler (this.buttonCancel_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add (this.tabPageGeneral);
            this.tabControl.Controls.Add (this.tabPagePagesOpt);
            this.tabControl.Location = new System.Drawing.Point (12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size (560, 308);
            this.tabControl.TabIndex = 4;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add (this.buttonDataLoad);
            this.tabPageGeneral.Controls.Add (this.buttonDataSave);
            this.tabPageGeneral.Controls.Add (this.buttonDataPathBrowse);
            this.tabPageGeneral.Controls.Add (this.textBoxDataPath);
            this.tabPageGeneral.Controls.Add (this.labelDataPath);
            this.tabPageGeneral.Controls.Add (this.checkBoxSaveChange);
            this.tabPageGeneral.Controls.Add (this.checkBoxRefreshChange);
            this.tabPageGeneral.Location = new System.Drawing.Point (4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding (3);
            this.tabPageGeneral.Size = new System.Drawing.Size (552, 282);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // buttonDataLoad
            // 
            this.buttonDataLoad.Location = new System.Drawing.Point (105, 91);
            this.buttonDataLoad.Name = "buttonDataLoad";
            this.buttonDataLoad.Size = new System.Drawing.Size (93, 20);
            this.buttonDataLoad.TabIndex = 8;
            this.buttonDataLoad.Text = "Load Database";
            this.buttonDataLoad.UseVisualStyleBackColor = true;
            this.buttonDataLoad.Click += new System.EventHandler (this.buttonDataLoad_Click);
            // 
            // buttonDataSave
            // 
            this.buttonDataSave.Location = new System.Drawing.Point (6, 91);
            this.buttonDataSave.Name = "buttonDataSave";
            this.buttonDataSave.Size = new System.Drawing.Size (93, 20);
            this.buttonDataSave.TabIndex = 7;
            this.buttonDataSave.Text = "Save Database";
            this.buttonDataSave.UseVisualStyleBackColor = true;
            this.buttonDataSave.Click += new System.EventHandler (this.buttonDataSave_Click);
            // 
            // buttonDataPathBrowse
            // 
            this.buttonDataPathBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonDataPathBrowse.Location = new System.Drawing.Point (478, 65);
            this.buttonDataPathBrowse.Name = "buttonDataPathBrowse";
            this.buttonDataPathBrowse.Size = new System.Drawing.Size (68, 20);
            this.buttonDataPathBrowse.TabIndex = 6;
            this.buttonDataPathBrowse.Text = "Browse";
            this.toolTip.SetToolTip (this.buttonDataPathBrowse, "Browse for the database\'s path");
            this.buttonDataPathBrowse.UseVisualStyleBackColor = true;
            this.buttonDataPathBrowse.Click += new System.EventHandler (this.buttonDataPathBrowse_Click);
            // 
            // textBoxDataPath
            // 
            this.textBoxDataPath.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDataPath.DataBindings.Add (new System.Windows.Forms.Binding ("Text", global::picThingyOLD.Properties.Settings.Default, "DataFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxDataPath.Location = new System.Drawing.Point (6, 65);
            this.textBoxDataPath.Name = "textBoxDataPath";
            this.textBoxDataPath.Size = new System.Drawing.Size (466, 20);
            this.textBoxDataPath.TabIndex = 5;
            this.textBoxDataPath.Text = global::picThingyOLD.Properties.Settings.Default.DataFile;
            this.toolTip.SetToolTip (this.textBoxDataPath, "The path to the JSON file the images database is stored in");
            // 
            // labelDataPath
            // 
            this.labelDataPath.AutoSize = true;
            this.labelDataPath.Location = new System.Drawing.Point (3, 49);
            this.labelDataPath.Name = "labelDataPath";
            this.labelDataPath.Size = new System.Drawing.Size (96, 13);
            this.labelDataPath.TabIndex = 4;
            this.labelDataPath.Text = "Database file path:";
            this.toolTip.SetToolTip (this.labelDataPath, "The path to the JSON file the images database is stored in");
            // 
            // checkBoxSaveChange
            // 
            this.checkBoxSaveChange.AutoSize = true;
            this.checkBoxSaveChange.Location = new System.Drawing.Point (6, 6);
            this.checkBoxSaveChange.Name = "checkBoxSaveChange";
            this.checkBoxSaveChange.Size = new System.Drawing.Size (152, 17);
            this.checkBoxSaveChange.TabIndex = 2;
            this.checkBoxSaveChange.Text = "Save database on change";
            this.toolTip.SetToolTip (this.checkBoxSaveChange, "Save the database every time a post is added or edited");
            this.checkBoxSaveChange.UseVisualStyleBackColor = true;
            // 
            // checkBoxRefreshChange
            // 
            this.checkBoxRefreshChange.AutoSize = true;
            this.checkBoxRefreshChange.Checked = global::picThingyOLD.Properties.Settings.Default.RefreshOnChange;
            this.checkBoxRefreshChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRefreshChange.DataBindings.Add (new System.Windows.Forms.Binding ("Checked", global::picThingyOLD.Properties.Settings.Default, "RefreshOnChange", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRefreshChange.Location = new System.Drawing.Point (6, 29);
            this.checkBoxRefreshChange.Name = "checkBoxRefreshChange";
            this.checkBoxRefreshChange.Size = new System.Drawing.Size (152, 17);
            this.checkBoxRefreshChange.TabIndex = 3;
            this.checkBoxRefreshChange.Text = "Refresh search on change";
            this.toolTip.SetToolTip (this.checkBoxRefreshChange, "Refresh the search every time a post is added or edited");
            this.checkBoxRefreshChange.UseVisualStyleBackColor = true;
            // 
            // tabPagePagesOpt
            // 
            this.tabPagePagesOpt.Controls.Add (this.numericUpDownThumbSize);
            this.tabPagePagesOpt.Controls.Add (this.labelThumbSize);
            this.tabPagePagesOpt.Controls.Add (this.labelPageMax);
            this.tabPagePagesOpt.Controls.Add (this.numericUpDownPageMax);
            this.tabPagePagesOpt.Location = new System.Drawing.Point (4, 22);
            this.tabPagePagesOpt.Name = "tabPagePagesOpt";
            this.tabPagePagesOpt.Padding = new System.Windows.Forms.Padding (3);
            this.tabPagePagesOpt.Size = new System.Drawing.Size (552, 282);
            this.tabPagePagesOpt.TabIndex = 1;
            this.tabPagePagesOpt.Text = "Pages and search window";
            this.tabPagePagesOpt.UseVisualStyleBackColor = true;
            // 
            // numericUpDownThumbSize
            // 
            this.numericUpDownThumbSize.DataBindings.Add (new System.Windows.Forms.Binding ("Value", global::picThingyOLD.Properties.Settings.Default, "ThumbSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDownThumbSize.Location = new System.Drawing.Point (6, 57);
            this.numericUpDownThumbSize.Maximum = new decimal (new int [] {
            1000,
            0,
            0,
            0});
            this.numericUpDownThumbSize.Name = "numericUpDownThumbSize";
            this.numericUpDownThumbSize.Size = new System.Drawing.Size (60, 20);
            this.numericUpDownThumbSize.TabIndex = 12;
            this.toolTip.SetToolTip (this.numericUpDownThumbSize, "Specifies the thumbnail size in the search window. \'0\' means auto (Automatically " +
        "resizes thumbnails based on window size)");
            this.numericUpDownThumbSize.Value = global::picThingyOLD.Properties.Settings.Default.ThumbSize;
            // 
            // labelThumbSize
            // 
            this.labelThumbSize.AutoSize = true;
            this.labelThumbSize.Location = new System.Drawing.Point (3, 41);
            this.labelThumbSize.Name = "labelThumbSize";
            this.labelThumbSize.Size = new System.Drawing.Size (77, 13);
            this.labelThumbSize.TabIndex = 11;
            this.labelThumbSize.Text = "Thumbnail size";
            this.toolTip.SetToolTip (this.labelThumbSize, "Specifies the thumbnail size in the search window. \'0\' means auto (Automatically " +
        "resizes thumbnails based on window size)");
            // 
            // labelPageMax
            // 
            this.labelPageMax.AutoSize = true;
            this.labelPageMax.Location = new System.Drawing.Point (3, 3);
            this.labelPageMax.Name = "labelPageMax";
            this.labelPageMax.Size = new System.Drawing.Size (112, 13);
            this.labelPageMax.TabIndex = 10;
            this.labelPageMax.Text = "Max. entries per page:";
            this.toolTip.SetToolTip (this.labelPageMax, "\'0\' means infinite (Disables pages)");
            // 
            // numericUpDownPageMax
            // 
            this.numericUpDownPageMax.DataBindings.Add (new System.Windows.Forms.Binding ("Value", global::picThingyOLD.Properties.Settings.Default, "MaxPostsPerPage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDownPageMax.Location = new System.Drawing.Point (6, 18);
            this.numericUpDownPageMax.Maximum = new decimal (new int [] {
            30000,
            0,
            0,
            0});
            this.numericUpDownPageMax.Name = "numericUpDownPageMax";
            this.numericUpDownPageMax.Size = new System.Drawing.Size (60, 20);
            this.numericUpDownPageMax.TabIndex = 9;
            this.toolTip.SetToolTip (this.numericUpDownPageMax, "\'0\' means infinite (Disables pages)");
            this.numericUpDownPageMax.Value = global::picThingyOLD.Properties.Settings.Default.MaxPostsPerPage;
            // 
            // openFileDialogJSON
            // 
            this.openFileDialogJSON.DefaultExt = "JSON";
            this.openFileDialogJSON.Filter = "JSON files|*.JSON|All files| *.*";
            this.openFileDialogJSON.SupportMultiDottedExtensions = true;
            // 
            // settingsForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size (584, 361);
            this.ControlBox = false;
            this.Controls.Add (this.tabControl);
            this.Controls.Add (this.buttonCancel);
            this.Controls.Add (this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "settingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            this.tabControl.ResumeLayout (false);
            this.tabPageGeneral.ResumeLayout (false);
            this.tabPageGeneral.PerformLayout ();
            this.tabPagePagesOpt.ResumeLayout (false);
            this.tabPagePagesOpt.PerformLayout ();
            ((System.ComponentModel.ISupportInitialize) (this.numericUpDownThumbSize)).EndInit ();
            ((System.ComponentModel.ISupportInitialize) (this.numericUpDownPageMax)).EndInit ();
            this.ResumeLayout (false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.CheckBox checkBoxSaveChange;
        private System.Windows.Forms.CheckBox checkBoxRefreshChange;
        private System.Windows.Forms.Button buttonDataPathBrowse;
        private System.Windows.Forms.TextBox textBoxDataPath;
        private System.Windows.Forms.Label labelDataPath;
        private System.Windows.Forms.Button buttonDataLoad;
        private System.Windows.Forms.Button buttonDataSave;
        private System.Windows.Forms.NumericUpDown numericUpDownPageMax;
        private System.Windows.Forms.Label labelPageMax;
        private System.Windows.Forms.TabPage tabPagePagesOpt;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.OpenFileDialog openFileDialogJSON;
        private System.Windows.Forms.NumericUpDown numericUpDownThumbSize;
        private System.Windows.Forms.Label labelThumbSize;
    }
}