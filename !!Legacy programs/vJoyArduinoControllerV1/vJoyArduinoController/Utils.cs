using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vJoyArduinoController {
    public static class Utils {
        static internal uint ToUInt32 (byte [] value, int startIndex = 0) {
            if (value is null)
                throw new ArgumentNullException ("value");
            if (startIndex + 4 > value.Length)
                throw new ArgumentOutOfRangeException ("startIndex");

            var buffer = new byte [4];
            Array.Copy (value, startIndex, buffer, 0, 4);

            if (BitConverter.IsLittleEndian)
                Array.Reverse (buffer);

            return BitConverter.ToUInt32 (buffer, 0);
        }

        static internal ushort ToUInt16 (byte [] value, int startIndex = 0) {
            if (value is null)
                throw new ArgumentNullException ("value");
            if (startIndex + 2 > value.Length)
                throw new ArgumentOutOfRangeException ("startIndex");

            var buffer = new byte [2];
            Array.Copy (value, startIndex, buffer, 0, 2);

            if (BitConverter.IsLittleEndian)
                Array.Reverse (buffer);

            return BitConverter.ToUInt16 (buffer, 0);
        }

        static internal int Map (int val, int bottom, int top, int newBottom, int newTop) {
            return (val - bottom) / (top - bottom) * (newTop - newBottom);
        }
    }
}
