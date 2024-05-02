using Commons;
using Serwer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Services
{
    internal class ChatService : IServiceModule
    {
        private Dictionary<string, List<Tuple<string, DateTime, string>>> messageList = new Dictionary<string, List<Tuple<string, DateTime, string>>>();
        public string AnswerCommand(string command)
        {
            string commandType = ServerTools.DetectCommandType(command);
            string answer;
            switch (commandType)
            {
                case "msg":
                    answer = MsgService(command);
                    break;
                case "get":
                    answer = GetService(command);
                    break;
                default:
                    return "Wrong command option! \n";
                
            }
            return answer;
        }

        private string GetService(string command)
        {
            string user = command.Split(" ")[2];
            int numberOfMessages = int.Parse(command.Split(" ")[3]);



            if (messageList.TryGetValue(user, out List<Tuple<string, DateTime, string>> messages))
            {
                int messageCounter = Math.Min(numberOfMessages, messages.Count);
                StringBuilder result = new StringBuilder();


                result.AppendLine($"Messages for user {user}");

                messages.OrderByDescending(msg => msg.Item2).ToList();

                for (int i = 0; i < messageCounter; i++)
                {
                    Tuple<string, DateTime, string> message = messages[i];
                    Console.WriteLine($"From: {message.Item1} | Date: {message.Item2} | Message: {message.Item3}");
                    result.AppendLine($"From: {message.Item1} | Date: {message.Item2} | Message: {message.Item3}");
                }

                return result.ToString() + "\n";

            }
            else
            {
                return "No message found! \n";
            }
        }

        private string MsgService(string command)
        {
            string sourceUser = command.Split(" ")[2];
            string destinationUser = command.Split(" ")[3];
            string message = command.Split(' ')[4];

            if (!messageList.TryGetValue(destinationUser, out List<Tuple<string, DateTime, string>> messages))
            {
                messages = new List<Tuple<string, DateTime, string>>();
                messageList.Add(destinationUser, messages);
            }

            messages.Add(new Tuple<string, DateTime, string>(sourceUser, DateTime.Now, message));

            return "Message sent!\n";
        }
    }
}
