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
        private CommandD onCommand;
        private CommunicatorD onDisconnect;
        private Thread thread;
        private IPEndPoint remoteEndPoint;

        public UDPCommunicator(UdpClient client, IPEndPoint remoteEndPoint)
        {
            this.client = client;
            this.remoteEndPoint = remoteEndPoint;
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
            client.Close();
        }

        private void Communicate()
        {
            //while (true)
            //{
                byte[] receiveBytes = client.Receive(ref remoteEndPoint);
                string request = Encoding.ASCII.GetString(receiveBytes);
                Console.WriteLine("Received: " + request);

                string response = onCommand(request);

                byte[] sendBytes = Encoding.ASCII.GetBytes(response);
                client.Send(sendBytes, sendBytes.Length, remoteEndPoint);
                Console.WriteLine("Sent: " + response);
            //}
        }
    }
}