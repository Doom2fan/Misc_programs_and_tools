#include "Controller.h"

void Controller::WriteInt16ToBuffer (int16_t val, byte *buf, size_t offs) {
    buf [offs  ] = (byte) ((val >> 8) & 0xFF);
    buf [offs+1] = (byte) ((val     ) & 0xFF);
}

void Controller::Update () {
    if (Serial.available () > 0) {
        int code = Serial.read ();
        switch (code) {
            case HC_PollInput:
                DoSendInput ();
                break;
            case HC_SetRumble:
                while (Serial.available () < 2);
                SetRumble (Serial.read (), Serial.read ());
                break;
            case HC_InfoRequest:
                DoSendInfo ();
                break;
        }
    }
}

void Controller::SendInput (int16_t buttons, int16_t leftX, int16_t leftY, int16_t rightX, int16_t rightY) {
    byte inputBuffer [5 * 2 + 1];
    inputBuffer [0] = SC_Input;
    WriteInt16ToBuffer (buttons, inputBuffer, 1);
    WriteInt16ToBuffer (leftX, inputBuffer, 3);
    WriteInt16ToBuffer (leftY, inputBuffer, 5);
    WriteInt16ToBuffer (rightX, inputBuffer, 7);
    WriteInt16ToBuffer (rightY, inputBuffer, 9);

    Serial.write (inputBuffer, sizeof (inputBuffer));
}

void Controller::SendInfo (char *ctrlName, char *err) {
    // Wait until we can send data
    while (!Serial.availableForWrite ());

    Serial.write (SC_Info);
    Serial.write (ctrlName);
    Serial.write ("\n");
    if (err != nullptr)
        Serial.write (err);
    Serial.write ("\n");
}
