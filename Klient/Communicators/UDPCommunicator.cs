using Commons;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Klient.Communicators
{
    internal class UDPCommunicator : ClientCommunicator
    {
        private IPAddress serverAddress;
        private int port;
        private UdpClient client;

        public UDPCommunicator(string hostname, int port)
        {
            if (!IPAddress.TryParse(hostname, out serverAddress))
            {
                throw new ArgumentException("Invalid IP address format");
            }

            this.port = port;
            client = new UdpClient();
        }

        public override string QA(string question)
        {
            byte[] data = Encoding.ASCII.GetBytes(question);
            IPEndPoint serverEP = new IPEndPoint(serverAddress, port);
            client.Send(data, data.Length, serverEP);

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveBytes = client.Receive(ref remoteEP);
            string response = Encoding.ASCII.GetString(receiveBytes);

            return response;
        }
    }
}