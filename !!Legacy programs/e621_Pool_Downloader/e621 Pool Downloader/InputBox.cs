using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace e621_Pool_Downloader {
    public partial class InputBox : Form {
        protected string InputPrompt { get; set; } = "";
        protected string InputText { get; set; } = "";
        public string Value { get; protected set; } = "";

        public InputBox () {
            InitializeComponent ();
        }

        public static string ShowInputBox (string prompt, string text = "", string defaultValue = "") {
            string input;

            using (InputBox box = new InputBox ()) {
                box.InputPrompt = prompt;
                box.InputText = text;
                box.textBoxInput.Text = defaultValue;

                if (box.ShowDialog () != DialogResult.OK)
                    input = null;
                else
                    input = box.Value;
            }

            return input;
        }

        private void InputBox_Shown (object sender, EventArgs e) {
            this.Text = this.InputPrompt;
            this.labelText.Text = this.InputText;

            this.SuspendLayout ();
            
            // Set the location of the controls
            this.textBoxInput.Location = new Point (this.textBoxInput.Location.X, this.labelText.Location.Y + this.labelText.Size.Height + 3);
            this.buttonOk.Location = new Point (this.buttonOk.Location.X, this.textBoxInput.Location.Y + this.textBoxInput.Size.Height + 6);
            this.buttonCancel.Location = new Point (this.buttonCancel.Location.X, this.buttonOk.Location.Y);
            // Set the size of the form
            this.ClientSize = new Size (Math.Max (this.Size.Width, this.labelText.Location.X + this.labelText.Size.Width + 28), this.buttonCancel.Location.Y + this.buttonCancel.Size.Height + 14);
            
            this.ResumeLayout ();
        }

        private void textBoxInput_TextChanged (object sender, EventArgs e) {
            this.Value = textBoxInput.Text;
        }
    }
}
