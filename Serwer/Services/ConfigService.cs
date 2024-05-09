using Commons;
using Serwer.Interfaces;
using Serwer.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Services
{
    internal class ConfigService : IServiceModule
    {
        private Func<string, string> _disconnectService;
        private Func<string, string> _disconnectListener;

        private Action<string, IServiceModule> _connectService;
        private Action<IListener, bool> _connectListener;
        
        
        private Func<List<string>> _listenersState;
        private Func<List<string>> _servicesState;

        private List<string> modulesAndServices = new List<string>
        {
            "tcp", "udp", "ping", "ftp", "chat"
        };

        public ConfigService()
        {
        }

        public void SetConnectService(Action<string, IServiceModule> connectService)
        {
            _connectService = connectService;
        }

        public void SetConnectListener(Action<IListener, bool> connectListener)
        {
            _connectListener = connectListener;
        }

        public void SetDisconnectService(Func<string, string> disconnectService)
        {
            _disconnectService = disconnectService;
        }

        public void SetDisconnectListener(Func<string, string> disconnectListener)
        {
            _disconnectListener = disconnectListener;
        }

       public void GetListenersState(Func<List<string>> listenersState)
        {
            _listenersState = listenersState;
        }

        public void GetServicesState(Func<List<string>> servicesState)
        {
            _servicesState = servicesState;
        }

        public string AnswerCommand(string command)
        {
            string commandType = ServerTools.DetectCommandType(command);
            string answer;
            switch (commandType)
            {
                case "get-states":
                    answer = GetStates();
                    break;
                case "stop-service":
                    answer = StopService(command);
                    break;
                case "start-service":
                    answer = StartService(command);
                    break;
                case "stop-medium":
                    answer = StopMedium(command);
                    break;
                case "start-medium":
                    answer = StartMedium(command);
                    break;
                default:
                    return "Wrong command option! \n";
            }
            return answer;
        }

        private string GetStates()
        {

            List<string> listeners = _listenersState();
            foreach (string listener in listeners)
            {
                Console.WriteLine(listener);
            }

            List<string> services = _servicesState();
            foreach (string service in services)
            {
                Console.WriteLine(service);
            }
            List<Tuple<string, string>> states = new List<Tuple<string, string>>();
            foreach (string item in modulesAndServices)
            {
                bool isOnline = listeners.Any(listener => listener.ToLower().Contains(item.ToLower())) ||
                                services.Any(service => service.ToLower().Contains(item.ToLower()));

                states.Add(new Tuple<string, string>(item, isOnline ? "ONLINE" : "OFFLINE"));
            }


            string info = "Online-Listeners: ";
            foreach(Tuple<string, string> state in states)
            {
                if (state.Item1 == "ping")
                    info += "  Online-Services: ";
                info += state.Item1+"\t"+state.Item2+" ";
            }
            info += "\n";
            return info;
        }

        private string StopMedium(string command)
        {
            string mediumName = ServerTools.GetConfigName(command);
            if (!ServerTools.GetSpecifiedState(mediumName, GetStates()))
                return "Medium is already OFFLINE or doesn\'t exist!";
            _disconnectListener(mediumName);
            return $"Service {mediumName} has been disconnected.\n";
        }

        private string StartMedium(string command)
        {
            string mediumName = ServerTools.GetConfigName(command);
            if (ServerTools.GetSpecifiedState(mediumName, GetStates()))
                return "Medium is already ONLINE!\n";
            switch (mediumName)
            {
                case "tcp":
                    _connectListener(new TCPListener(12345), true);
                    break;
                case "udp":
                    _connectListener(new UDPListener(12346), true);
                    break;
                default:
                    return "Medium not found";
            }
            return $"Service {mediumName} has been connected.\n";
        }

        private string StartService(string command)
        {
            string serviceName = ServerTools.GetConfigName(command);
            if (ServerTools.GetSpecifiedState(serviceName, GetStates()))
                return "Service is already ONLINE!\n";
            switch(serviceName)
            {
                case "ping":
                    _connectService("ping", new PingService());
                    break;
                case "ftp":
                    _connectService("ftp", new FtpService());
                    break;
                case "chat":
                    _connectService("chat", new ChatService());
                    break;
                default:
                    return "Service not found";
            }
            return $"Service {serviceName} has been connected.\n";
        }

        private string StopService(string command)
        {
            string serviceName = ServerTools.GetConfigName(command);
            if (!ServerTools.GetSpecifiedState(serviceName, GetStates()))
                return "Service is already OFFLINE or doesn\'t exist!";
            _disconnectService(serviceName);
            return $"Service {serviceName} has been disconnected.\n";
        }
    }
}
