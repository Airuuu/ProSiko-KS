using Klient.Clients;
using Klient.Communicators;



TCPCommunicator clientCommunicator = new TCPCommunicator("localhost", 12345);
//while (true)
//{

//}
//PingClient pc = new PingClient(clientCommunicator);
//double time = pc.Test(10, 1024, 2048);
//Console.WriteLine($"Ping client average time: {time}");
FtpClient fc = new FtpClient(clientCommunicator);
double time = fc.Test(@"C:\rules.txt");