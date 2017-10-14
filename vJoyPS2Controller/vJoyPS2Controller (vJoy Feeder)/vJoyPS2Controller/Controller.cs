using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using vGenInterfaceWrap;

namespace vJoyPS2Controller {
    public class Controller : IDisposable {
        private static vGen vJoy = new vGen ();

        private uint handle;
        private vGen.JoystickState iReport;
        public bool Initialized { get; private set; }
        public uint Id { get; private set; }

        public Controller () {
            handle = 0;
            iReport = new vGen.JoystickState ();
            Initialized = false;
            Id = 0;
        }

        public bool Initialize (uint id) {
            if (id <= 0 || (id > 15 && id < 1001) || id > 1004)
                throw new ArgumentException ("Invalid ID!", "id");

            bool xbox = id > 1000;
            
            // Test for the driver
            if (!vJoy.vJoyEnabled ()) {
                Console.Write (Color.Red, "vJoy driver not enabled\n");
                return false;
            }
            if (xbox && vJoy.isVBusExist () != 0) {
                Console.Write (Color.Red, "vXbox driver not enabled\n");
                return false;
            }

            // Test if DLL matches the driver
            uint DllVer = 0, DrvVer = 0;
            if (!xbox && !vJoy.DriverMatch (ref DllVer, ref DrvVer)) {
                Console.Write (Color.Red, "Driver version ({0:X}) does NOT match DLL version ({1:X})\n", DrvVer, DllVer);
                return false;
            }

            // Get the state of the controller
            VjdStat status = vJoy.GetVJDStatus (id);
            switch (status) {
                case VjdStat.VJD_STAT_FREE:
                    Console.Write (Color.White, "Device {0} is free\n", id);
                    break;
                case VjdStat.VJD_STAT_OWN:
                    Console.Write (Color.Red, "Device {0} is already owned by this feeder\n", id);
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    Console.Write (Color.Red, "Device {0} is already owned by another feeder\n", id);
                    return false;
                case VjdStat.VJD_STAT_MISS:
                    Console.Write (Color.Red, "Device {0} is not installed or disabled\n", id);
                    return false;
                default:
                    Console.Write (Color.Red, "Device {0}: UNKNOWN ERROR\n", id);
                    return false;
            }

            // Make sure the controller has the required configuration
            if (vJoy.GetVJDButtonNumber (id) < 12 || vJoy.GetVJDContPovNumber (id) < 1 ||
                !vJoy.GetVJDAxisExist (id, HID_USAGES.HID_USAGE_X) || !vJoy.GetVJDAxisExist (id, HID_USAGES.HID_USAGE_Y) ||
                !vJoy.GetVJDAxisExist (id, HID_USAGES.HID_USAGE_Z) || !vJoy.GetVJDAxisExist (id, HID_USAGES.HID_USAGE_RX)) {
                Console.Write (Color.Red, "Unsupported controller: Controller does not match required configuration. (12+ buttons, 1+ POV, X, Y, Z and RX axes)\n");
                return false;
            }

            // Acquire the controller
            if (status == VjdStat.VJD_STAT_FREE && !vJoy.AcquireVJD (id)) {
                Console.Write (Color.Red, String.Format ("Failed to acquire vJoy device number {0}\n", id));
                return false;
            } else
                Console.Write (Color.Green, String.Format ("Acquired: vJoy device number {0}\n", id));

            // No FFB for now...
            /*if (vJoy.IsDeviceFfb (id)) {
                vJoy.FfbRegisterGenCB (new vGen.FfbCbFunc (FFBCallback), null);
            }*/

            handle = id;
            Initialized = true;
            return true;
        }

        /*public void FFBCallback (IntPtr objPtr, object objData) {

        }*/

        private static int ScaleAxis (int val) {
            return Math.Max (Math.Min ((int) (val * 128), 32768), 0);
        }
        public void Update (int x, int y, int z, int rz, uint buttons, int pov) {
            iReport.bDevice = (byte) handle;
            iReport.AxisX = ScaleAxis (x);
            iReport.AxisY = ScaleAxis (y);
            iReport.AxisZ = ScaleAxis (z);
            iReport.AxisXRot = ScaleAxis (rz);
            iReport.Buttons = buttons & 0x00000FFF;
            iReport.bHats = (uint) pov;
            
            vJoy.UpdateVJD (handle, ref iReport);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose (bool disposing) {
            if (!disposedValue) {
                //if (disposing) { }

                if (Initialized) { // Must release the controller
                    vJoy.RelinquishDev ((int) handle);
                }

                disposedValue = true;
            }
        }

        ~Controller () {
            Dispose (false);
        }

        public void Dispose () {
            Dispose (true);
            GC.SuppressFinalize (this);
        }
        #endregion
    }
}
