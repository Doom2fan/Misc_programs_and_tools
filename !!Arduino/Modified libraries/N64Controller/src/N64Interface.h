#ifndef N64Interface_h
#define N64Interface_h

class N64Interface {
public:
    virtual void init();
    virtual void send(unsigned char * buffer, char length);
    virtual void get();

    char raw_dump[33];

protected:
    N64Interface(unsigned char pincode) : pincode(pincode) {};
    unsigned char pincode;
};

#define PORTLETTER A
    #include "N64InterfaceDef.h"
#undef PORTLETTER

#define PORTLETTER B
    #include "N64InterfaceDef.h"
#undef PORTLETTER

#define PORTLETTER C
    #include "N64InterfaceDef.h"
#undef PORTLETTER

#define PORTLETTER D
    #include "N64InterfaceDef.h"
#undef PORTLETTER

#define PORTLETTER E
    #include "N64InterfaceDef.h"
#undef PORTLETTER

#define PORTLETTER G
    #include "N64InterfaceDef.h"
#undef PORTLETTER

#define PORTLETTER H
    #include "N64InterfaceDef.h"
#undef PORTLETTER

#define PORTLETTER J
    #include "N64InterfaceDef.h"
#undef PORTLETTER

#define PORTLETTER L
    #include "N64InterfaceDef.h"
#undef PORTLETTER

#endif