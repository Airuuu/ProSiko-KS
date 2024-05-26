using Grpc.Core;
using Serwer.Communicators;
using Serwer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Listeners
{
    internal class GRPCListener : IListener
    {
        private Server server;
        private int portNo;
        private CommunicatorD onConnect;

        public GRPCListener(int portNo)
        {
            this.portNo = portNo;
        }

        public void Start(CommunicatorD onConnect)
        {
            this.onConnect = onConnect;

            server = new Server
            {
                Services = { QAService.BindService(new GRPCCommunicator(onConnect)) },
                Ports = { new ServerPort("localhost", portNo, ServerCredentials.Insecure) }
            };

            server.Start();
        }

        public void Stop()
        {
            if (server != null)
            {
                server.ShutdownAsync().Wait();
            }
        }
    }
}
