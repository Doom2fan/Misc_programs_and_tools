#include <Arduino.h>
#include "Controller.h"

void Controller::WriteInt16ToBuffer (int16_t val, uint8_t *buf, size_t offs) {
    buf [offs  ] = (uint8_t) ((val >> 8) & 0xFF);
    buf [offs+1] = (uint8_t) ((val     ) & 0xFF);
}

void Controller::Update () {
    if (Serial.available () > 0) {
        int code = Serial.read ();

        switch (code) {
            case HC_PollInput:
                DoSendInput ();
                break;
        }
    }
}

void Controller::SendInput (int16_t buttons, uint8_t leftX, uint8_t leftY, uint8_t rightX, uint8_t rightY) {
    uint8_t inputBuffer [10];

    inputBuffer [0] = 'C';
    inputBuffer [1] = 'T';
    inputBuffer [2] = 'R';
    inputBuffer [3] = 'L';
    WriteInt16ToBuffer (buttons, inputBuffer, 4);
    inputBuffer [6] = leftX;
    inputBuffer [7] = leftY;
    inputBuffer [8] = rightX;
    inputBuffer [9] = rightY;

    Serial.write (inputBuffer, sizeof (inputBuffer));
}
