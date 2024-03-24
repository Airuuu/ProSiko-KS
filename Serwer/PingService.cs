using System.Transactions;
using Commons;

namespace Serwer
{
    internal class PingService : IServiceModule
    {
        public PingService()
        {
        }

        public string AnswerCommand(string command)
        {
            int n = GetAnswerLength(command);
            return "ping " + CommonTools.Trush(n-6) + '\n';
          }

        private int GetAnswerLength(string command)
        {
            var separator = " ";
            var i = command.IndexOf(separator);
            var j = command.IndexOf(separator, i+1);
            return int.Parse(command.Substring(i+1, j-i-1));
        }
    }
}