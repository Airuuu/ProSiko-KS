using Commons;
using Serwer.Interfaces;
using Serwer.Services;
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
            client.Client.ReceiveBufferSize = 65536;
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
            try
            {
                while (!shouldTerminate)
                {
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, portNo);

                    byte[] sizeBuffer = client.Receive(ref remoteEndPoint);
                    int dataSize = BitConverter.ToInt32(sizeBuffer, 0);

                    byte[] receivedData = new byte[dataSize];
                    int totalReceived = 0;
                    int bufferSize = 1024;

                    while (totalReceived < dataSize)
                    {
                        byte[] tempBuffer = client.Receive(ref remoteEndPoint);
                        int bytesToCopy = Math.Min(tempBuffer.Length, dataSize - totalReceived);

                        Array.Copy(tempBuffer, 0, receivedData, totalReceived, bytesToCopy);
                        totalReceived += bytesToCopy;

                        client.Send(new byte[] { 1 }, 1, remoteEndPoint);
                    }

                    Console.WriteLine($"UDP connect: {remoteEndPoint}");


                    string request = Encoding.ASCII.GetString(receivedData);

                    string response = HandleRequest(request);

                    byte[] responseData = Encoding.ASCII.GetBytes(response);
                    byte[] responseSizeBuffer = BitConverter.GetBytes(responseData.Length);
                    client.Send(responseSizeBuffer, responseSizeBuffer.Length, remoteEndPoint);

                    int bytesSent = 0;
                    while (bytesSent < responseData.Length)
                    {
                        int bytesToSend = Math.Min(bufferSize, responseData.Length - bytesSent);
                        byte[] chunk = new byte[bytesToSend];
                        Array.Copy(responseData, bytesSent, chunk, 0, bytesToSend);

                        client.Send(chunk, chunk.Length, remoteEndPoint);
                        bytesSent += bytesToSend;

                        byte[] ackBuffer = client.Receive(ref remoteEndPoint);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled Exception: {ex}");
            }
        }

        private string HandleRequest(string request)
        {
            ConfigService config = new ConfigService();
            string configAnswer = onCommand("conf get-states");

            if (request.Split(" ")[0] != "conf" && !ServerTools.GetSpecifiedState(request.Split(" ")[0], configAnswer))
            {
                return "Error: Service is OFFLINE!\n";
            }
            return onCommand(request);
        }
    }

}