using System.Net.Sockets;

namespace Extension.Socket;

public class SocketService : ISocketService
{
    private const string Host = "service-action";
    private const int Port = 2728;
    private const int BufferSize = 1024;
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
        byte[] paddedHandshakeData = AddPadding(handshakeData);
        
        SendData(paddedHandshakeData);
    }

    public void NotifyChange()
    {
        byte[] changeData = { 0xa4, 0x0e, 0xa0, 0x04, 0x00 };
        byte[] paddedChangeData = AddPadding(changeData);

        SendData(paddedChangeData);
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

    private byte[] AddPadding(byte[] data)
    {
        int paddingLength = BufferSize - data.Length;
        byte[] paddedData = new byte[BufferSize];
        Array.Copy(data, 0, paddedData, 0, data.Length);
        return paddedData;
    }
    
    public void CloseSocket()
    {
        _socketClient.Close();
        Console.WriteLine("Socket closed.");
    }
}