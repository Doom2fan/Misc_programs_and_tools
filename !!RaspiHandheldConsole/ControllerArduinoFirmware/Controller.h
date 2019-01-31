#pragma once
#include <stdint.h>
#include <stddef.h>
#include "Protocol.h"

class Controller {
    protected:
        void WriteInt16ToBuffer (int16_t val, uint8_t *buf, size_t offs);
        void SendInput (int16_t buttons, uint8_t leftX, uint8_t leftY, uint8_t rightX, uint8_t rightY);

        virtual void DoSendInput () = 0;

    public:
        virtual void Update ();
};
