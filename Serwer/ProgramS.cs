using Serwer;
using System.Net.Sockets;

//Console.WriteLine(new PingService().AnswerCommand("ping 100 abc\n"));

Serwer.Serwer serwer = new Serwer.Serwer();
serwer.AddServiceModule("ping", new PingService());
serwer.AddListener(new TCPListener(12345));
serwer.Start();
