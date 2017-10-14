namespace vJoyPS2Controller {
    partial class FormMain {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.labelIP = new System.Windows.Forms.Label();
            this.maskedTextBoxIP = new System.Windows.Forms.MaskedTextBox();
            this.buttonToggle = new System.Windows.Forms.Button();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizeToTrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownDevID = new System.Windows.Forms.NumericUpDown();
            this.checkBoxXbox = new System.Windows.Forms.CheckBox();
            this.richTextBoxConsole = new System.Windows.Forms.RichTextBox();
            this.trayContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDevID)).BeginInit();
            this.SuspendLayout();
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(12, 9);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(81, 13);
            this.labelIP.TabIndex = 0;
            this.labelIP.Text = "PS2 IP Address";
            // 
            // maskedTextBoxIP
            // 
            this.maskedTextBoxIP.Location = new System.Drawing.Point(12, 25);
            this.maskedTextBoxIP.Mask = "000.000.000.000";
            this.maskedTextBoxIP.Name = "maskedTextBoxIP";
            this.maskedTextBoxIP.Size = new System.Drawing.Size(91, 20);
            this.maskedTextBoxIP.TabIndex = 1;
            // 
            // buttonToggle
            // 
            this.buttonToggle.Location = new System.Drawing.Point(12, 113);
            this.buttonToggle.Name = "buttonToggle";
            this.buttonToggle.Size = new System.Drawing.Size(75, 23);
            this.buttonToggle.TabIndex = 2;
            this.buttonToggle.Text = "Start";
            this.buttonToggle.UseVisualStyleBackColor = true;
            this.buttonToggle.Click += new System.EventHandler(this.buttonToggle_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayContextMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "PS2CONTROLLER";
            this.trayIcon.Visible = true;
            // 
            // trayContextMenu
            // 
            this.trayContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.minimizeToTrayToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.trayContextMenu.Name = "trayContextMenu";
            this.trayContextMenu.Size = new System.Drawing.Size(138, 70);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // minimizeToTrayToolStripMenuItem
            // 
            this.minimizeToTrayToolStripMenuItem.Name = "minimizeToTrayToolStripMenuItem";
            this.minimizeToTrayToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.minimizeToTrayToolStripMenuItem.Text = "Send to tray";
            this.minimizeToTrayToolStripMenuItem.Click += new System.EventHandler(this.minimizeToTrayToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Controller ID";
            // 
            // numericUpDownDevID
            // 
            this.numericUpDownDevID.Location = new System.Drawing.Point(12, 64);
            this.numericUpDownDevID.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownDevID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDevID.Name = "numericUpDownDevID";
            this.numericUpDownDevID.Size = new System.Drawing.Size(42, 20);
            this.numericUpDownDevID.TabIndex = 4;
            this.numericUpDownDevID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // checkBoxXbox
            // 
            this.checkBoxXbox.AutoSize = true;
            this.checkBoxXbox.Location = new System.Drawing.Point(12, 90);
            this.checkBoxXbox.Name = "checkBoxXbox";
            this.checkBoxXbox.Size = new System.Drawing.Size(96, 17);
            this.checkBoxXbox.TabIndex = 5;
            this.checkBoxXbox.Text = "Xbox controller";
            this.checkBoxXbox.UseVisualStyleBackColor = true;
            this.checkBoxXbox.CheckedChanged += new System.EventHandler(this.checkBoxXbox_CheckedChanged);
            // 
            // richTextBoxConsole
            // 
            this.richTextBoxConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxConsole.Location = new System.Drawing.Point(114, 12);
            this.richTextBoxConsole.Name = "richTextBoxConsole";
            this.richTextBoxConsole.Size = new System.Drawing.Size(513, 347);
            this.richTextBoxConsole.TabIndex = 6;
            this.richTextBoxConsole.Text = "";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 371);
            this.Controls.Add(this.richTextBoxConsole);
            this.Controls.Add(this.checkBoxXbox);
            this.Controls.Add(this.numericUpDownDevID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonToggle);
            this.Controls.Add(this.maskedTextBoxIP);
            this.Controls.Add(this.labelIP);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Leave += new System.EventHandler(this.Form1_Leave);
            this.trayContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDevID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIP;
        private System.Windows.Forms.Button buttonToggle;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayContextMenu;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minimizeToTrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownDevID;
        private System.Windows.Forms.CheckBox checkBoxXbox;
        internal System.Windows.Forms.RichTextBox richTextBoxConsole;
    }
}

