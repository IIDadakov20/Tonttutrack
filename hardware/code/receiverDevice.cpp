#include <esp_now.h>
#include <WiFi.h>
#include <Arduino.h>

struct MessageData {
    String lat;
    String lon;
    String speed;
};

String authorizationCode = "123456";
String receivedMessage = "";

MessageData receivedData;

void onDataReceived(const uint8_t* mac, const uint8_t* incomingData, int len)
{
    memcpy(&receivedData, incomingData, sizeof(receivedData));
    delay(1000);
    Serial.println(receivedData.lat);
    Serial.println(receivedData.lon);
    Serial.println(receivedData.speed);
}

void setup()
{
    Serial.begin(115200);

    WiFi.mode(WIFI_STA);

    ESP_ERROR_CHECK(esp_now_init());
}

void loop()
{
    if (Serial.available() > 0)
    {
        receivedMessage = Serial.readString();
    }

    if (receivedMessage == "Authentication code required")
    {
        Serial.println(authorizationCode);
        receivedMessage.clear();
        ESP_ERROR_CHECK(esp_now_register_recv_cb(onDataReceived));
    }
}