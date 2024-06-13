#include "utils.h"

float readTemperature(int pin) {
  int tempValue = analogRead(pin);
  float millivolts = (tempValue / 1024.0) * 5000;
  return millivolts / 10;
}

void sendResponse(WiFiEspClient client, int bpm, float temperatureC) {
  client.println("HTTP/1.1 200 OK");
  client.println("Content-type:text/html");
  client.println();

  client.print("BPM: ");
  client.print(bpm);
  client.print("<br>Temperature: ");
  client.print(temperatureC);
  client.print(" C");
  client.println();
}
