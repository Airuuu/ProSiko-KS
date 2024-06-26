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
        }

        public override string QA(string question)
        {
            byte[] data = Encoding.ASCII.GetBytes(question);

            //Send data size
            byte[] sizeBuffer = BitConverter.GetBytes(data.Length);
            client.Send(sizeBuffer, sizeBuffer.Length);

            //Transfer trackers - Question
            int totalBytes = data.Length;
            int bytesSent = 0;
            int bufferSize = 1024;
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

            //Sending data, expecting ACK
            while (bytesSent < totalBytes)
            {
                int bytesToSend = Math.Min(bufferSize, totalBytes - bytesSent);
                byte[] chunk = new byte[bytesToSend];
                Array.Copy(data, bytesSent, chunk, 0, bytesToSend);

                client.Send(chunk, chunk.Length);
                bytesSent += bytesToSend;

                byte[] ackBuffer = client.Receive(ref remoteEP);
            }

            //Transfer trackers - Answer
            byte[] responseSizeBuffer = client.Receive(ref remoteEP);
            int dataSize = BitConverter.ToInt32(responseSizeBuffer, 0);
            byte[] receivedData = new byte[dataSize];
            int totalReceived = 0;

            //Receiving data, sending ACK
            while (totalReceived < dataSize)
            {
                byte[] tempBuffer = client.Receive(ref remoteEP);
                int bytesToCopy = Math.Min(tempBuffer.Length, dataSize - totalReceived);

                Array.Copy(tempBuffer, 0, receivedData, totalReceived, bytesToCopy);
                totalReceived += bytesToCopy;

                client.Send(new byte[] { 1 }, 1);
            }

            string response = Encoding.ASCII.GetString(receivedData, 0, receivedData.Length);
            if (response.StartsWith("Error"))
                Console.WriteLine(response);
            return response;
        }

        public override void Dispose()
        {
            client.Close();
            client.Dispose();
        }
    }
}