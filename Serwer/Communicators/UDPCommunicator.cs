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

        //private async void Communicate()
        //{
        //    while (!shouldTerminate)
        //    {
        //        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, portNo);

        //        // Odczytujemy wielkość otrzymanych danych
        //        byte[] sizeBuffer = client.Receive(ref remoteEndPoint);
        //        int dataSize = BitConverter.ToInt32(sizeBuffer, 0);

        //        // Odczytujemy dane w pętli, dopóki nie otrzymamy wszystkich
        //        byte[] receivedData = new byte[dataSize];
        //        int totalReceived = 0;
        //        await Console.Out.WriteLineAsync("DS : " + dataSize);
        //        while (totalReceived < dataSize)
        //        {
        //            // Odbieramy dane do tymczasowego bufora
        //            byte[] tempBuffer = client.Receive(ref remoteEndPoint);
        //            // Kopiujemy otrzymane dane do właściwego bufora danych
        //            Buffer.BlockCopy(tempBuffer, 0, receivedData, totalReceived, tempBuffer.Length);
        //            totalReceived += tempBuffer.Length;
        //            await Console.Out.WriteLineAsync($"R: {totalReceived}");
        //        }

        //        Console.WriteLine($"UDP connect: {remoteEndPoint}");

        //        string request = Encoding.ASCII.GetString(receivedData);

        //        string response = onCommand(request);

        //        byte[] sendBytes = Encoding.ASCII.GetBytes(response);
        //        client.Send(sendBytes, sendBytes.Length, remoteEndPoint);
        //    }
        //}

        private async void Communicate()
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
                        try
                        {
                            byte[] tempBuffer = new byte[bufferSize];
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

                    Console.WriteLine($"UDP connect: {remoteEndPoint}");

                    string request = Encoding.ASCII.GetString(receivedData);

                    string response = onCommand(request);

                    byte[] data = Encoding.ASCII.GetBytes(response);
                    byte[] buffer = BitConverter.GetBytes(data.Length);
                    int totalBytes = data.Length;
                    int bytesSent = 0;
                    int bSize = 1024;
                    client.Send(buffer, buffer.Length, remoteEndPoint);

                    while(bytesSent < totalBytes) {
                        int bytesToSend = Math.Min(bSize, totalBytes - bytesSent);
                        byte[] chunk = new byte[bytesToSend];
                        Array.Copy(data, bytesSent, chunk, 0, bytesToSend);

                        client.Send(chunk, chunk.Length, remoteEndPoint);
                        bytesSent += bytesToSend;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled Exception: {ex}");
            }
        }

    }
}