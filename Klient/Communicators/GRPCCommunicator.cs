using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient.Communicators
{
    internal class GRPCCommunicator : ClientCommunicator
    {
        private string grpclink;
        private readonly QAService.QAServiceClient client;
        private GrpcChannel channel;

        public GRPCCommunicator(string grpclink)
        {
            this.grpclink = grpclink;
            var channelOptions = new GrpcChannelOptions
            {
                MaxSendMessageSize = int.MaxValue,
                MaxReceiveMessageSize = int.MaxValue
            };

            channel = GrpcChannel.ForAddress(grpclink, channelOptions);
            client = new QAService.QAServiceClient(channel);
        }

        public override string QA(string question)
        {
            var questionMessage = new Question
            {
                Text = question
            };

            var answer = client.AskQuestion(questionMessage);
            return answer.Text;

        }

        public override void Dispose()
        {
            channel.Dispose();
        }
    }
}
