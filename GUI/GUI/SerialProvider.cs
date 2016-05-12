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
        public string serial;
       
      
        

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
            mySerialPort.DtrEnable = true;
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
                serial = mySerialPort.ReadLine();
                if(serial.Contains("$")&&serial.Contains("#")&&serial.Contains("\r"))
                receivedData.Add(serial);
            }
            catch
            {
            }
        }


        


        public string GetFirstDataBuff()
        {
            if (receivedData.Exists(t=>t.Contains("$"))&&receivedData.Exists(t=>t.Contains("#"))&&receivedData.Exists(t=>t.Contains("\r")))
            {
                string data = receivedData.First();
                string finaldata = data;
                receivedData.Remove(data);
                int firstChar = finaldata.LastIndexOf("$");
                int lastChar = finaldata.LastIndexOf("#");
                if (firstChar < lastChar)
                {
                    finaldata = finaldata.Substring(firstChar, lastChar-firstChar);
                    finaldata = finaldata.Replace("$", "").Replace("#", "").Replace("\r", "").Replace("\n", "");

                    return finaldata;
                }
            }
            return null;
        }

        public bool IsOpen{get{return mySerialPort.IsOpen;}}

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
                mySerialPort.Write("}");
            }
            mySerialPort.Close();
        }


 
    }
}
