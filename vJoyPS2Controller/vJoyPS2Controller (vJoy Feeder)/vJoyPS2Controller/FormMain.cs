using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using vGenInterfaceWrap;

namespace vJoyPS2Controller {
    public partial class FormMain : Form {
        private bool controllerRunning = false;
        private Thread ctrlThread;

        public FormMain () {
            InitializeComponent ();
        }

        private void DoMinimizeToTray () {
            if (this.Visible == false) {
                this.ShowInTaskbar = false;
                this.Visible = false;
                this.minimizeToTrayToolStripMenuItem.Text = "Restore";
            } else {
                this.ShowInTaskbar = true;
                this.Visible = true;
                this.minimizeToTrayToolStripMenuItem.Text = "Send to tray";
            }
        }

        private struct DCInfo {
            public uint id;
            public byte [] ip;
            public FormMain form;
        }
        private struct ControlInfo {
            public byte x,  y,   // Left analog stick.
                        z, rz;   // Right analog stick.
            public uint buttons; // Buttons. Bitfield. { 1: Triangle, 2: Circle, 3: Cross, 4: Square, 5: L1, 6: R1, 7: L2, 8: R2, 9: L3, 10: R3, 11: Select, 12: Start }
            public byte pov;     // D-pad. Bitfield. { 1: Up, 2: Right, 3: Down, 4: Left }
        }
        private const byte POV_UP    = 1;
        private const byte POV_RIGHT = 1 << 1;
        private const byte POV_DOWN  = 1 << 2;
        private const byte POV_LEFT  = 1 << 3;
        private unsafe static void DoController (object target) {
            var info = (DCInfo) target;

            IPAddress ip = new IPAddress (info.ip);
            TcpClient tcpClient = new TcpClient ();

            try {
                tcpClient.Connect (new IPEndPoint (ip, 5120));
            } catch (Exception e) {
                Console.Write (Color.Red, "{0}\n", e.ToString ());
                info.form.ControllerDone ();
                return;
            }

            Console.Write (Color.White, "Initializing controller\n");
            Controller ctrl = new Controller ();
            ControlInfo ctrlInfo = new ControlInfo ();
            ctrl.Initialize (info.id);

            NetworkStream nstr = tcpClient.GetStream ();
            var buffer = new byte [sizeof (ControlInfo) * 2]; // *2 so we have some extra room in case we need it.
            int i = 0;
            bool povUp, povRight, povDown, povLeft;
            while (true) {
                try {
                    nstr.WriteByte (0x7F); // Poll for data
                    nstr.Read (buffer, 0, sizeof (ControlInfo)); // Retrieve controller data.
                    ctrlInfo.x = buffer [i++]; ctrlInfo.y = buffer [i++]; // Assemble the controller info.
                    ctrlInfo.z = buffer [i++]; ctrlInfo.rz = buffer [i++];
                    ctrlInfo.buttons = BitConverter.ToUInt32 (buffer, i);
                    i += 4;
                    ctrlInfo.pov = buffer [i++];
                    i = 0;

                    int pov = 0;
                    if (ctrlInfo.pov == 0 ||
                        (ctrlInfo.pov & (POV_UP | POV_DOWN)) == (POV_UP | POV_DOWN) ||
                        (ctrlInfo.pov & (POV_LEFT | POV_RIGHT)) == (POV_LEFT | POV_RIGHT))
                        pov = -1;
                    else {
                        povUp    = (ctrlInfo.pov & POV_UP)    == POV_UP;
                        povRight = (ctrlInfo.pov & POV_RIGHT) == POV_RIGHT;
                        povDown  = (ctrlInfo.pov & POV_DOWN)  == POV_DOWN;
                        povLeft  = (ctrlInfo.pov & POV_LEFT)  == POV_LEFT;
                        if (povUp && povRight)
                            pov = 4500;
                        else if (povDown && povRight)
                            pov = 13500;
                        else if (povUp && povLeft)
                            pov = 31500;
                        else if (povDown && povLeft)
                            pov = 22500;
                        else if (povUp)
                            pov = 0;
                        else if (povRight)
                            pov = 9000;
                        else if (povDown)
                            pov = 18000;
                        else if (povLeft)
                            pov = 27000;
                    }

                    ctrl.Update (ctrlInfo.x, ctrlInfo.y, ctrlInfo.z, ctrlInfo.rz, ctrlInfo.buttons, pov); // Update the controller.

                    Thread.Sleep (20); // Wait a bit before updating again...
                } catch (ThreadAbortException) {
                    ctrl.Dispose ();
                    return;
                } catch (Exception e) {
                    ctrl.Dispose ();
                    Console.Write (Color.Red, "{0}\n", e.ToString ());
                    info.form.ControllerDone ();
                    return;
                }
            }
        }

        private delegate void ControlDoneDlg ();
        public void ControllerDone () {
            if (this.InvokeRequired)
                this.Invoke (new ControlDoneDlg (ControllerDone));
            else {
                this.startToolStripMenuItem.Text = "Start";
                this.buttonToggle.Text = "Start";
                controllerRunning = false;
            }
        }

        private void StartController () {
            if (controllerRunning)
                return;

            var info = new DCInfo ();
            info.id = ((uint) numericUpDownDevID.Value) + (checkBoxXbox.Checked ? 1000u : 0u);
            info.ip = new byte [4];
            info.form = this;

            var split = maskedTextBoxIP.Text.Split ('.');
            byte b;
            for (int i = 0; i < 4; i++) {
                if (!byte.TryParse (split [i], out b))
                    MessageBox.Show ("Invalid IP", "The specified IP contains invalid characters");
                else
                    info.ip [i] = b;

            }
            var pts = new ParameterizedThreadStart (DoController);
            ctrlThread = new Thread (pts);
            ctrlThread.Start (info);
            controllerRunning = true;
        }

        private void StopController () {
            if (!controllerRunning)
                return;

            if (ctrlThread != null)
                ctrlThread.Abort ();
            controllerRunning = false;
        }

        private void ToggleController () {
            if (!controllerRunning) {
                StartController ();
                this.startToolStripMenuItem.Text = "Stop";
                this.buttonToggle.Text = "Stop";
            } else {
                StopController ();
                this.startToolStripMenuItem.Text = "Start";
                this.buttonToggle.Text = "Start";
            }
        }

        private void startToolStripMenuItem_Click (object sender, EventArgs e) { ToggleController (); }
        private void buttonToggle_Click (object sender, EventArgs e) { ToggleController (); }
        private void minimizeToTrayToolStripMenuItem_Click (object sender, EventArgs e) { DoMinimizeToTray (); }

        private void Form1_Leave (object sender, EventArgs e) {
            if (this.WindowState == FormWindowState.Minimized) {
                this.ShowInTaskbar = false;
                this.Visible = false;
                this.minimizeToTrayToolStripMenuItem.Text = "Restore";
            }
        }

        private void exitToolStripMenuItem_Click (object sender, EventArgs e) {
            StopController ();
            Application.Exit ();
        }

        private void checkBoxXbox_CheckedChanged (object sender, EventArgs e) {
            numericUpDownDevID.Value = 1;
            if (checkBoxXbox.Checked)
                numericUpDownDevID.Maximum = 4;
            else
                numericUpDownDevID.Maximum = 16;
        }

        private void FormMain_FormClosing (object sender, FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.UserClosing) {
                if (ctrlThread != null && ctrlThread.IsAlive) {
                    e.Cancel = true;
                    MessageBox.Show ("You must stop the controller before closing.");
                }
            } else {
                if (ctrlThread != null)
                    ctrlThread.Abort ();
            }
        }
    }
}
