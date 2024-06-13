#ifndef UTILS_H
#define UTILS_H

#include <WiFiEsp.h>
#include <WiFiEspServer.h>
#include <WiFiEspClient.h>

float readTemperature(int pin);
void sendResponse(WiFiEspClient client, int bpm, float temperatureC);

#endif
