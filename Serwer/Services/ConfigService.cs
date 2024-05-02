using Commons;
using Serwer.Interfaces;
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
        

        public ConfigService()
        {
        }

        public void SetDisconnectService(Func<string, string> disconnectService)
        {
            _disconnectService = disconnectService;
        }

       

        public string AnswerCommand(string command)
        {
            string commandType = ServerTools.DetectCommandType(command);
            string answer;
            switch (commandType)
            {
                case "stop-service":
                    answer = StopService(command);
                    break;
                case "start-service":
                    answer = StartService(command);
                    break;
                default:
                    return "Wrong command option! \n";
            }
            return answer;
        }

        private string StartService(string command)
        {
            string serviceName = ServerTools.GetConfigName(command);
            
            return $"Service {serviceName} has been connected.\n";
        }

        private string StopService(string command)
        {
            string serviceName = ServerTools.GetConfigName(command);
            _disconnectService(serviceName);
            return $"Service {serviceName} has been disconnected.\n";
        }
    }
}
