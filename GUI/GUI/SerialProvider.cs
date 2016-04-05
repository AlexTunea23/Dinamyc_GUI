using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI
{
    public class SerialProvider
    {

        SerialPort mySerialPort=new SerialPort();
        List<string> receivedData = new List<string>();
        List<string> receiveAxis = new List<string>();
      
        

        public SerialProvider(string portName, int baudRate)
        {
            PortNameBaud(portName,baudRate);
            SerialInit();
        }


        public void PortNameBaud(string port,int baud)
        {
            mySerialPort.PortName = port;
            mySerialPort.BaudRate = baud;
        }

        
        public void SerialInit()
        {
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;
        }

        public void SerialReceive()
        {
            mySerialPort.DataReceived += DataReceivedHandler;
        }

        public void SerialOpen()
        {
            mySerialPort.Open();
        }

        public void SerialClose()
        {
            mySerialPort.Close();
        }


        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort mySerialPort = (SerialPort)sender;
                receivedData.Add(mySerialPort.ReadLine());
            }
            catch
            {
            }
        }


        public string GetAxisName()
        {
            string name = receivedData.First();
            receivedData.Remove(name);
            return name;
        }



        public string GetFirstDataBuff()
        {
            if (receivedData.Any())
            {
                string data = receivedData.First();
                receivedData.Remove(data);
                data = data.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");

                return data;
            }
            return null;
        }



        public void SerialSend(string data)
        {
            mySerialPort.Open();
            if (mySerialPort.IsOpen)
            {

                // Send the binary data out the port
                byte[] hexstring = Encoding.ASCII.GetBytes(data);
                
                foreach (byte hexval in hexstring)
                {
                    byte[] _hexval = new byte[] { hexval }; // need to convert byte to byte[] to write
                    mySerialPort.Write(_hexval, 0, 1);
                    //Thread.Sleep(1);
                }
            }
        }

    }
}
