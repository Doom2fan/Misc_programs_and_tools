#pragma once
/*
 * All integers are little-endian.
*/

enum HostCodes {
    /* Input protocol:
     * Header - 4 byte - CTRL
     * Buttons bitfield - 2 bytes
     * Left analog X - 1 byte [0-255]
     * Left analog Y - 1 byte [0-255]
     * Right analog X - 1 byte [0-255]
     * Right analog Y - 1 byte [0-255]
    */
    HC_PollInput = 'A',
};

enum ControllerButtons {
    BTN_Button1 = 0,
    BTN_Button2 = 1,
    BTN_Button3 = 2,
    BTN_Button4 = 3,
    BTN_L1 = 4,
    BTN_R1 = 5,
    BTN_L2 = 6,
    BTN_R2 = 7,
    BTN_Start = 8,
    BTN_Select = 9,
    BTN_POV_Up = 10,
    BTN_POV_Down = 11,
    BTN_POV_Left = 12,
    BTN_POV_Right = 13,
};
