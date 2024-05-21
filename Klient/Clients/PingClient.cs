using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commons;
using Klient.Communicators;

namespace Klient.Clients
{
    internal class PingClient : QAClient
    {
        public PingClient(ClientCommunicator clientCommunicator)
            : base(clientCommunicator)
        {
        }

        public double Test(int amount, int outputLen, int inputLen)
        {
            string question = $"ping {inputLen} ";
            question += CommonTools.Trush(outputLen - question.Length - 1);
            question += '\n';

            DateTime startTime = DateTime.Now;
            for (int i = 0; i < amount; i++)
            {
                string answer = clientCommunicator.QA(question);
                Console.WriteLine($"Ping {i+1} / {amount}");
            }
            DateTime endTime = DateTime.Now;
            double duration = (endTime - startTime).TotalSeconds / amount;
            return duration;
        }

    }
}
