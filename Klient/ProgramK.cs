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
        //while (true)
        //{

        //}
        //PingClient pc = new PingClient(clientCommunicator);
        //double time = pc.Test(10, 1024, 2048);
        //Console.WriteLine($"Ping client average time: {time}");
        //FtpClient fc = new FtpClient(clientCommunicator);
        //double time = fc.FtpPut(@"C:\rules.txt");
        //double time = fc.FtpPut(@"C:\Users\AirU\Downloads\logos\2.png");
        //double time = fc.FtpList();
        //double time = fc.FtpGet("2(2).png");
        //Console.WriteLine($"Ftp put client average time: {time}");
        Console.WriteLine("Welcome to Giga Server");

        while (true)
        {
            Console.Write($"<UserHolder> $ "); string line = Console.ReadLine();
            string clientType = ClientTools.GetClient(line);
            switch (clientType)
            {
                case "ping":
                    PingClient pc = new PingClient(clientCommunicator);
                    string[] paramsPing = line.Split(" ");
                    pc.Test(int.Parse(paramsPing[1]), int.Parse(paramsPing[2]), int.Parse(paramsPing[3]));
                    break;
                case "ftp":
                    FtpClient fc = new FtpClient(clientCommunicator);
                    string[] paramsFtp = line.Split(" ");
                    switch(paramsFtp[1])
                    {
                        case "put":
                            fc.FtpPut(paramsFtp[2]);
                            break;
                        case "get":
                            fc.FtpGet(paramsFtp[2]);
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

