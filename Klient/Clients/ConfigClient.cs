using Klient.Communicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient.Clients
{
    internal class ConfigClient : QAClient
    {
        public ConfigClient(ClientCommunicator clientCommunicator) : base(clientCommunicator)
        {
        }

        internal void GetStates()
        {
            string question = "conf get-states \n";
            string answer = clientCommunicator.QA(question);
            //answer.Replace('\t', '\n');
            answer = answer.Replace(" ", "\n");
            Console.WriteLine(answer);
        }

        internal void StartMedium(string mediumName)
        {
            string question = $"conf start-medium {mediumName} \n";
            string answer = clientCommunicator.QA(question);
            Console.WriteLine(answer);
        }

        internal void StartService(string serviceName)
        {
            string question = $"conf start-service {serviceName} \n";
            string answer = clientCommunicator.QA(question);
            Console.WriteLine(answer);
        }

        internal void StopMedium(string mediumName)
        {
            string qustion = $"conf stop-medium {mediumName} \n";
            string answer = clientCommunicator.QA(qustion);
            Console.WriteLine(answer);
        }

        internal void StopService(string serviceName)
        {
            string question = $"conf stop-service {serviceName} \n";
            string answer = clientCommunicator.QA(question);
            Console.WriteLine(answer);
        }


    }
}
