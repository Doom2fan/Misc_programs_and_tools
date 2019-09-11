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

            int defRateIndex = comboBoxBaudRate.Items.IndexOf ("115200");
            comboBoxBaudRate.SelectedIndex = defRateIndex;

            UpdatePortsList ();
        }

        protected void UpdatePortsList () {
            comboBoxPort.SuspendLayout ();

            comboBoxPort.Sorted = true;

            string [] ports = SerialPort.GetPortNames ().Distinct ().ToArray ();

            comboBoxPort.Items.Clear ();
            comboBoxPort.Items.AddRange (ports);

            comboBoxPort.ResumeLayout ();
        }

        private void DoMinimizeToTray () {
            if (Visible) {
                ShowInTaskbar = false;
                Visible = false;
                minimizeToTrayToolStripMenuItem.Text = "Restore";
            } else {
                ShowInTaskbar = true;
                Visible = true;
                minimizeToTrayToolStripMenuItem.Text = "Send to tray";
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
            var ctrl = new Controller ();
            var port = new SerialPort (info.port, info.baudRate, Parity.None, 8, StopBits.One);
            ctrl.Initialize (info.id);

            try {
                port.Open ();
            } catch (Exception e) {
                ctrl.Dispose ();
                Console.Write (Color.Red, "{0}\n", e.ToString ());
                info.form.ControllerDone ();
                return;
            }

            var timer = new Stopwatch ();
            timer.Start ();
            bool waitingForResponse = false;

            while (port.IsOpen) {
                if (info.abort) {
                    if (ctrl != null)
                        ctrl.Dispose ();

                    if (port != null) {
                        port.Close ();
                        port.Dispose ();
                    }

                    info.form.ControllerDone ();

                    return;
                }
                if (!waitingForResponse && timer.ElapsedMilliseconds >= 5) {
                    port.Write (new byte [] { (byte) ProtocolData.HostCodes.PollInput }, 0, 1);
                    waitingForResponse = true;
                }

                while (port.BytesToRead >= ProtocolData.InputSize) {
                    char [] header = new char [4];
                    port.Read (header, 0, 4);

                    if (String.Equals (header.ToString (), ProtocolData.HeaderChars.ToString ())) {
                        ReadInput (ctrl, port);
                        waitingForResponse = false;
                    }
                }
            }
        }

        private static void ReadInput (Controller ctrl, SerialPort port) {
            byte [] inputBuffer = new byte [5 * 2];

            port.Read (inputBuffer, 0, inputBuffer.Length);

            ushort buttons = Utils.ToUInt16 (inputBuffer, 0);
            short leftX = Utils.ToInt16 (inputBuffer, 2);
            short leftY = Utils.ToInt16 (inputBuffer, 4);
            short rightX = Utils.ToInt16 (inputBuffer, 6);
            short rightY = Utils.ToInt16 (inputBuffer, 8);

            ctrl.Update (leftX, leftY, rightX, rightY, (ProtocolData.ControllerButtons) buttons); // Update the controller.
        }

        public void ControllerDone () {
            if (InvokeRequired) {
                Invoke (new Action (ControllerDone));
                return;
            }

            startToolStripMenuItem.Text = "Start";
            buttonToggle.Text = "Start";
            controllerRunning = false;
        }

        private bool StartController () {
            if (controllerRunning)
                return false;

            var info = new DCInfo ();
            info.id = ((uint) numericUpDownDevID.Value) + (checkBoxXbox.Checked ? 1000u : 0u);
            info.port = comboBoxPort.Text;
            info.abort = false;
            info.form = this;

            if (string.IsNullOrWhiteSpace (info.port)) {
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

        private void StopController () {
            if (!controllerRunning)
                return;

            if (ctrlThread != null && ctrlThread.Item1 != null)
                ctrlThread.Item2.abort = true;

            controllerRunning = false;
        }

        private void ToggleController () {
            if (!controllerRunning) {
                if (StartController ()) {
                    this.startToolStripMenuItem.Text = "Stop";
                    this.buttonToggle.Text = "Stop";
                }
            } else
                StopController ();
        }

        private void startToolStripMenuItem_Click (object sender, EventArgs e) { ToggleController (); }
        private void buttonToggle_Click (object sender, EventArgs e) { ToggleController (); }
        private void minimizeToTrayToolStripMenuItem_Click (object sender, EventArgs e) { DoMinimizeToTray (); }

        private void Form1_Leave (object sender, EventArgs e) {
            if (WindowState == FormWindowState.Minimized) {
                ShowInTaskbar = false;
                Visible = false;
                minimizeToTrayToolStripMenuItem.Text = "Restore";
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
