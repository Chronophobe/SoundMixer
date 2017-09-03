int currentValues[] = {-100, -100, -100, -100};
int pins[] = {A1, A2, A3, A4};

void setup() {
    Serial.begin(9600);
    pinMode(5, OUTPUT);
}

void loop() { 
  for(int i = 0; i < 1; i++){
    int val = analogRead(pins[i]);
    int difference = val - currentValues[i];
    if(abs(difference) > 2){
      digitalWrite(5, HIGH);
      Serial.print(i);
      Serial.write(":");
      Serial.println(val);
      currentValues[i] = val;
    }
  }
  delay(100);
  digitalWrite(5, LOW);
}

