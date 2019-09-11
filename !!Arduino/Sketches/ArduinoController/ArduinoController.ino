#include "Controller.h"
#include "PS2Controller.h"
#include "N64Gamepad.h"

/******************************************************************
    set pins connected to PS2 controller:
    replace pin numbers by the ones you use
 ******************************************************************/
/*#define PS2_DAT        22
#define PS2_CMD        23
#define PS2_SEL        24
#define PS2_CLK        25*/
#define PS2_DAT        7
#define PS2_CMD        6
#define PS2_SEL        5
#define PS2_CLK        4

#define N64_DAT        22

/******************************************************************
    select modes of PS2 controller:
     - pressures = analog reading of push-butttons
     - rumble    = motor rumbling
    uncomment 1 of the lines for each mode selection
 ******************************************************************/
//#define pressures   true
#define pressures   false
#define rumble      true
//#define rumble      false

//right now, the library does NOT support hot pluggable controllers, meaning
//you must always either restart your Arduino after you connect the controller,
//or call config_gamepad(pins) again after connecting the controller.
Controller *ctrl;

void setup() {
    Serial.begin (115200);

    pinMode (LED_BUILTIN, OUTPUT);
    digitalWrite (LED_BUILTIN, LOW);
    
    //pinMode (A0, INPUT);

    //if (true || analogRead (A0) > 25) {
        ctrl = PS2Controller::Create (PS2_CLK, PS2_CMD, PS2_SEL, PS2_DAT, pressures, rumble);
    //} else if (analogRead (A1) > 25) {
        //ctrl = N64Gamepad::Create (N64_DAT);
    /*} else {
        ctrl = nullptr;
    }*/
}

void loop() {
    if (ctrl != nullptr)
        ctrl->Update ();
}
