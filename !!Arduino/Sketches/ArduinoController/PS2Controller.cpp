#include "Controller.h"
#include "PS2Controller.h"

PS2Controller *PS2Controller::Create (int clkPin, int cmdPin, int selPin, int dataPin, bool pressures, bool rumble) {
    PS2Controller *ctrl = new PS2Controller ();
    // Setup pins and settings: GamePad(clock, command, attention, data, Pressures?, Rumble?) check for error
    ctrl->error = ctrl->ps2x.config_gamepad (clkPin, cmdPin, selPin, dataPin, pressures, rumble);
    ctrl->type = ctrl->ps2x.readType ();

    return ctrl;
}

const char Err_NoControl [] = "No controller found, check wiring.";
const char Err_CmdRefuse [] = "Controller found but not accepting commands.";
const char Err_NoPress   [] = "Controller refusing to enter Pressures mode, may not support it.";
const char Ctrl_Unknown [] = "Generic PS2 controller";
const char Ctrl_DShock  [] = "Dualshock controller";
const char Ctrl_DShockW [] = "Wireless Dualshock controller";
const char Ctrl_Guitar [] = "Guitar Hero controller (UNSUPPORTED)";
void PS2Controller::DoSendInfo () {
    // Send our info
    char *err = nullptr;
    switch (error) {
        case 1: err = &Err_NoControl [0]; break;
        case 2: err = &Err_CmdRefuse [0]; break;
        case 3: err = &Err_NoPress   [0]; break;
    }

    char *ctrlName = nullptr;
    switch (type) {
        default:
        case 0: ctrlName = &Ctrl_Unknown [0]; break;
        case 1: ctrlName = &Ctrl_DShock  [0]; break;
        case 2: ctrlName = &Ctrl_Guitar  [0]; break;
        case 3: ctrlName = &Ctrl_DShockW [0]; break;
    }

    SendInfo (ctrlName, err);
}

void PS2Controller::SetRumble (byte m1, byte m2) {
    motor1 =  (m1 > 0 && m1 < 256);
    motor2 = ((m2 > 0 && m2 < 256) ? m2 : 0);
}

void PS2Controller::DoSendInput () {
    ps2x.read_gamepad (motor1, motor2);

    int16_t buttons = GetButtonsBitfield ();
    int16_t leftX  = ps2x.Analog (PSS_LX) * 257; //map (ps2x.Analog (PSS_LX), 0, 255, 0, 65535);
    int16_t leftY  = ps2x.Analog (PSS_LY) * 257; //map (ps2x.Analog (PSS_LY), 0, 255, 0, 65535);
    int16_t rightX = ps2x.Analog (PSS_RX) * 257; //map (ps2x.Analog (PSS_RX), 0, 255, 0, 65535);
    int16_t rightY = ps2x.Analog (PSS_RY) * 257; //map (ps2x.Analog (PSS_RY), 0, 255, 0, 65535);

    SendInput (buttons, leftX, leftY, rightX, rightY);
}

int16_t PS2Controller::GetButtonsBitfield () {
    int16_t buttons = 0;

    if (ps2x.Button (PSB_TRIANGLE )) buttons |= (1 << BTN_Button1);
    if (ps2x.Button (PSB_CIRCLE   )) buttons |= (1 << BTN_Button2);
    if (ps2x.Button (PSB_CROSS    )) buttons |= (1 << BTN_Button3);
    if (ps2x.Button (PSB_SQUARE   )) buttons |= (1 << BTN_Button4);
    if (ps2x.Button (PSB_L1       )) buttons |= (1 << BTN_L1);
    if (ps2x.Button (PSB_R1       )) buttons |= (1 << BTN_R1);
    if (ps2x.Button (PSB_L2       )) buttons |= (1 << BTN_L2);
    if (ps2x.Button (PSB_R2       )) buttons |= (1 << BTN_R2);
    if (ps2x.Button (PSB_L3       )) buttons |= (1 << BTN_L3);
    if (ps2x.Button (PSB_R3       )) buttons |= (1 << BTN_R3);
    if (ps2x.Button (PSB_START    )) buttons |= (1 << BTN_Start);
    if (ps2x.Button (PSB_SELECT   )) buttons |= (1 << BTN_Select);
    if (ps2x.Button (PSB_PAD_UP   )) buttons |= (1 << BTN_POV_Up);
    if (ps2x.Button (PSB_PAD_DOWN )) buttons |= (1 << BTN_POV_Down);
    if (ps2x.Button (PSB_PAD_LEFT )) buttons |= (1 << BTN_POV_Left);
    if (ps2x.Button (PSB_PAD_RIGHT)) buttons |= (1 << BTN_POV_Right);

    return buttons;
}
