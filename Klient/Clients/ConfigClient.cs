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

        internal void StartService(string serviceName)
        {
            string question = $"conf start-service {serviceName} \n";
            string answer = clientCommunicator.QA(question);
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
