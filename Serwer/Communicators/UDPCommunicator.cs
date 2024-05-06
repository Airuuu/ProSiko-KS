using Commons;
using Serwer.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Serwer.Communicators
{
    internal class UDPCommunicator : ICommunicator
    {
        private UdpClient client;
        private int portNo;
        private CommandD onCommand;
        private CommunicatorD onDisconnect;
        private Thread thread;
        private IPEndPoint remoteEndPoint;
        private bool shouldTerminate;

        public UDPCommunicator(int portNo)
        {
            this.portNo = portNo;
            client = new UdpClient(portNo);
        }

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            shouldTerminate = false;
            this.onCommand = onCommand;
            this.onDisconnect = onDisconnect;
            thread = new Thread(Communicate);
            thread.Start();
        }

        public void Stop()
        {
            shouldTerminate = true;
            client.Close();
        }

        private void Communicate()
        {
            while (!shouldTerminate)
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, portNo);
                byte[] receiveBytes = client.Receive(ref remoteEndPoint);
            
                Console.WriteLine($"UDP connect: {remoteEndPoint}");
                string request = Encoding.ASCII.GetString(receiveBytes);

                string response = onCommand(request);

                byte[] sendBytes = Encoding.ASCII.GetBytes(response);
                client.Send(sendBytes, sendBytes.Length, remoteEndPoint);
                
            }
        }
    }
}