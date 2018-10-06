#include "N64Interface.h"

#include <Arduino.h>
#include <pins_arduino.h>

#define NOP asm volatile ("nop")
#define NOP5 asm volatile ("nop\nnop\nnop\nnop\nnop\n")
#define NOP30 asm volatile ("nop\nnop\nnop\nnop\nnop\n" \
                              "nop\nnop\nnop\nnop\nnop\n" \
                              "nop\nnop\nnop\nnop\nnop\n" \
                              "nop\nnop\nnop\nnop\nnop\n" \
                              "nop\nnop\nnop\nnop\nnop\n" \
                              "nop\nnop\nnop\nnop\nnop\n")
#define DELAY_CYCLES(n) __builtin_avr_delay_cycles(n)

// these two macros set arduino pin 2 to input or output, which with an
// external 1K pull-up resistor to the 3.3V rail, is like pulling it high or
// low.  These operations translate to 1 op code, which takes 2 cycles

#define PORTLETTER A
    #include "N64InterfaceDef.cpp"
#undef PORTLETTER

#define PORTLETTER B
    #include "N64InterfaceDef.cpp"
#undef PORTLETTER

#define PORTLETTER C
    #include "N64InterfaceDef.cpp"
#undef PORTLETTER

#define PORTLETTER D
    #include "N64InterfaceDef.cpp"
#undef PORTLETTER

#define PORTLETTER E
    #include "N64InterfaceDef.cpp"
#undef PORTLETTER

#define PORTLETTER G
    #include "N64InterfaceDef.cpp"
#undef PORTLETTER

#define PORTLETTER H
    #include "N64InterfaceDef.cpp"
#undef PORTLETTER

#define PORTLETTER J
    #include "N64InterfaceDef.cpp"
#undef PORTLETTER

#define PORTLETTER L
    #include "N64InterfaceDef.cpp"
#undef PORTLETTER