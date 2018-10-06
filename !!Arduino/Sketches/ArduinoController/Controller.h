#pragma once
#include <PS2X_lib.h>
#include "Protocol.h"

class Controller {
    protected:
        void WriteInt16ToBuffer (int16_t val, byte *buf, size_t offs);
        void SendInput (int16_t buttons, int16_t leftX, int16_t leftY, int16_t rightX, int16_t rightY);
        virtual void SetRumble (byte m1, byte m2) = 0;
        void SendInfo (char *ctrlName, char *err);

        virtual void DoSendInput () = 0;
        virtual void DoSendInfo () = 0;

    public:
        virtual void Update ();
};
