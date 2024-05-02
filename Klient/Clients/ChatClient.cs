using Klient.Communicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient.Clients
{
    internal class ChatClient : QAClient
    {
        private string nickname;
        public ChatClient(ClientCommunicator clientCommunicator, string nickname) : base(clientCommunicator)
        {
            this.nickname = nickname;
        }

        public void ChatMsg(string destinationUser, string message)
        {
            string question = $"chat msg {nickname} {destinationUser} {message}\n";
            string answer = clientCommunicator.QA(question);
            Console.WriteLine("Message sent");
        }

        public void ChatGet(int numberOfMessages = 10)
        {
            string question = $"chat get {nickname} {numberOfMessages}\n";
            string answer = clientCommunicator.QA(question);
            Console.WriteLine(answer);
        }
    }
}
