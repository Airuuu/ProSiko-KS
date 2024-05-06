using Commons;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Klient.Communicators
{
    internal class UDPCommunicator : ClientCommunicator
    {
        private string serverAddress;
        private int port;
        private UdpClient client;

        public UDPCommunicator(string hostname, int port)
        {
            serverAddress = hostname;
            this.port = port;
            client = new UdpClient();
            client.Connect(serverAddress, port);
            //add stop methods for exitting
        }

        public override string QA(string question)
        {
            byte[] data = Encoding.ASCII.GetBytes(question);
            
            client.Send(data, data.Length);
            
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveBytes = client.Receive(ref remoteEP);
            string response = Encoding.ASCII.GetString(receiveBytes);

            return response;
        }
    }
}