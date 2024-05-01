#include <esp_now.h>
#include <WiFi.h>
#include <Arduino.h>

struct MessageData {
	String lat;
	String lon;
	String speed;
};

MessageData receivedMessage;

void onDataReceived(const uint8_t* mac, const uint8_t* incomingData, int len)
{
	memcpy(&receivedMessage, incomingData, sizeof(receivedMessage));
	Serial.println(receivedMessage.lat);
	Serial.println(receivedMessage.lon);
	Serial.println(receivedMessage.speed);
	delay(1000);
}

void setup()
{
	Serial.begin(115200);

	WiFi.mode(WIFI_STA);

	ESP_ERROR_CHECK(esp_now_init());
	ESP_ERROR_CHECK(esp_now_register_recv_cb(onDataReceived));
}

void loop()
{

}