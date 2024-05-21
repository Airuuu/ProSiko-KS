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
        //TCPCommunicator tCPCommunicator;

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
                try
                {
                    TcpClient client = server.AcceptTcpClient();

                    if (client != null)
                    {
                        Console.WriteLine($"TCP connect: {client.Client.RemoteEndPoint}");
                        TCPCommunicator tCPCommunicator = new TCPCommunicator(client);
                        //tCPCommunicator = new TCPCommunicator(client);
                        onConnect(tCPCommunicator);
                        //tCPCommunicator.Stop();
                    }
                }
                catch(SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.Interrupted)
                        break;
                    else throw;
                }
            }
        }

        public void Stop()
        {
            shouldTerminate = true;
            //tCPCommunicator.Stop();
            server.Stop();
        }
    }
}