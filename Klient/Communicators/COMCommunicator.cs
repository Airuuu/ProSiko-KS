using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient.Communicators
{
    internal class COMCommunicator : ClientCommunicator, IDisposable
    {
        private string portName;
        private SerialPort serialPort;

        public COMCommunicator(string portName)
        {
            this.portName = portName;
            serialPort = new SerialPort(portName);
            this.serialPort.BaudRate = 115200;
            serialPort.Open();
        }

        public override string QA(string question)
        {
            //serialPort.Open();
            WriteToSerialPort(question);
            string response = ReadFromSerialPort();
            //serialPort.Close();
            return response;
        }

        private void WriteToSerialPort(string data)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(data + "\n");
                serialPort.Write(bytes, 0, bytes.Length);
                serialPort.BaseStream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Write Error: " + ex.Message);
            }
        }

        private string ReadFromSerialPort()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                while (true)
                {
                    sb.Append(serialPort.ReadExisting());
                    if (sb.ToString().Contains("\n"))
                        break;
                    //Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Read Error: " + ex.Message);
            }
            return sb.ToString().Trim();
        }

        public void Dispose()
        {
            serialPort.Close();
            serialPort.Dispose();
        }
    }
}
