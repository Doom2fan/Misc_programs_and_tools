#pragma once
#include "Protocol.h"
#include "Controller.h"

class RaspiHandheldController : public Controller {
    protected:
        static const int AnalogLeftX = A1;
        static const int AnalogLeftY = A0;
        static const int AnalogLeftXMin = 141, AnalogLeftXMax = 884;
        static const int AnalogLeftYMin = 146, AnalogLeftYMax = 844;
        virtual void DoSendInput ();
        int16_t GetButtonsBitfield ();

    public:
        static RaspiHandheldController *Create ();
        virtual void Update ();
};
