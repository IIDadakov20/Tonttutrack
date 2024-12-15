#include <SPI.h>
#include <MQTT.h>
#include <WiFi.h>
#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <TinyGPS++.h>
#include <SoftwareSerial.h>
#include <Arduino.h>

#define SCREEN_WIDTH 128
#define SCREEN_HEIGHT 64
#define OLED_SDA 8
#define OLED_SCL 9
#define OLED_RESET -1

#define GPS_RX 14
#define GPS_TX 12

Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);

SoftwareSerial gpsSerial(GPS_RX, GPS_TX);
TinyGPSPlus gps;

const char ssid[] = "";
const char pass[] = "";

WiFiClient net;
MQTTClient client;

unsigned long lastMillis = 0;
String mac = WiFi.macAddress();

void connect()
{
    displayData("Checking wifi...");
    while (WiFi.status() != WL_CONNECTED) {
        delay(1000);
    }

    displayData("Connecting...");
    while (!client.connect("arduino", "public", "public")) {
        delay(1000);
    }

    displayData("Connected!");
}

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
    Wire.begin();
    display.begin(SSD1306_SWITCHCAPVCC, 0x3C);

    gpsSerial.begin(9600);

    WiFi.mode(WIFI_STA);
    WiFi.begin(ssid, pass);
    client.begin("192.168.0.106", 1883, net);

    connect();
}

void loop()
{
    client.loop();

    if (!client.connected()) {
        connect();
    }

    String displayOutput = "";
    while (gpsSerial.available())
    {
        if (gps.encode(gpsSerial.read()))
        {
            if (millis() - lastMillis > 1000) {
                lastMillis = millis();
                client.publish("car_statistics/" + mac + "/speed", String(gps.speed.kmph()));
                client.publish("car_statistics/" + mac + "/latitude", String(gps.location.lat(), 6));
                client.publish("car_statistics/" + mac + "/longitude", String(gps.location.lng(), 6));
            }

            displayOutput += "SATS: " + String(gps.satellites.value()) + "\n";
            displayOutput += "LAT: " + String(gps.location.lat(), 6) + "\n";
            displayOutput += "LON: " + String(gps.location.lng(), 6) + "\n";
            displayOutput += "ALT: " + String(gps.altitude.meters()) + " m\n";
            displayOutput += "SPEED: " + String(gps.speed.kmph()) + " km/h\n";
            displayOutput += "DATE: " + String(gps.date.day()) + "/" + String(gps.date.month()) + "/" + String(gps.date.year()) + "\n";
            displayOutput += "TIME: " + String(gps.time.hour()) + ":" + String(gps.time.minute()) + ":" + String(gps.time.second()) + "\n";
            displayData(displayOutput);
        }
    }
}