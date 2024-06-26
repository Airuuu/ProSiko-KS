using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient.Communicators
{
    internal abstract class ClientCommunicator
    {
        public abstract string QA(string question);
        public abstract void Dispose();
        
    }
}
