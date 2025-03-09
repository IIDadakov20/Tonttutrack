#include <SPI.h>
#include <WiFi.h>
#include <MQTT.h>
#include <TinyGPS++.h>
#include <SoftwareSerial.h>
#include <Arduino.h>

#define GPS_RX 14
#define GPS_TX 12

SoftwareSerial gpsSerial(GPS_RX, GPS_TX);
TinyGPSPlus gps;

const char ssid[] = "";
const char pass[] = "";

WiFiClient net;
MQTTClient client;

unsigned long lastMillis = 0;
String mac;
String speed;
String latitude;
String longitude;

void connect()
{
    while (WiFi.status() != WL_CONNECTED) {
        delay(1000);
    }

    while (!client.connect(mac.c_str(), "public", "public")) {
        delay(1000);
    }
}

void setup()
{
    gpsSerial.begin(9600);

    WiFi.mode(WIFI_STA);
    WiFi.begin(ssid, pass);
    mac = WiFi.macAddress();
    client.begin("", 1883, net);

    connect();
}

void loop()
{
    client.loop();

    if (!client.connected()) {
        connect();
    }

    while (gpsSerial.available())
    {
        char data = gpsSerial.read();
        if (data) {
            gps.encode(data);
            latitude = String(gps.location.lat(), 6);
            longitude = String(gps.location.lng(), 6);
            speed = String(gps.speed.kmph());

            if (millis() - lastMillis >= 1000) {
                lastMillis = millis();
                client.publish("statistics/" + mac + "/speed", speed);
                client.publish("statistics/" + mac + "/latitude", latitude);
                client.publish("statistics/" + mac + "/longitude", longitude);
            }
        }
    }
}