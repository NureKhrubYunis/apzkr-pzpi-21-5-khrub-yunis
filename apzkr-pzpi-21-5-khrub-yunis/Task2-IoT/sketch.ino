#include <SoftwareSerial.h>
#include <PulseSensorPlayground.h>
#include "utils.h"

// Встановлення програмного серійного порту для ESP
SoftwareSerial espSerial(2, 3); // RX, TX

// Данні підключення до Wi-Fi
char ssid[] = "your_SSID";          
char pass[] = "your_PASSWORD";      

// Ініціалізація серверу на порту 80
WiFiEspServer server(80);

// Налаштування датчика серцебиття
const int PulseWire = A0;
const int LED13 = 13;
int Threshold = 550;
PulseSensorPlayground pulseSensor;

// Налаштування датчика температури
const int tempPin = A1;

void setup() {
  // Налаштування серійного з'єднання для виводу даних та програмного серійного порту для ESP
  Serial.begin(115200);
  espSerial.begin(9600);
  
  // Ініціалізація ESP
  WiFi.init(&espSerial);

  // Перевірка, чи підключений ESP до Arduino
  if (WiFi.status() == WL_NO_SHIELD) {
    Serial.println("ESP не виявлено");
    while (true);
  }

  // Підключення до Wi-Fi
  while (WiFi.status() != WL_CONNECTED) {
    Serial.print("Підключення до Wi-Fi...");
    WiFi.begin(ssid, pass);
    delay(5000);
  }

  // Виведення IP-адреси
  Serial.print("IP-адреса: ");
  Serial.println(WiFi.localIP());

  // Запуск серверу
  server.begin();

  // Ініціалізація датчика серцебиття
  pulseSensor.analogInput(PulseWire);
  pulseSensor.blinkOnPulse(LED13);
  pulseSensor.setThreshold(Threshold);
  pulseSensor.begin();

  // Ініціалізація датчика температури
  pinMode(tempPin, INPUT);
}

void loop() {
  int myBPM = pulseSensor.getBeatsPerMinute();
  float temperatureC = readTemperature(tempPin);

  // Перевірка наявності клієнта
  WiFiEspClient client = server.available();
  if (client) {
    Serial.println("Новий клієнт.");
    boolean currentLineIsBlank = true;
    while (client.connected()) {
      if (client.available()) {
        char c = client.read();
        Serial.write(c);
        if (c == '\n' && currentLineIsBlank) {
          sendResponse(client, myBPM, temperatureC);
          break;
        }
        if (c == '\n') {
          currentLineIsBlank = true;
        } else if (c != '\r') {
          currentLineIsBlank = false;
        }
      }
    }
    client.stop();
    Serial.println("Клієнт відключився.");
  }
  delay(1000);
}
