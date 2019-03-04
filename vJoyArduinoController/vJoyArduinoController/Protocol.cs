using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vJoyArduinoController {
    public static class ProtocolData {
        /*
         * All integers are little-endian.
        */

        public const int InputSize = 4 + (5 * 2);
        public static readonly char [] HeaderChars = {
            'C',
            'T',
            'R',
            'L',
        };

        public enum HostCodes : byte {
            /* Input protocol:
             * Header - 4 byte - CTRL
             * Buttons bitfield - 2 bytes
             * Left analog X - 2 bytes [0-65535]
             * Left analog Y - 2 bytes [0-65535]
             * Right analog X - 2 bytes [0-65535]
             * Right analog Y - 2 bytes [0-65535]
            */
            PollInput = 1,

            /* Rumble protocol:
             * HC_SetRumble - 1 byte
             * Small motor - 1 byte [0-255]
             * Big motor - 1 byte [0-255]
            */
            SetRumble = 2,

            /* Info protocol:
             * SC_Info - 1 byte
             * Name string [Line feed terminated]
             * Error string [Line feed terminated]
            */
            InfoRequest = 127,
        }

        public enum SlaveCodes : byte {
            Input = 1,
            Info = 2,
        }

        public enum ControllerButtons : ushort {
            BTN_Button1 = 0,
            BTN_Button2 = 1,
            BTN_Button3 = 2,
            BTN_Button4 = 3,
            BTN_L1 = 4,
            BTN_R1 = 5,
            BTN_L2 = 6,
            BTN_R2 = 7,
            BTN_L3 = 8,
            BTN_R3 = 9,
            BTN_Start = 10,
            BTN_Select = 11,
            BTN_POV_Up = 12,
            BTN_POV_Down = 13,
            BTN_POV_Left = 14,
            BTN_POV_Right = 15,
        }
    }
}
