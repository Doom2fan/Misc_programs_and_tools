using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using static vJoyArduinoController.ProtocolData;

namespace vJoyArduinoController {
    public class Controller : IDisposable {
        private ViGEmClient client;
        private Xbox360Controller ctrl;
        private Xbox360Report iReport;
        public bool Initialized { get; private set; }
        public uint Id { get; private set; }

        public Controller () {
            iReport = new Xbox360Report ();
            Initialized = false;
            Id = 0;
        }

        public bool Initialize (uint id) {
            if (id <= 0 || (id > 15 && id < 1001) || id > 1004)
                throw new ArgumentException ("Invalid ID!", "id");

            client = new ViGEmClient ();
            ctrl = new Xbox360Controller (client);

            ctrl.Connect ();

            Initialized = true;
            return true;
        }

        public void Update (short lX, short lY, short rX, short rY, ControllerButtons buttons) {
            // Axes
            iReport.LeftThumbX = lX;
            iReport.LeftThumbY = lY;
            iReport.RightThumbX = rX;
            iReport.RightThumbY = rY;

            // Face buttons
            iReport.SetButtonState (Xbox360Buttons.Y, (buttons & ControllerButtons.BTN_Button1) != 0);
            iReport.SetButtonState (Xbox360Buttons.B, (buttons & ControllerButtons.BTN_Button2) != 0);
            iReport.SetButtonState (Xbox360Buttons.A, (buttons & ControllerButtons.BTN_Button3) != 0);
            iReport.SetButtonState (Xbox360Buttons.X, (buttons & ControllerButtons.BTN_Button4) != 0);

            // POV buttons
            iReport.SetButtonState (Xbox360Buttons.Up   , (buttons & ControllerButtons.BTN_POV_Up   ) != 0);
            iReport.SetButtonState (Xbox360Buttons.Down , (buttons & ControllerButtons.BTN_POV_Down ) != 0);
            iReport.SetButtonState (Xbox360Buttons.Left , (buttons & ControllerButtons.BTN_POV_Left ) != 0);
            iReport.SetButtonState (Xbox360Buttons.Right, (buttons & ControllerButtons.BTN_POV_Right) != 0);

            // L buttons
            iReport.SetButtonState (Xbox360Buttons.LeftShoulder, (buttons & ControllerButtons.BTN_L1) != 0);
            iReport.LeftTrigger = ((buttons & ControllerButtons.BTN_L2) == ControllerButtons.BTN_L2) ? byte.MaxValue : byte.MinValue;
            iReport.SetButtonState (Xbox360Buttons.LeftThumb, (buttons & ControllerButtons.BTN_L3) != 0);
            // R buttons
            iReport.SetButtonState (Xbox360Buttons.RightShoulder, (buttons & ControllerButtons.BTN_R1) != 0);
            iReport.RightTrigger = ((buttons & ControllerButtons.BTN_R2) == ControllerButtons.BTN_R2) ? byte.MaxValue : byte.MinValue;
            iReport.SetButtonState (Xbox360Buttons.RightThumb, (buttons & ControllerButtons.BTN_R3) != 0);

            // Start/Select
            iReport.SetButtonState (Xbox360Buttons.Start, (buttons & ControllerButtons.BTN_Start) != 0);
            iReport.SetButtonState (Xbox360Buttons.Back, (buttons & ControllerButtons.BTN_Select) != 0);

            ctrl.SendReport (iReport);
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose (bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // Must release the controller
                    if (Initialized) {
                        ctrl.Disconnect ();
                        ctrl.Dispose ();
                        client.Dispose ();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose () {
            Dispose (true);
        }

        #endregion
    }
}
