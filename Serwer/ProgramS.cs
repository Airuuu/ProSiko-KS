using Serwer.Listeners;
using Serwer.Services;
using System.Net.Sockets;

//Console.WriteLine(new PingService().AnswerCommand("ping 100 abc\n"));

namespace Serwer
{
    public class Program
    {
        public static void Main()
        {
            Serwer serwer = new Serwer();
            serwer.AddServiceModule("ping", new PingService());
            Console.WriteLine("Ping initiated");
            serwer.AddServiceModule("ftp", new FtpService());
            Console.WriteLine("Ftp initiated");
            serwer.AddServiceModule("chat", new ChatService());
            Console.WriteLine("Chat initiated");
            serwer.AddServiceModule("conf", new ConfigService());
            Console.WriteLine("Config initiated");
            serwer.AddListener(new TCPListener(12345));
            serwer.AddListener(new UDPListener(12346));
            serwer.Start();
        }
    }
}

