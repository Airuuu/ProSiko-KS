using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klient.Communicators;

namespace Klient.Clients
{
    internal class QAClient
    {
        protected ClientCommunicator clientCommunicator;

        public QAClient(ClientCommunicator clientCommunicator)
        {
            this.clientCommunicator = clientCommunicator;
        }
    }
}
