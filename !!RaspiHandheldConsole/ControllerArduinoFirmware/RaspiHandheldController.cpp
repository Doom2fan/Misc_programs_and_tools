#include <Arduino.h>
#include <Bounce2.h>
#include "RaspiHandheldController.h"

#define DoButton(pin, button) Bounce debouncer##button = Bounce ()
#include "ButtonList.h"
#undef DoButton

RaspiHandheldController *RaspiHandheldController::Create () {
    RaspiHandheldController *ctrl = new RaspiHandheldController ();

#define DoButton(pin, button) pinMode (pin, INPUT_PULLUP); debouncer##button.attach (pin); debouncer##button.interval (4)
#include "ButtonList.h"
#undef DoButton

    pinMode (AnalogLeftX, INPUT);
    pinMode (AnalogLeftY, INPUT);

    // No right analog
    //pinMode (A2, INPUT);
    //pinMode (A3, INPUT);

    return ctrl;
}

void RaspiHandheldController::DoSendInput () {
    int16_t buttons = GetButtonsBitfield ();
    uint8_t leftX  = map (analogRead (AnalogLeftX), AnalogLeftXMin, AnalogLeftXMax, 0, 255);
    uint8_t leftY  = map (analogRead (AnalogLeftY), AnalogLeftYMin, AnalogLeftYMax, 0, 255);
    // No right analog
    uint8_t rightX = 0; // analogRead (A2);
    uint8_t rightY = 0; // analogRead (A3);

    SendInput (buttons, leftX, leftY, rightX, rightY);
}

int16_t RaspiHandheldController::GetButtonsBitfield () {
    int16_t buttons = 0;

#define DoButton(pin, button) buttons |= ((debouncer##button.read () == LOW) ? (1 << button) : 0)
#include "ButtonList.h"
#undef DoButton

    return buttons;
}
int lowestX = 2000, lowestY = 2000, highestX = -500, highestY = -500;

void RaspiHandheldController::Update () {
#define DoButton(pin, button) debouncer##button.update ()
#include "ButtonList.h"
#undef DoButton

    Controller::Update ();
}
