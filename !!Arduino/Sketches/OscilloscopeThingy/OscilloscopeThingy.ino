#include <Adafruit_GFX.h>    // Core graphics library
#include <TouchScreen.h>
#include <ButtonDebounce.h>

#if defined(__SAM3X8E__)
#undef __FlashStringHelper::F(string_literal)
#define F(string_literal) string_literal
#endif

/*
    #define YP 9  // must be an analog pin, use "An" notation!
    #define XM 8  // must be an analog pin, use "An" notation!
    #define YM A2   // can be a digital pin
    #define XP A3   // can be a digital pin
*/

#define YP A2  // must be an analog pin, use "An" notation!
#define XM A3  // must be an analog pin, use "An" notation!
#define YM 8   // can be a digital pin
#define XP 9   // can be a digital pin

#define TS_MINX 130
#define TS_MAXX 905

#define TS_MINY 75
#define TS_MAXY 930

TouchScreen ts = TouchScreen(XP, YP, XM, YM, 300);

#define LCD_CS A3
#define LCD_CD A2
#define LCD_WR A1
#define LCD_RD A0
// optional
#define LCD_RESET A4

// Assign human-readable names to some common 16-bit color values:
#define BLACK   0x0000
#define BLUE    0x001F
#define RED     0xF800
#define GREEN   0x07E0
#define CYAN    0x07FF
#define MAGENTA 0xF81F
#define YELLOW  0xFFE0
#define WHITE   0xFFFF
int16_t top = 0, width, lines, scroll = 0;

#include <MCUFRIEND_kbv.h>
MCUFRIEND_kbv tft;

ButtonDebounce debounce = ButtonDebounce (22, 5);

void setup(void) {
    Serial.begin(9600);

    tft.reset();

    uint16_t identifier = tft.readID();
    if (identifier == 0x0101)
        identifier = 0x9341;
    else {
        /*  Serial.print(F("Unknown LCD driver chip: "));
            Serial.println(identifier, HEX);
            Serial.println(F("If using the Adafruit 2.8\" TFT Arduino shield, the line:"));
            Serial.println(F("  #define USE_ADAFRUIT_SHIELD_PINOUT"));
            Serial.println(F("should appear in the library header (Adafruit_TFT.h)."));
            Serial.println(F("If using the breakout board, it should NOT be #defined!"));
            Serial.println(F("Also if using the breakout, double-check that all wiring"));
            Serial.println(F("matches the tutorial."));*/
        identifier = 0x9486;
    }

    tft.begin(identifier);
    //Serial.print("TFT size is "); Serial.print(tft.width()); Serial.print("x"); Serial.println(tft.height());
    tft.setRotation(0);

    tft.fillScreen(BLACK);

    lines = tft.height ();
    width = tft.width ();

    if (identifier == 0x7783) {
        tft.println("can NOT scroll");
        while (1);                    // die.
    }

    pinMode (A15, INPUT);
    pinMode (A14, INPUT);
    pinMode (A13, INPUT);
    pinMode (22, INPUT_PULLUP);
    debounce.setCallback (onButtonPressed);
}

#define MINPRESSURE 10
#define MAXPRESSURE 1000
int timer = 0;
float prevOscVal = 0.0;
bool drawDots = false;
void loop()
{
    float oscVal = analogRead (A15) / 1023.;

    if (!drawDots)
        tft.drawLine ((int) (prevOscVal * width), scroll - 2, (int) (oscVal * width), scroll - 1, RED);
    else
        tft.drawPixel ((int) (oscVal * width), scroll - 1, RED);
    tft.fillRect (0, scroll + 1, width, 1, BLACK);

    prevOscVal = oscVal;

    if (++scroll >= lines)
        scroll = 0;
    tft.vertScroll (top, tft.height (), scroll);

    debounce.update ();

    int delayTime = ((analogRead (A14) / 1023.) * (analogRead (A13) / 1023.)) * 1000000;

    delayMicroseconds (delayTime);
}

void onButtonPressed (int state) {
    if (state == 0)
        drawDots = !drawDots;
}
