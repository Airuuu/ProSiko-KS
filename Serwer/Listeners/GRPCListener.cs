using Grpc.Core;
using Serwer.Communicators;
using Serwer.Interfaces;
using System;
using System.Threading;

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
            GRPCCommunicator gRPCCommunicator = new GRPCCommunicator();

            var options = new List<ChannelOption>
            {
                new ChannelOption(ChannelOptions.MaxSendMessageLength, int.MaxValue),
                new ChannelOption(ChannelOptions.MaxReceiveMessageLength, int.MaxValue)
            };

            server = new Server(options)
            {
                Services = { QAService.BindService(gRPCCommunicator) },
                Ports = { new ServerPort("localhost", portNo, ServerCredentials.Insecure) },
            };
            
            server.Start();
            onConnect(gRPCCommunicator);
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

