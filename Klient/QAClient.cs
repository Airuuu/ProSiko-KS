﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient
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
