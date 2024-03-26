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
//double time = fc.FtpPut(@"C:\rules.txt");
//double time = fc.FtpPut(@"C:\Users\AirU\Downloads\logos\2.png");
//double time = fc.FtpList();
double time = fc.FtpGet("2(2).png");
Console.WriteLine($"Ftp put client average time: {time}");
