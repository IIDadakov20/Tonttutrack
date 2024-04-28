#include <esp_now.h>
#include <WiFi.h>

typedef struct structMessage {
	String lat;
	String lon;
	String speed;
} structMessage;

structMessage receivedMessage;

void OnDataReceived(const uint8_t* mac, const uint8_t* incomingData, int len)
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

	esp_now_init();
	esp_now_register_recv_cb(OnDataReceived);
}

void loop()
{

}