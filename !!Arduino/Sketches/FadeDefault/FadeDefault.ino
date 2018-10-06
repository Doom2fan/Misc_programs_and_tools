int led = LED_BUILTIN; // the PWM pin the LED is attached to
int brightness = 0;    // how bright the LED is
int fadeAmount = 10;    // how many points to fade the LED by

// The setup routine runs once when you press reset:
void setup() {
  // Declare pin to be an output:
  pinMode(led, OUTPUT);
}

// The loop routine runs over and over again forever:
void loop() {
  // Set the brightness of pin 9:
  analogWrite(led, brightness);

  // Change the brightness for next time through the loop:
  brightness = constrain (brightness + fadeAmount, 0, 255);;

  // Reverse the direction of the fading at the ends of the fade:
  if (brightness <= 0 || brightness >= 255)
    fadeAmount = -fadeAmount;

  // Wait for 30 milliseconds to see the dimming effect
  delay (30);
}
