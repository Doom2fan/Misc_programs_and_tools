#include "Controller.h"
#include "N64Gamepad.h"

N64Gamepad *N64Gamepad::Create (int dataPin) {
    N64Gamepad *ctrl = new N64Gamepad ();

    ctrl->n64Ctrl = new N64Controller (dataPin);
    ctrl->n64Ctrl->begin ();
    delay (150);

    return ctrl;
}

const char Ctrl_Name [] = "Nintendo 64 controller";
void N64Gamepad::DoSendInfo () {
    // Send our info
    SendInfo (&Ctrl_Name [0], NULL);
}

void N64Gamepad::SetRumble (byte m1, byte m2) { }

void N64Gamepad::DoSendInput () {
    n64Ctrl->update ();

    int16_t buttons = GetButtonsBitfield ();
    int16_t leftX  = map (n64Ctrl->axis_x (), -128, 127, 0, 65535);
    int16_t leftY  = map (n64Ctrl->axis_y (), -128, 127, 0, 65535);

    SendInput (buttons, leftX, leftY, 32767, 32767);
}

int16_t N64Gamepad::GetButtonsBitfield () {
    int16_t buttons = 0;

    if (n64Ctrl->B       ()) buttons |= (1 << BTN_Button1);
    if (n64Ctrl->A       ()) buttons |= (1 << BTN_Button2);
    if (n64Ctrl->Z       ()) buttons |= (1 << BTN_Button3);
    if (n64Ctrl->L       ()) buttons |= (1 << BTN_L1);
    if (n64Ctrl->R       ()) buttons |= (1 << BTN_R1);
    if (n64Ctrl->C_up    ()) buttons |= (1 << BTN_L2);
    if (n64Ctrl->C_down  ()) buttons |= (1 << BTN_R2);
    if (n64Ctrl->C_left  ()) buttons |= (1 << BTN_L3);
    if (n64Ctrl->C_right ()) buttons |= (1 << BTN_R3);
    if (n64Ctrl->Start   ()) buttons |= (1 << BTN_Start);
    if (n64Ctrl->D_up    ()) buttons |= (1 << BTN_POV_Up);
    if (n64Ctrl->D_down  ()) buttons |= (1 << BTN_POV_Down);
    if (n64Ctrl->D_left  ()) buttons |= (1 << BTN_POV_Left);
    if (n64Ctrl->D_right ()) buttons |= (1 << BTN_POV_Right);
if (n64Ctrl->B ())    Serial.println ("DR");

    return buttons;
}
