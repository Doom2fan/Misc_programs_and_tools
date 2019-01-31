# -*- coding: utf-8 -*-
"""
RPi Handheld Console Input Driver

@author: Chronos
"""

import serial
import serial.tools.list_ports
import time
import uinput
from protocol import HostCodes, SlaveCodes, ControllerButtons

class ControllerInput:
    buttons = 0
    leftX   = 0
    leftY   = 0
    rightX  = 0
    rightY  = 0

def main():
    comPorts = serial.tools.list_ports.comports ()
    ctrlPort = None
    
    for w in comPorts:
        if (w.vid == 0x1A86 and w.pid == 0x7523 and w.product == "USB2.0-Serial"):
            ctrlPort = serial.Serial (port = w.device, baudrate = 115200)
            break
    
    if ctrlPort is None:
        raise serial.SerialException ("Could not find controller")
        return

    time.sleep (0.075)
    
    events = (
        uinput.BTN_A,
        uinput.BTN_B,
        uinput.BTN_X,
        uinput.BTN_Y,
        uinput.BTN_TL,
        uinput.BTN_TR,
        uinput.BTN_TL2,
        uinput.BTN_TR2,
        uinput.BTN_START,
        uinput.BTN_SELECT,
        uinput.ABS_X + (0, 255, 0, 0),
        uinput.ABS_Y + (0, 255, 0, 0),
        uinput.ABS_RX + (0, 255, 0, 0),
        uinput.ABS_RY + (0, 255, 0, 0),
        uinput.ABS_HAT0X + (0, 255, 0, 0),
        uinput.ABS_HAT0Y + (0, 255, 0, 0),
    )
    
    ctrlDevice = uinput.Device (events)
    
    lastPollTime = 0
    while True:
        curTime = time.time () * 1000.0
        if (lastPollTime - curTime) >= 15:
            ctrlPort.write (HostCodes.HC_PollInput)
            ctrlPort.flush ()
            lastPollTime = curTime

        while ctrlPort.in_waiting >= 7:
            readBytes = ctrlPort.read (7)
            if readBytes [0] != SlaveCodes.SC_Input:
                continue
            
            ctrlInput = ControllerInput ()
            ctrlInput.buttons = readBytes [1] << 8 | readBytes [2]
            ctrlInput.leftX = readBytes [3]
            ctrlInput.leftY = readBytes [4]
            ctrlInput.rightX = readBytes [5]
            ctrlInput.rightY = readBytes [6]
            
            updateController (ctrlDevice, ctrlInput)

buttonsList = (
    (ControllerButtons.BTN_Button1, uinput.BTN_A),
    (ControllerButtons.BTN_Button4, uinput.BTN_B),
    (ControllerButtons.BTN_Button2, uinput.BTN_X),
    (ControllerButtons.BTN_Button3, uinput.BTN_Y),
    (ControllerButtons.BTN_L1, uinput.BTN_TL),
    (ControllerButtons.BTN_R1, uinput.BTN_TR),
    (ControllerButtons.BTN_L2, uinput.BTN_TL2),
    (ControllerButtons.BTN_R2, uinput.BTN_TR2),
    (ControllerButtons.BTN_Start, uinput.BTN_START),
    (ControllerButtons.BTN_Select, uinput.BTN_SELECT),
)
def updateController (ctrlDevice, ctrlInput: ControllerInput):
    for btn in buttonsList:
        if (ctrlInput.buttons & (1 << btn [0])) == (1 << btn [0]):
            ctrlDevice.emit_click (btn [1], syn = False)
    
    ctrlDevice.Emit (uinput.ABS_X, ctrlInput.leftX, syn = False)
    ctrlDevice.Emit (uinput.ABS_Y, ctrlInput.leftY, syn = False)
    ctrlDevice.Emit (uinput.ABS_RX, ctrlInput.rightX, syn = False)
    ctrlDevice.Emit (uinput.ABS_RY, ctrlInput.rightY, syn = False)
    
    povHatX = 128
    povHatY = 128
    if (ctrlInput.buttons & (1 << ControllerButtons.BTN_POV_Left)) == (1 << ControllerButtons.BTN_POV_Left):
        povHatX -= 128
    if (ctrlInput.buttons & (1 << ControllerButtons.BTN_POV_Right)) == (1 << ControllerButtons.BTN_POV_Right):
        povHatX += 128
    if (ctrlInput.buttons & (1 << ControllerButtons.BTN_POV_Up)) == (1 << ControllerButtons.BTN_POV_Up):
        povHatY -= 128
    if (ctrlInput.buttons & (1 << ControllerButtons.BTN_POV_Down)) == (1 << ControllerButtons.BTN_POV_Down):
        povHatY += 128
    
    ctrlDevice.Emit (uinput.ABS_HAT0X, max (0, min (povHatX, 255)), syn = False)
    ctrlDevice.Emit (uinput.ABS_HAT0Y, max (0, min (povHatY, 255)), syn = False)
        
    ctrlDevice.syn ()

main ()