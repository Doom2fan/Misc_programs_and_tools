#pragma once
#include "Protocol.h"
#include "Controller.h"

/* Input ranges:
 *  Throttle: [40-200] (brown-white)
 *  Stick X [40-170] (green-white)
 *  Stick Y [40-170] (green)
 *  L-R potentiometer thing: [15-190] (brown)
 */

/*class RCHeliController : public Controller {
    protected:
        virtual void DoSendInput ();
        virtual void DoSendInfo ();
        virtual void SetRumble (byte m1, byte m2);
        int16_t GetButtonsBitfield ();

    public:
        static RCHeliController *Create (int dataPin);
};*/
