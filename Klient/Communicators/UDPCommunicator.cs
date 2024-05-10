using Commons;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Intrinsics.Arm;
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
            client.Client.SendBufferSize = 65536;
            client.Ttl = 255;
            client.Connect(serverAddress, port);
            //add stop methods for exitting
        }

        public override string QA(string question)
        {
            byte[] data = Encoding.ASCII.GetBytes(question);

            byte[] sizeBuffer = BitConverter.GetBytes(data.Length);
            client.Send(sizeBuffer, sizeBuffer.Length);

            int totalBytes = data.Length;
            int bytesSent = 0;
            int bufferSize = 1024;

            while (bytesSent < totalBytes)
            {
                int bytesToSend = Math.Min(bufferSize, totalBytes - bytesSent);
                byte[] chunk = new byte[bytesToSend];
                Array.Copy(data, bytesSent, chunk, 0, bytesToSend);

                client.Send(chunk, chunk.Length);
                bytesSent += bytesToSend;
            }

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

            byte[] responseBuffer = client.Receive(ref remoteEP);
            int dataSize = BitConverter.ToInt32(responseBuffer, 0);

            byte[] receivedData = new byte[dataSize];
            int totalReceived = 0;
            int bufferSizeReceive = 1024;


            while (totalReceived < dataSize)
            {
                try
                {
                    byte[] tempBuffer = new byte[bufferSizeReceive];
                    int received = client.Client.Receive(tempBuffer);

                    Buffer.BlockCopy(tempBuffer, 0, receivedData, totalReceived, received);
                    totalReceived += received;

                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"SocketException: {ex}");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex}");
                    break;
                }
            }

            string response = Encoding.ASCII.GetString( receivedData, 0 , receivedData.Length );
            if (response.Split(" ")[0] == "Error")
                Console.WriteLine(response);
            return response;
        }
    }
}