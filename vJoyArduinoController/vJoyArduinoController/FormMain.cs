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
using System.IO.Ports;
using System.Management;
using System.Diagnostics;

namespace vJoyArduinoController {
    public partial class FormMain : Form {
        private bool controllerRunning = false;
        private Tuple<Thread, DCInfo> ctrlThread;

        public FormMain () {
            InitializeComponent ();

            int defRateIndex = comboBoxBaudRate.Items.IndexOf ("250000");
            comboBoxBaudRate.SelectedIndex = defRateIndex;

            UpdatePortsList ();
        }

        protected void UpdatePortsList () {
            comboBoxPort.SuspendLayout ();

            comboBoxPort.Sorted = true;

            string [] ports = SerialPort.GetPortNames ().Distinct ().ToArray ();

            string selectedText = comboBoxPort.SelectedText;
            comboBoxPort.Items.Clear ();
            comboBoxPort.Items.AddRange (ports);

            comboBoxPort.ResumeLayout ();
        }

        private void DoMinimizeToTray () {
            if (this.Visible) {
                this.ShowInTaskbar = false;
                this.Visible = false;
                this.minimizeToTrayToolStripMenuItem.Text = "Restore";
            } else {
                this.ShowInTaskbar = true;
                this.Visible = true;
                this.minimizeToTrayToolStripMenuItem.Text = "Send to tray";
            }
        }

        private class DCInfo {
            public uint id;
            public string port;
            public int baudRate;

            public bool abort;
            public FormMain form;
        }
        private unsafe static void DoController (object target) {
            var info = (DCInfo) target;

            Console.Write (Color.Black, "Initializing controller\n");
            Controller ctrl = new Controller ();
            SerialPort port = new SerialPort (info.port, info.baudRate, Parity.None, 8, StopBits.One);
            ctrl.Initialize (info.id);

            try {
                port.Open ();
            } catch (Exception e) {
                ctrl.Dispose ();
                Console.Write (Color.Red, "{0}\n", e.ToString ());
                info.form.ControllerDone ();
                return;
            }

            while (port.IsOpen) {
                if (info.abort) {
                    ctrl.Dispose ();
                    if (port != null) {
                        port.Dispose ();
                    }

                    return;
                }
                port.Write (new byte [] { (byte) ProtocolData.HostCodes.PollInput }, 0, 1);

                int b = port.ReadByte ();
                switch (b) {
                    case (int) ProtocolData.SlaveCodes.Input:
                        ReadInput (ctrl, port);
                        break;
                }

                Thread.Sleep (5); // Wait a bit before updating again...
            }
        }

        private static void ReadInput (Controller ctrl, SerialPort port) {
            byte [] inputBuffer = new byte [5 * 2];

            port.Read (inputBuffer, 0, inputBuffer.Length);

            ushort buttons = Utils.ToUInt16 (inputBuffer, 0);
            int leftX = Utils.ToUInt16 (inputBuffer, 2) / 2;
            int leftY = Utils.ToUInt16 (inputBuffer, 4) / 2;
            int rightX = Utils.ToUInt16 (inputBuffer, 6) / 2;
            int rightY = Utils.ToUInt16 (inputBuffer, 8) / 2;

            bool povUp = ((buttons & (1 << (ushort) ProtocolData.ControllerButtons.BTN_POV_Up)) != 0),
                povDown = ((buttons & (1 << (ushort) ProtocolData.ControllerButtons.BTN_POV_Down)) != 0),
                povLeft = ((buttons & (1 << (ushort) ProtocolData.ControllerButtons.BTN_POV_Left)) != 0),
                povRight = ((buttons & (1 << (ushort) ProtocolData.ControllerButtons.BTN_POV_Right)) != 0);

            int pov = -1;

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

            ctrl.Update (leftX, leftY, rightX, rightY, (uint) (buttons & 0x0FFF), pov); // Update the controller.
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

        private bool StartController () {
            if (controllerRunning)
                return false;

            var info = new DCInfo ();
            info.id = ((uint) numericUpDownDevID.Value) + (checkBoxXbox.Checked ? 1000u : 0u);
            info.port = comboBoxPort.Text;
            info.abort = false;
            info.form = this;

            if (String.IsNullOrWhiteSpace (info.port)) {
                MessageBox.Show ("Invalid port", "The specified port is invalid");
                return false;
            }

            if (!int.TryParse (comboBoxBaudRate.Text, out info.baudRate)) {
                MessageBox.Show ("Invalid baud rate", "The specified baud rate is invalid");
                return false;
            }

            var pts = new ParameterizedThreadStart (DoController);
            ctrlThread = new Tuple<Thread, DCInfo> (new Thread (pts), info);
            ctrlThread.Item1.Start (info);
            controllerRunning = true;

            return true;
        }

        private bool StopController () {
            if (!controllerRunning)
                return false;

            if (ctrlThread != null && ctrlThread.Item1 != null)
                ctrlThread.Item2.abort = true;
            controllerRunning = false;

            return true;
        }

        private void ToggleController () {
            if (!controllerRunning) {
                if (StartController ()) {
                    this.startToolStripMenuItem.Text = "Stop";
                    this.buttonToggle.Text = "Stop";
                }
            } else {
                if (StopController ()) {
                    this.startToolStripMenuItem.Text = "Start";
                    this.buttonToggle.Text = "Start";
                }
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
                if (ctrlThread != null && ctrlThread.Item1 != null && ctrlThread.Item1.IsAlive) {
                    e.Cancel = true;
                    MessageBox.Show ("You must stop the controller before closing.");
                }
            } else {
                if (ctrlThread != null && ctrlThread.Item1 != null)
                    ctrlThread.Item2.abort = true;
            }
        }
    }
}
