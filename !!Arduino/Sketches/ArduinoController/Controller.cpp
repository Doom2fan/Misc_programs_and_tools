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
            /*case HC_SetRumble:
                while (Serial.available () < 2);
                SetRumble (Serial.read (), Serial.read ());
                break;
            case HC_InfoRequest:
                DoSendInfo ();
                break;*/
        }
    }
}

void Controller::SendInput (int16_t buttons, int16_t leftX, int16_t leftY, int16_t rightX, int16_t rightY) {
    byte inputBuffer [5 * 2 + 4];
    inputBuffer [0] = 'C';
    inputBuffer [1] = 'T';
    inputBuffer [2] = 'R';
    inputBuffer [3] = 'L';
    WriteInt16ToBuffer (buttons, inputBuffer, 4);
    WriteInt16ToBuffer (leftX, inputBuffer,   4 + 2);
    WriteInt16ToBuffer (leftY, inputBuffer,   4 + 2 * 2);
    WriteInt16ToBuffer (rightX, inputBuffer,  4 + 2 * 3);
    WriteInt16ToBuffer (rightY, inputBuffer,  4 + 2 * 4);

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
