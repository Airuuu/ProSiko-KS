using Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Klient.Communicators
{
    internal class TCPCommunicator : ClientCommunicator
    {
        private string hostname;
        private int port;
        private TcpClient client;

        public TCPCommunicator(string hostname, int port)
        {
            this.hostname = hostname;
            this.port = port;
            client = new TcpClient(hostname, port);
        }

        public override string QA(string question)
        {
            byte[] data = Encoding.ASCII.GetBytes(question);
            NetworkStream stream = client.GetStream();
            Console.WriteLine($"S: {data.Length} B, {question.SubstringMax(40)}");
            stream.Write(data, 0, data.Length);
            data = new byte[4096];
            string odp = string.Empty;
            int bytes;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                odp += Encoding.ASCII.GetString(data, 0, bytes);
            }
            while (odp.LastIndexOf('\n') == -1);
            Console.WriteLine($"Odp: {odp.Length} B, {odp.SubstringMax(40)}");
            return odp;
        }
    }
}
