using Commons;
using Klient.Clients;
using Klient.Communicators;



internal class Program
{
    static string[] availableClients = ["ping", "fpt"];
    enum Clients
    {
        ping = 1,
        ftp = 1
    }
    private static void Main(string[] args)
    {
        TCPCommunicator clientCommunicator = new TCPCommunicator("localhost", 12345);
        Console.WriteLine("Welcome to Giga Server");
        //Dictionary<string, object> clientCommunicator = new Dictionary<string, object> {
        //    { "tcp", new TCPCommunicator("localhost", 12345)}
        //};

        while (true)
        {
            Console.Write($"<UserHolder> $ "); string line = Console.ReadLine();
            string clientComm = ClientTools.GetCommunicator(line);
            string clientType = ClientTools.GetClient(line);
            switch (clientType)
            {
                case "ping":
                    PingClient pc = new PingClient(clientCommunicator);
                    string[] paramsPing = line.Split(" ");
                    pc.Test(int.Parse(paramsPing[2]), int.Parse(paramsPing[3]), int.Parse(paramsPing[4]));
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
                default:
                    Console.WriteLine("Missing or incorrect client type");
                    break;
            }
        }
    }
}

