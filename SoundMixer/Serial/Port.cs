using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;

namespace SoundMixer.Serial
{
    class Port
    {
        private SerialPort port;

        public delegate void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e);
        public event DataReceivedHandler OnData;

        public Port()
        {
            this.port = new SerialPort();
            this.port.DataReceived += this.PassData;
        }

        public IList<String> AvailablePorts() { return SerialPort.GetPortNames(); }

        private void PassData(object sender, SerialDataReceivedEventArgs e)
        {
            OnData(sender, e);
        }

        public bool Open(String port)
        {
            if (this.port.IsOpen && this.port.PortName == port)
            {
                return true;
            }
            if(port == null)
            {
                return false;
            }
            this.Close();

            this.port.PortName = port;
            this.port.BaudRate = 9600;
            try
            {
                this.port.Open();
            }
            catch(Exception e) when (e is UnauthorizedAccessException || e is IOException)
            {
                return false;
            }
            return true;
        }

        public void Close()
        {
            if (this.port.IsOpen)
            {
                this.port.Close();
            }
        }
    }
}
