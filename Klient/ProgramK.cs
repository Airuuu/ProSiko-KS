﻿using Commons;
using Klient.Clients;
using Klient.Communicators;




internal class Program
{
    static string[] availableClients = ["ping", "fpt"];
    private static string servername = "localhost";
    private static int TcpPortNo = 12345;
    private static int UdpPortNo = 12346;

    private static void Main(string[] args)
    {
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
            ClientCommunicator clientCommunicator = null;
            Console.Write($"<{nickname}> $: "); string line = Console.ReadLine();
            string clientComm = ClientTools.GetCommunicator(line);
            switch(clientComm)
            {
                case "tcp":
                    clientCommunicator = new TCPCommunicator(servername, TcpPortNo);
                    break;
                case "udp":
                    clientCommunicator = new UDPCommunicator(servername, UdpPortNo);
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Missing or incorrect communicator option");
                    break;
            }

            string clientType = ClientTools.GetClient(line);
            switch (clientType)
            {
                case "ping":
                    PingClient pc = new PingClient(clientCommunicator);
                    string[] paramsPing = line.Split(" ");
                    double responseTime = pc.Test(int.Parse(paramsPing[2]), int.Parse(paramsPing[3]), int.Parse(paramsPing[4]));
                    Console.WriteLine($"Response time : {responseTime} s");
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
                        case "get-states":
                            config.GetStates();
                            break;
                        case "start-service":
                            config.StartService(paramsConf[3]);
                            break;
                        case "stop-service":
                            config.StopService(paramsConf[3]);
                            break;
                        case "start-medium":
                            config.StartMedium(paramsConf[3]);
                            break;
                        case "stop-medium":
                            config.StopMedium(paramsConf[3]);
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

