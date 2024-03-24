using Klient;

TCPCommunicator clientCommunicator = new TCPCommunicator("localhost", 12345);
PingClient pc = new PingClient(clientCommunicator);
double time = pc.Test(10, 1024, 2048);
Console.WriteLine($"Ping client average time: {time}");