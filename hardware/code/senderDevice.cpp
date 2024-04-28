#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <SPI.h>
#include <TinyGPS++.h>
#include <SoftwareSerial.h>

#define SCREEN_WIDTH 128
#define SCREEN_HEIGHT 64

#define OLED_SDA 8
#define OLED_SCL 9

#define GPS_RX 14
#define GPS_TX 12

// Declaration for an SSD1306 display connected to I2C (SDA, SCL pins)
#define OLED_RESET -1
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);

SoftwareSerial gpsSerial(GPS_RX, GPS_TX);
TinyGPSPlus gps;

void writeTextBasic(String text)
{
    display.clearDisplay();

    display.setTextSize(1);
    display.setTextColor(SSD1306_WHITE);
    display.setCursor(0, 0);
    display.cp437();

    display.write(text.c_str());
    display.display();
}


void setup()
{
    Wire.begin(OLED_SDA, OLED_SCL);

    // Initialize display
    if (!display.begin(SSD1306_SWITCHCAPVCC, 0x3C)) { // Address 0x3C for 128x64
        Serial.println(F("SSD1306 allocation failed"));
        for (;;);
    }

    // Clear the buffer
    display.clearDisplay();

    gpsSerial.begin(9600);

    writeTextBasic("test");
}

void loop()
{
    String displayOutput = "";

    while (gpsSerial.available())
    {
        if (gps.encode(gpsSerial.read()))
        {
            displayOutput += "SATS: " + String(gps.satellites.value()) + "\n";
            displayOutput += "LAT: " + String(gps.location.lat(), 6) + "\n";
            displayOutput += "LON: " + String(gps.location.lng(), 6) + "\n";
            displayOutput += "ALT: " + String(gps.altitude.meters()) + " m\n";
            displayOutput += "SPEED: " + String(gps.speed.kmph()) + " km/h\n";
            displayOutput += "DATE: " + String(gps.date.day()) + "/" + String(gps.date.month()) + "/" + String(gps.date.year()) + "\n";
            displayOutput += "TIME: " + String(gps.time.hour()) + ":" + String(gps.time.minute()) + ":" + String(gps.time.second()) + "\n";

            writeTextBasic(displayOutput);
        }
    }
}