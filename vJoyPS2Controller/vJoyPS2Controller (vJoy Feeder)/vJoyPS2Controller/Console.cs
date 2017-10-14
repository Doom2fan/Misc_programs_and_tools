using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace vJoyPS2Controller {
    public static class Console {
        private static void AppendText (this RichTextBox box, string text, Color color) {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText (text);
            box.SelectionColor = box.ForeColor;

            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.ScrollToCaret ();
        }

        private delegate void WriteDelegate (Color color, string text);
        public static void Write (Color color, string text) {
            if (Program.form.richTextBoxConsole.InvokeRequired)
                Program.form.Invoke (new WriteDelegate (Write), color, text);
            else
                Program.form.richTextBoxConsole.AppendText (text, color);
        }

        private delegate void WriteFormatDelegate (Color color, string text, params object [] args);
        public static void Write (Color color, string text, params object [] args) {
            if (Program.form.richTextBoxConsole.InvokeRequired)
                Program.form.Invoke (new WriteFormatDelegate (Write), color, text, args);
            else
                Program.form.richTextBoxConsole.AppendText (String.Format (text, args), color);
        }
    }
}
