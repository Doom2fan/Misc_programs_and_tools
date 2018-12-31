using System;
using System.Collections.Generic;
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

namespace e621PoolDownloader {
    /// <summary>
    /// Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : Window {
        protected string InputPrompt { get; set; } = "";
        protected string InputText { get; set; } = "";
        public string Value { get; protected set; } = "";

        protected InputBox () {
            InitializeComponent ();
        }

        protected InputBox (string prompt, string text, string defaultValue) : this () {
            InputPrompt = prompt;
            InputText = text;
            textBoxInput.Text = defaultValue;

            Title = InputPrompt;
            labelText.Text = InputText;
        }

        public static string ShowInputBox (string prompt, string text = "", string defaultValue = "") {
            string input;

            InputBox box = new InputBox (prompt, text, defaultValue);

            if (box.ShowDialog () != true)
                input = null;
            else
                input = box.Value;

            return input;
        }

        private void TextBoxInput_TextChanged (object sender, TextChangedEventArgs e) {
            this.Value = textBoxInput.Text;
        }

        private void ButtonAccept_Click (object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close ();
        }
    }
}
