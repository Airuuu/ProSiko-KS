using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serwer.Interfaces;

namespace Serwer
{
    internal class Serwer
    {
        Dictionary<string, IServiceModule> services = new();
        List<IListener> listeners = new();
        List<ICommunicator> communicators = new();

        public void AddServiceModule(string name, IServiceModule service)
        {
            services.Add(name, service);
        }
        public void AddCommunicator(ICommunicator communicator)
        {
            communicators.Add(communicator);
            communicator.Start(CentrumUsług, DisconnectCommunicator);
        }

        private void DisconnectCommunicator(ICommunicator commander)
        {
            communicators.Remove(commander);
        }

        private string CentrumUsług(string command)
        {
            try
            {
                var commandType = GetCommandType(command);
                var service = services[commandType];
                return service.AnswerCommand(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return $"Service failure with exception: {ex.Message}\n";
            }
        }

        private string GetCommandType(string command)
        {
            var separator = " ";
            var i = command.IndexOf(separator);
            return command.Substring(0, i);
        }

        public void AddListener(IListener listener)
        {
            listeners.Add(listener);
        }

        internal void Start()
        {
            foreach (var listener in listeners)
                listener.Start(AddCommunicator);
        }
    }
}
