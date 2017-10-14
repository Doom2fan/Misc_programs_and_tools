using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Linq;
//using MathHelper;

namespace CartesianPlotter {
    public partial class FormMain : Form {
        Pen p;
        public FormMain () {
            InitializeComponent ();
            p = new Pen (Brushes.Black);
            scintilla.Font = new System.Drawing.Font ("Courier New", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        }

        private void RunCode () {
            Bitmap bmp = new Bitmap (2048, 2048);
            Graphics graph = Graphics.FromImage (bmp);

            p.Width = 2;
            p.Color = Color.Black;
            graph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graph.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            graph.Clear (Color.White);
            graph.DrawLine (new Pen (Color.Black, 1f), new PointF (1024f, 0f), new PointF (1024f, 2048f));
            graph.DrawLine (new Pen (Color.Black, 1f), new PointF (0f, 1024f), new PointF (2048f, 1024f));

            try {
                var cp = CodeDomProvider.CreateProvider ("C#");
                CompilerResults cr;
                var cParams = new CompilerParameters (new string [] { "Microsoft.CSharp.dll", "System.dll", "System.Drawing.dll", "System.Windows.Forms.dll", "MathHelper.dll" });
                cParams.GenerateInMemory = true;

                string code = String.Concat ("using System; using System.Drawing; using MathHelper;", scintilla.Text);
                using (var reader = new StringReader (code))
                    cr = cp.CompileAssemblyFromSource (cParams, scintilla.Text);

                bool noErrors = true;
                List<string> lines = new List<string> ();

                if (cr.Errors.Count > 0) {
                    if (cr.Errors.HasErrors) noErrors = false;

                    foreach (var error in cr.Errors)
                        lines.Add (error.ToString ());
                }
                if (!cr.Errors.HasErrors && cr.CompiledAssembly == null) {
                    lines.Add ("cr.CompiledAssembly is null");
                    noErrors = false;
                } /*else if (!cr.Errors.HasErrors && cr.CompiledAssembly.EntryPoint == null) {
                    lines.Add ("cr.CompiledAssembly.EntryPoint is null");
                    noErrors = false;
                }*/

                if (lines.Count > 0)
                    MessageBox.Show (string.Join ("\n", lines.ToArray ()), noErrors ? "Compilation warnings" : "Compilation errors");

                if (!noErrors) {
                    graph.Dispose (); bmp.Dispose ();
                    return;
                }

                var run = cr.CompiledAssembly.GetStaticMethod ("*.Run", p, graph);
                if (run != null) {
                    run.Invoke (p, graph);
                } else {
                    MessageBox.Show ("Unknown error", "Script error");
                }
            } catch (Exception e) {
                MessageBox.Show (String.Format ("{0}\n\nSource: {1}\nInner exception: {2}\n\nStack trace:\n{3}", e.Message, e.Source, e.InnerException, e.StackTrace), "Exception");
                graph.Dispose (); bmp.Dispose ();
                return;
            }

            graph.Dispose ();
            if (pictureBox.Image != null)
                pictureBox.Image.Dispose ();
            pictureBox.Image = bmp;
        }

        private void newToolStripMenuItem_Click (object sender, EventArgs e) {

        }

        private void openToolStripMenuItem_Click (object sender, EventArgs e) {

        }

        private void saveToolStripMenuItem_Click (object sender, EventArgs e) {

        }

        private void saveAsToolStripMenuItem_Click (object sender, EventArgs e) {

        }

        private void exitToolStripMenuItem_Click (object sender, EventArgs e) {

        }

        private void runToolStripMenuItem_Click (object sender, EventArgs e) {
            RunCode ();
        }

        private void settingsToolStripMenuItem_Click (object sender, EventArgs e) {
            SettingsForm form = new SettingsForm ();
            form.ShowDialog ();
        }

        private void scintilla_TextChanged (object sender, EventArgs e) {
            if (Program.Options.RunOnChange)
                RunCode ();
        }
    }
}
