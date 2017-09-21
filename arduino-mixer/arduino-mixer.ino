int currentValues[] = {-100, -100, -100, -100, -100};
int pins[] = {A0, A1, A2, A3, A4};

void setup() {
    Serial.begin(9600);
}

void loop() { 
  for(int i = 0; i < 5; i++){
    int val = analogRead(pins[i]);
    int difference = val - currentValues[i];
    if(abs(difference) > 1){
      Serial.print(i);
      Serial.write(":");
      Serial.println(val);
      currentValues[i] = val;
    }
  }
  delay(100);
}

