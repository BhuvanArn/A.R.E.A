using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Extension.Socket
{
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

        public void SendHandshakeAndNotifyChange()
        {
            // 0xa4 0xea 0x01 0x00
            byte[] handshakeData = { 0xa4, 0xea, 0x01, 0x00 };
            byte[] paddedHandshakeData = AddPadding(handshakeData);

            SendData(paddedHandshakeData);
            Console.WriteLine("Handshake sent.");

            byte[]? receivedData = ReceiveData();

            if (receivedData is null)
            {
                Console.WriteLine("No response received for handshake.");
                return;
            }
            
            Console.WriteLine("Handshake response received: " + BitConverter.ToString(receivedData));

            // 0xa4 0xea 0x04 0x00 
            byte[] changeData = { 0xa4, 0xea, 0x04, 0x00 };
            byte[] paddedChangeData = AddPadding(changeData);
            SendData(paddedChangeData);
            Console.WriteLine("Notify change sent.");
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
            byte[] paddedData = new byte[BufferSize];
            Array.Copy(data, 0, paddedData, 0, data.Length);
            return paddedData;
        }

        private byte[]? ReceiveData()
        {
            if (_socketClient is { Connected: true })
            {
                var stream = _socketClient.GetStream();
                byte[] buffer = new byte[BufferSize];
                int bytesRead = 0;

                try
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading data: " + ex.Message);
                    return null;
                }

                if (bytesRead > 0)
                {
                    byte[] receivedData = new byte[bytesRead];
                    Array.Copy(buffer, receivedData, bytesRead);
                    return receivedData;
                }
            }

            Console.WriteLine("Socket not connected or no data received.");
            return null;
        }

        public void CloseSocket()
        {
            _socketClient.Close();
            Console.WriteLine("Socket closed.");
        }
    }
}
