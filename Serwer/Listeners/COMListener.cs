using Serwer.Communicators;
using Serwer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Listeners
{
    internal class COMListener : IListener
    {
        private Thread _thread;
        private string portName;
        private CommunicatorD onConnect;
        private SerialPort serialPort;
        private bool shouldTerminate;

        public COMListener(string portName)
        {
            this.portName = portName;
        }

        public void Start(CommunicatorD onConnect)
        {
            this.onConnect = onConnect;
            shouldTerminate = false;
            _thread = new Thread(Listen);
            _thread.Start();
        }

        public void Stop()
        {
            shouldTerminate = true;
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
        private async void Listen()
        {
            serialPort = new SerialPort(portName, 115200);
            serialPort.Open();
            while (!shouldTerminate)
            {
                try
                {
                    if (serialPort.BytesToRead > 0)
                    {
                        var communicator = new COMCommunicator(serialPort);
                        onConnect(communicator);
                    }
                    await Task.Delay(100);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    break;
                }
            }
        }
    }
}
