using Commons;
using Serwer.Interfaces;
using Serwer.Services;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Communicators
{
    internal class COMCommunicator : ICommunicator
    {
        private SerialPort serialPort;
        private CommandD onCommand;
        private CommunicatorD onDisconnect;
        private Thread thread;

        public COMCommunicator(SerialPort serialPort)
        {
            this.serialPort = serialPort;
            this.serialPort.BaudRate = 115200;
            Console.WriteLine($"COM connect: {serialPort.PortName}");

        }

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            this.onCommand = onCommand;
            this.onDisconnect = onDisconnect;
            thread = new Thread(Communicate);
            thread.Start();
        }

        public void Stop()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                
                serialPort.Close();
            }
        }

        private async void Communicate()
        {
            try
            {
                while (serialPort.IsOpen)
                {
                    string line = await ReadFromSerialPort();
                    if (!string.IsNullOrEmpty(line))
                    {
                        string answer = ProcessCommand(line);
                        WriteToSerialPort(answer);
                    }
                    await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                onDisconnect(this);
            }
        }

        private async Task<string> ReadFromSerialPort()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                while (serialPort.BytesToRead > 0)
                {
                    sb.Append(serialPort.ReadExisting());
                    if (sb.ToString().Contains("\n"))
                        break;
                    await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Read Error: " + ex.Message);
            }
            return sb.ToString();
        }

        private void WriteToSerialPort(string data)
        {
            try
            {
                serialPort.WriteLine(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Write Error: " + ex.Message);
            }
        }

        private string ProcessCommand(string line)
        {
            ConfigService config = new ConfigService();
            string configAnswer = onCommand("conf get-states");
            string answer = "";
            if (line.Split(" ")[0] != "conf" && !ServerTools.GetSpecifiedState(line.Split(" ")[0], configAnswer))
            {
                answer += "Error : Service is OFFLINE! \n";
            }
            else
            {
                answer += onCommand(line);
            }
            return answer;
        }
    }
}
