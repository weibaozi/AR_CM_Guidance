#include <SPI.h>
#include <WiFiNINA.h>
// #include <WiFiUdp.h>

// WiFi credentials
const char* ssid = "GJK";
const char* password = "jhu123456";

WiFiUDP udp;
unsigned int localPort = 2390; // Any local port that isn't in use
unsigned int broadcastPort = 50000; // The port listeners will be monitoring

void setup() {
  Serial.begin(9600); // Start serial communication
  while (!Serial); // Wait for serial port to connect

  // Connect to WiFi
  Serial.print("Connecting to ");
  Serial.println(ssid);
  while (WiFi.begin(ssid, password) != WL_CONNECTED) {
    Serial.println("Connecting to WiFi...");
    delay(1000);
  }

  Serial.println("Connected to WiFi");
  udp.begin(localPort);
}

void loop() {
  // int packetSize = udp.parsePacket();
  broadcastA0Reading();
  delay(10); // Wait for 2 seconds between broadcasts
}

void broadcastA0Reading() {
  int sensorValue = analogRead(A0); // Read the analog value from pin A0
  char valueStr[20]= "CMSignal  ";
  sprintf(valueStr,"CMSignal %d",sensorValue);
  // itoa(sensorValue, valueStr, 10); // Convert integer to string

  udp.beginPacket(IPAddress(255, 255, 255, 255), broadcastPort);// Begin a packet to the broadcast address
  udp.write(valueStr); // Add the sensor value to the packet
  udp.endPacket(); // Send the packet

  Serial.print("Broadcasted: ");
  Serial.println(valueStr); 
  // Serial.println(WiFi.localIP());
}
