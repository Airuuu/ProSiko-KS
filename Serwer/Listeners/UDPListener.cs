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

    public UDPListener(int portNo)
    {
        this.portNo = portNo;
    }

    public void Start(CommunicatorD onConnect)
    {
        this.onConnect = onConnect;
        _thread = new Thread(Listen);
        _thread.Start();
    }

    private void Listen()
    {

        UDPCommunicator udpCommunicator = new UDPCommunicator(portNo);
        onConnect(udpCommunicator);
    }

    public void Stop()
    {
        
    }
}
