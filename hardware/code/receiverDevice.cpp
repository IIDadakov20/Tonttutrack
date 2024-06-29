#include <esp_now.h>
#include <WiFi.h>
#include <Arduino.h>

struct MessageData {
    String lat;
    String lon;
    String speed;
};

String authorizationCode = "E8:6B:EA:E0:0A:A4";
String receivedMessage = "";

MessageData receivedData;

void onDataReceived(const uint8_t* mac, const uint8_t* incomingData, int len)
{
    memcpy(&receivedData, incomingData, sizeof(receivedData));
    Serial.println(receivedData.lat);
    Serial.println(receivedData.lon);
    Serial.println(receivedData.speed);
    delay(5000);
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

    if (receivedMessage == "break")
    {
        ESP_ERROR_CHECK(esp_now_unregister_recv_cb());
        Serial.println("break");
        delay(1000);
        ESP.restart();
    }
}