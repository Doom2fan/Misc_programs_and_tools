#include "N64Controller.h"
#include <Arduino.h>

unsigned char GetPinCode (unsigned char serialPin);

N64Controller::N64Controller(unsigned char serialPin) {
  /*if(serialPin > 13)
    serialPin = 2;*/
  // Communication with N64 controller controller on this pin
  // Don't remove these lines, we don't want to push +5V to the controller
  digitalWrite(serialPin, LOW);
  pinMode(serialPin, INPUT_PULLUP);

  if (serialPin >= 22 && serialPin <= 29) {
    interface = new N64Interface_PINA (GetPinCode (serialPin));
  } else if ((serialPin >= 10 && serialPin <= 13) || (serialPin >= 50 && serialPin <= 53)) {
    interface = new N64Interface_PINB (GetPinCode (serialPin));
  } else if (serialPin >= 30 && serialPin <= 37) {
    interface = new N64Interface_PINC (GetPinCode (serialPin));
  } else if (serialPin == 38 || (serialPin >= 18 && serialPin <= 21)) {
    interface = new N64Interface_PIND (GetPinCode (serialPin));
  } else if (serialPin == 5 || (serialPin >= 0 && serialPin <= 3)) {
    interface = new N64Interface_PINE (GetPinCode (serialPin));
  } else if (serialPin == 4 || (serialPin >= 39 && serialPin <= 41)) {
    interface = new N64Interface_PING (GetPinCode (serialPin));
  } else if ((serialPin >= 6 && serialPin <= 9) || (serialPin >= 17 && serialPin <= 16)) {
    interface = new N64Interface_PINH (GetPinCode (serialPin));
  } else if (serialPin >= 14 && serialPin <= 15) {
    interface = new N64Interface_PINJ (GetPinCode (serialPin));
  } else if (serialPin >= 42 && serialPin <= 49) {
    interface = new N64Interface_PINL (GetPinCode (serialPin));
  }
}

unsigned char GetPinCode (unsigned char serialPin) {
  switch (serialPin) {
    case  0: case 15: case 17:
    case 21: case 22: case 37:
    case 41: case 49: case 53:
      return 0;
    case  1: case 14: case 16:
    case 20: case 23: case 36:
    case 40: case 48: case 52:
      return 1;
    case 19: case 24: case 35:
    case 39: case 47: case 51:
      return 2;
    case  5: case  6: case 18:
    case 25: case 34: case 46:
    case 50:
      return 3;
    case  2: case  7: case 10:
    case 26: case 33: case 45:
      return 4;
    case  3: case  4: case 8:
    case 11: case 27: case 32:
    case 44:
      return 5;
    case  9: case 12: case 28:
    case 31: case 43:
      return 6;
    case 13: case 29: case 30:
    case 38: case 42:
      return 7;
  }
}

void N64Controller::begin() {
    interface->init();
}

void N64Controller::print_N64_status()
{
    // bits: A, B, Z, Start, Dup, Ddown, Dleft, Dright
    // bits: 0, 0, L, R, Cup, Cdown, Cleft, Cright
    Serial.println();
    Serial.print("Start: ");
    Serial.println(Start());

    Serial.print("Z:     ");
    Serial.println(Z());

    Serial.print("B:     ");
    Serial.println(B());

    Serial.print("A:     ");
    Serial.println(A());

    Serial.print("L:     ");
    Serial.println(L());
    Serial.print("R:     ");
    Serial.println(R());

    Serial.print("Cup:   ");
    Serial.println(C_up());
    Serial.print("Cdown: ");
    Serial.println(C_down());
    Serial.print("Cright:");
    Serial.println(C_right());
    Serial.print("Cleft: ");
    Serial.println(C_left());
    
    Serial.print("Dup:   ");
    Serial.println(D_up());
    Serial.print("Ddown: ");
    Serial.println(D_down());
    Serial.print("Dright:");
    Serial.println(D_right());
    Serial.print("Dleft: ");
    Serial.println(D_left());

    Serial.print("Stick X:");
    Serial.println(axis_x(), DEC);
    Serial.print("Stick Y:");
    Serial.println(axis_y(), DEC);
}

void N64Controller::update() {
  unsigned char command[] = {0x01};
  noInterrupts();
  interface->send(command, 1);
  interface->get();
  interrupts();
}
