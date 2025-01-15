using System.Net.Sockets;

namespace Extension.Socket;

public class SocketService : ISocketService
{
    private const string Host = "service-action";
    private const int Port = 2728;
    private TcpClient _socketClient;

    public void OpenSocket()
    {
        _socketClient = new TcpClient();
        _socketClient.Connect(Host, Port);
        Console.WriteLine("Socket connected.");
    }

    public void SendHandshake()
    {
        byte[] handshakeData = { 0xa4, 0x0e, 0xa0, 0x01, 0x00 };
        SendData(handshakeData);
    }

    public void NotifyChange()
    {
        byte[] changeData = { 0xa4, 0x0e, 0xa0, 0x04, 0x00 };
        SendData(changeData);
    }

    private void SendData(byte[] data)
    {
        if (_socketClient is { Connected: true })
        {
            var stream = _socketClient.GetStream();
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Data sent.");
        }
        else
        {
            Console.WriteLine("Socket not connected.");
        }
    }

    public void CloseSocket()
    {
        _socketClient.Close();
        Console.WriteLine("Socket closed.");
    }
}