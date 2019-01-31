# -*- coding: utf-8 -*-

from enum import Enum

# All integers are little-endian.
class HostCodes(Enum):
    '''
     Input protocol:
      * SC_Input - 1 byte
      * Buttons bitfield - 2 bytes
      * Left analog X - 1 bytes [0-255]
      * Left analog Y - 1 bytes [0-255]
      * Right analog X - 1 bytes [0-255]
      * Right analog Y - 1 bytes [0-255]
    '''
    HC_PollInput = 1

class SlaveCodes(Enum):
    SC_Input = 1

class ControllerButtons(Enum):
    BTN_Button1 = 0
    BTN_Button2 = 1
    BTN_Button3 = 2
    BTN_Button4 = 3
    BTN_L1 = 4
    BTN_R1 = 5
    BTN_L2 = 6
    BTN_R2 = 7
    BTN_Start = 8
    BTN_Select = 9
    BTN_POV_Up = 10
    BTN_POV_Down = 11
    BTN_POV_Left = 12
    BTN_POV_Right = 13

# %%