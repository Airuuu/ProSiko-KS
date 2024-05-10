using Commons;
using Serwer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Commons;
using Serwer.Services;

namespace Serwer.Communicators
{
    internal class TCPCommunicator : ICommunicator
    {
        private TcpClient client;
        private CommandD onCommand;
        private CommunicatorD onDisconnect;
        private Thread thread;

        public TCPCommunicator(TcpClient client)
        {
            this.client = client;
        }

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            this.onCommand = onCommand;
            this.onDisconnect = onDisconnect;
            thread = new Thread(Communicate);
            thread.Start();
        }

        public void Stop()
        {
            if (client.Connected)
            {
                client.Close();
            }
        }

        void Communicate()
        {
            byte[] bytes = new byte[4096];
            string? data = null;
            int len, nl;
            NetworkStream stream = client.GetStream();
            try
            {
                while ((len = stream.Read(bytes, 0, bytes.Length)) > 0)
                {
                    data += Encoding.ASCII.GetString(bytes, 0, len);
                    while ((nl = data.IndexOf('\n')) != -1)
                    {
                        string line = data.Substring(0, nl + 1);
                        data = data.Substring(nl + 1);
                        

                        ConfigService config = new ConfigService();
                        string configAnswer = onCommand("conf get-states");

                        string answer = "";
                        if (line.Split(" ")[0] != "conf" && !ServerTools.GetSpecifiedState(line.Split(" ")[0], configAnswer))
                        {
                            answer += "Error : Service is OFFLINE! \n";
                        }
                        else
                        {
                            answer += onCommand(line);
                        }

                        byte[] msg = Encoding.ASCII.GetBytes(answer);
                        stream.Write(msg, 0, msg.Length);
                    }
                }
            }
            catch { }
            if (client.Connected)
            {
                client.Close();
                onDisconnect(this);
            }
        }
    }
}
