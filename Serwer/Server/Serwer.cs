using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serwer.Interfaces;
using Serwer.Services;

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

            if (service is ConfigService configService)
            {
                configService.SetDisconnectService(DisconnectServiceModule);
                configService.SetDisconnectListener(DisconnectListener);

                configService.SetConnectService(AddServiceModule);
                configService.SetConnectListener(AddListener);
                
                configService.GetListenersState(ListenersState);
                configService.GetServicesState(ServicesState);

            }
        }

        public List<string> ServicesState()
        {
            List<string> onlineServices = new List<string>();
            foreach (var service in services)
            {
                onlineServices.Add(service.Key);
            }
            return onlineServices;
        }

        public List<string> ListenersState()
        {
            List<string> onlineListeners = new List<string>();
            foreach (IListener listener in listeners)
            {
                onlineListeners.Add(listener.ToString());
            }
            return onlineListeners;
        }

        public string DisconnectServiceModule(string name)
        {
            services.Remove(name);
            return $"Service {name} disconnected";
        }

        public string DisconnectListener(string name)
        {

            foreach (var listener in listeners)
            {
                if (listener.ToString().ToLower().Contains(name.ToLower()))
                {
                    listener.Stop();
                    listeners.Remove(listener);

                    return $"Listener {name} disconnected";
                }
            }
            return $"Listener {name} not found";
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
                var service = services[commandType] ?? null;
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

        public void AddListener(IListener listener, bool isRequested = false)
        {
            listeners.Add(listener);
            if(isRequested) {
                listener.Start(AddCommunicator);
            }
        }

        internal void Start()
        {
            foreach (var listener in listeners)
                listener.Start(AddCommunicator);
        }
    }
}
