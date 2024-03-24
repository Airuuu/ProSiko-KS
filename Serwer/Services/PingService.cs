using System.Transactions;
using Commons;
using Serwer.Interfaces;
using Commons;

namespace Serwer.Services
{
    internal class PingService : IServiceModule
    {
        public PingService()
        {
        }

        public string AnswerCommand(string command)
        {
            int n = CommonTools.GetAnswerLength(command);
            return "ping " + CommonTools.Trush(n - 6) + '\n';
        }
    }
}