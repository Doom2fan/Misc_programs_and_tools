#pragma once
#include <N64Controller.h>
#include "Protocol.h"
#include "Controller.h"

class N64Gamepad : public Controller {
    protected:
        N64Controller *n64Ctrl;

        virtual void DoSendInput ();
        virtual void DoSendInfo ();
        virtual void SetRumble (byte m1, byte m2);
        int16_t GetButtonsBitfield ();

    public:
        static N64Gamepad *Create (int dataPin);
};
