using Serwer.Communicators;
using Serwer.Interfaces;
using System.Net.Sockets;
using System.Net;

internal class UDPListener : IListener
{
    private Thread _thread;
    private int portNo;
    private CommunicatorD onConnect;
    private UdpClient server;
    private bool shouldTerminate;

    public UDPListener(int portNo)
    {
        this.portNo = portNo;
    }

    public void Start(CommunicatorD onConnect)
    {
        this.onConnect = onConnect;
        shouldTerminate = false;
        _thread = new Thread(Listen);
        _thread.Start();
    }

    private void Listen()
    {
        server = new UdpClient(portNo);
        while (!shouldTerminate)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveBytes = server.Receive(ref remoteEndPoint);

            Console.WriteLine($"UDP connect: {remoteEndPoint}");

            UDPCommunicator udpCommunicator = new UDPCommunicator(server, remoteEndPoint); // Tutaj jest błąd
            onConnect(udpCommunicator);
        }
    }

    public void Stop()
    {
        shouldTerminate = true;
        server.Close();
    }
}
