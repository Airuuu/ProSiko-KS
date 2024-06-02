using Commons;
using Grpc.Core;
using Serwer.Interfaces;
using Serwer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Communicators
{
    internal class GRPCCommunicator : QAService.QAServiceBase, ICommunicator
    {
        private CommunicatorD onConnect;
        private CommandD onCommand;
        private CommunicatorD onDisconnect;
        //public GRPCCommunicator(CommunicatorD onConnect)
        //{
        //    this.onConnect = onConnect;
        //}

        public override Task<Answer> AskQuestion(Question request, ServerCallContext context)
        {
            string question = request.Text;

            string answerText = ProcessCommand(question);
            //string answerText = "ugabuga";

            Answer answer = new Answer { Text = answerText };
            return Task.FromResult(answer);
        }

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            this.onCommand = onCommand;
            this.onDisconnect = onDisconnect;
        }

        public void Stop()
        {
            
        }

        private string ProcessCommand(string line)
        {
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
            return answer;
        }
    }
}
