using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commons;

namespace Klient
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
            for (int i=0; i< amount; i++)
            {
                //Console.WriteLine($"S: {question}");
                string answer = clientCommunicator.QA(question);
                //Console.WriteLine($"R: {answer}");
            }
            DateTime endTime = DateTime.Now;
            double duration = (endTime - startTime).TotalSeconds / amount;
            return duration;
        }

    }
}
