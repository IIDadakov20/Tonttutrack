#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <TinyGPS++.h>
#include <SoftwareSerial.h>
#include <esp_now.h>
#include <WiFi.h>
#include <Arduino.h>

#define SCREEN_WIDTH 128
#define SCREEN_HEIGHT 64
#define OLED_SDA 8
#define OLED_SCL 9
#define OLED_RESET -1

#define GPS_RX 14
#define GPS_TX 12

struct MessageData {
    String lat;
    String lon;
    String speed;
};

Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);

SoftwareSerial gpsSerial(GPS_RX, GPS_TX);
TinyGPSPlus gps;

esp_now_peer_info_t peerInfo;

const uint8_t broadcastAddress[] = { 0xE8, 0x6B, 0xEA, 0xE0, 0x0A, 0xA4 };

MessageData message;

void displayData(String text)
{
    display.clearDisplay();
    display.setTextSize(1);
    display.setTextColor(SSD1306_WHITE);
    display.setCursor(0, 0);
    display.write(text.c_str());
    display.display();
}

void setup()
{
    Wire.begin(OLED_SDA, OLED_SCL);

    display.begin(SSD1306_SWITCHCAPVCC, 0x3C);
    display.clearDisplay();

    Serial.begin(115200);
    gpsSerial.begin(9600);

    WiFi.mode(WIFI_STA);

    ESP_ERROR_CHECK(esp_now_init());

    memcpy(peerInfo.peer_addr, broadcastAddress, 6);
    peerInfo.channel = 0;
    peerInfo.encrypt = false;

    ESP_ERROR_CHECK(esp_now_add_peer(&peerInfo));
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
            displayData(displayOutput);

            message.lat = String(gps.location.lat(), 6);
            message.lon = String(gps.location.lng(), 6);
            message.speed = String(gps.speed.kmph());
            ESP_ERROR_CHECK(esp_now_send(broadcastAddress, (uint8_t*)&message, sizeof(message)));
        }
    }
}