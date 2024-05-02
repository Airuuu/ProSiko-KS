using Commons;
using Klient.Clients;
using Klient.Communicators;



internal class Program
{
    static string[] availableClients = ["ping", "fpt"];

    private static void Main(string[] args)
    {
        TCPCommunicator clientCommunicator = new TCPCommunicator("localhost", 12345);
        Console.WriteLine("Welcome to Giga Server");
        string nickname;
        while (true)
        {
            Console.Write("Enter your Giga Nickname : ");
            nickname = Console.ReadLine();
            if(nickname != "")
            {
                break;
            }
            Console.WriteLine("Enter your real Giga Nickname!");
        }

        while (true)
        {
            Console.Write($"<{nickname}> $: "); string line = Console.ReadLine();
            string clientComm = ClientTools.GetCommunicator(line);
            string clientType = ClientTools.GetClient(line);
            switch (clientType)
            {
                case "ping":
                    PingClient pc = new PingClient(clientCommunicator);
                    string[] paramsPing = line.Split(" ");
                    double responseTime = pc.Test(int.Parse(paramsPing[2]), int.Parse(paramsPing[3]), int.Parse(paramsPing[4]));
                    Console.WriteLine($"Response time : {responseTime}");
                    break;
                case "ftp":
                    FtpClient fc = new FtpClient(clientCommunicator);
                    string[] paramsFtp = line.Split(" ");
                    switch(paramsFtp[2])
                    {
                        case "put":
                            fc.FtpPut(paramsFtp[3]);
                            break;
                        case "get":
                            fc.FtpGet(paramsFtp[3]);
                            break;
                        case "list":
                            fc.FtpList();
                            break;
                        default:
                            Console.WriteLine("Missing or incorrect client option");
                            break;
                    }
                    break;
                case "chat":
                    ChatClient chat = new ChatClient(clientCommunicator, nickname);
                    string[] paramsChat = line.Split(" ");
                    switch (paramsChat[2])
                    {
                        case "msg":
                            chat.ChatMsg(paramsChat[3], paramsChat[4]);
                            break;
                        case "get":
                            chat.ChatGet();
                            break;
                        default:
                            Console.WriteLine("Missing or incorrect client option");
                            break;
                    }
                    break;
                case "conf":
                    ConfigClient config = new ConfigClient(clientCommunicator);
                    string[] paramsConf = line.Split(" ");
                    switch(paramsConf[2])
                    {
                        case "start-service":
                            config.StartService(paramsConf[3]);
                            break;
                        case "stop-service":
                            config.StopService(paramsConf[3]);
                            break;
                        case "start-medium":
                            break;
                        case "stop-medium":
                            break;
                        default:
                            Console.WriteLine("Missing or incorrect client option");
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Missing or incorrect client type");
                    break;
            }
        }
    }
}

