using Klient.Clients;
using Klient.Communicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient
{
    internal class PingTest
    {
        private string servername;
        private string serialPortName;
        private string grpclink;

        private int TcpPortNo;
        private int UdpPortNo;

        private Dictionary<string, ClientCommunicator> clients = new Dictionary<string, ClientCommunicator>();
        private Dictionary<string, double> avgTimes = new Dictionary<string, double>();

        private int pingAmount;
        private int length;
        private int trushSize;

        private int repeatingTimes;
        public PingTest(string servername, string serialPortName, string grpclink, int TcpPortNo, int UdpPortNo)
        {
            Console.Clear();
            this.servername = servername;
            this.serialPortName = serialPortName;
            this.grpclink = grpclink;
            this.TcpPortNo = TcpPortNo;
            this.UdpPortNo = UdpPortNo;
            Console.WriteLine("PING CLASS INITIALIZED...");
            Initiate();
        }

        internal void Begin()
        {
            Console.WriteLine("Testing in progress, please wait ...");
            foreach (var client in clients) 
            {
                List<double> times = new List<double>();
                PingClient pingClient = new PingClient(client.Value);
                for(int i = 0; i < repeatingTimes; i++)
                {
                    double time = pingClient.Test(pingAmount, length, trushSize, false);
                    times.Add(time);
                }
                avgTimes.Add(client.Key, times.Average());
            }
        }

        internal void Dispose()
        {
            foreach(var communicator in clients.Values)
                communicator.Dispose();
        }

        internal void Results()
        {
            Console.Clear();
            foreach (var avgTime in avgTimes)
            {
                Console.WriteLine($"{avgTime.Key}\t:\t{avgTime.Value * 1000} ms");
            }
        }

        private void Initiate()
        {
            clients.Add("file", new FileCommunicator());
            clients.Add("tcp", new TCPCommunicator(servername, TcpPortNo));
            clients.Add("udp", new UDPCommunicator(servername, UdpPortNo));
            clients.Add("com", new COMCommunicator(serialPortName));
            clients.Add("grpc", new GRPCCommunicator(grpclink));
            Console.WriteLine("COMMUNICATORS INITIALIZED...\n");
            Console.Write("Number of pings : "); pingAmount = int.Parse(Console.ReadLine());
            Console.Write("Length : "); length = int.Parse(Console.ReadLine());
            Console.Write("Trush size : "); trushSize = int.Parse(Console.ReadLine());
            Console.Write("Repeating times : "); repeatingTimes = int.Parse(Console.ReadLine());
            Console.WriteLine($"\nCommand: ping {pingAmount} {length} {trushSize}");
            Console.WriteLine($"Repeating times : {repeatingTimes}");
            Console.WriteLine("Press any key to proceed ... "); Console.ReadKey();
            Console.Clear();
        }
    }
}
