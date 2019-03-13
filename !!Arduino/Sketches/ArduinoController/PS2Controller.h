#pragma once
#include <PS2X_lib.h>
#include "Protocol.h"
#include "Controller.h"

class PS2Controller : public Controller {
    protected:
        PS2X ps2x;
        int error;
        int type;
        bool motor1 = false;
        byte motor2 = 0;
        
        virtual void DoSendInput ();
        virtual void DoSendInfo ();
        virtual void SetRumble (byte m1, byte m2);
        int16_t GetButtonsBitfield ();

    public:
        virtual void Update ();
        static PS2Controller *Create (int clkPin, int cmdPin, int selPin, int dataPin, bool pressures, bool rumble);
};
