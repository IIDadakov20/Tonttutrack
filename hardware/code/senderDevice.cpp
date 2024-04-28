#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <SPI.h>
#include <TinyGPS++.h>
#include <SoftwareSerial.h>
#include <esp_now.h>
#include <WiFi.h>

#define SCREEN_WIDTH 128
#define SCREEN_HEIGHT 64

#define OLED_SDA 8
#define OLED_SCL 9

#define GPS_RX 14
#define GPS_TX 12

uint8_t broadcastAddress[] = { 0xE8, 0x6B, 0xEA, 0xE0, 0x0A, 0xA4 };

#define OLED_RESET -1
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);

SoftwareSerial gpsSerial(GPS_RX, GPS_TX);
TinyGPSPlus gps;

esp_now_peer_info_t peerInfo;

typedef struct struct_message {
    String lat;
    String lon;
    String speed;
} struct_message;

struct_message myData;

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

    display.begin(SSD1306_SWITCHCAPVCC, 0x3C);
    display.clearDisplay();

    gpsSerial.begin(9600);

    writeTextBasic("test");

    Serial.begin(115200);
    WiFi.mode(WIFI_STA);

    esp_now_init();

    memcpy(peerInfo.peer_addr, broadcastAddress, 6);
    peerInfo.channel = 0;
    peerInfo.encrypt = false;

    esp_now_add_peer(&peerInfo);
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

            myData.lat = String(gps.location.lat(), 6);
            myData.lon = String(gps.location.lng(), 6);
            myData.speed = String(gps.speed.kmph());
            esp_err_t result = esp_now_send(broadcastAddress, (uint8_t*)&myData, sizeof(myData));
        }
    }
}