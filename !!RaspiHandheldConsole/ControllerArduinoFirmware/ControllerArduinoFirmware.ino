#include "Controller.h"
#include "RaspiHandheldController.h"

Controller *ctrl;

void setup() {
    Serial.begin (115200);

    pinMode (LED_BUILTIN, OUTPUT);
    digitalWrite (LED_BUILTIN, LOW);

    ctrl = RaspiHandheldController::Create ();
}

void loop() {
    if (ctrl != nullptr)
        ctrl->Update ();
}
