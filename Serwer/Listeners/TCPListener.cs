using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using Serwer.Interfaces;
using Serwer.Communicators;

namespace Serwer.Listeners
{

    internal class TCPListener : IListener
    {
        Thread _thread;
        int portNo;
        CommunicatorD onConnect;
        TcpListener server;
        bool shouldTerminate;

        public TCPListener(int portNo) { this.portNo = portNo; }

        public void Start(CommunicatorD onConnect)
        {
            this.onConnect = onConnect;
            shouldTerminate = false;
            _thread = new Thread(Listen);
            _thread.Start();
        }

        private void Listen()
        {
            server = new TcpListener(IPAddress.Any, portNo);
            server.Start();
            while (!shouldTerminate)
            {
                TcpClient client = server.AcceptTcpClient();
                if (client != null)
                {
                    Console.WriteLine($"TCP connect: {client.Client.RemoteEndPoint}");
                    TCPCommunicator tCPCommunicator = new TCPCommunicator(client);
                    onConnect(tCPCommunicator);
                }
            }
        }

        public void Stop()
        {
            shouldTerminate = true;
            server.Stop();
        }
    }
}