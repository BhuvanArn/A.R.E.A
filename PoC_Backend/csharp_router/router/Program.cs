using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace router;

class Program
{
    static readonly byte[] HeaderTerminator = "\r\n\r\n"u8.ToArray();
    static readonly byte[] ContentLengthHeader = "Content-Length: "u8.ToArray();
    static readonly byte[] CRLF = "\r\n"u8.ToArray();
    static readonly byte[] GetMethodBytes = "GET"u8.ToArray();
    static readonly byte[] PostMethodBytes = "POST"u8.ToArray();

    static readonly byte[] ResponseBytes = "HTTP/1.1 200 OK\r\nContent-Length: 21\r\n\r\nhello from the server"u8.ToArray();

    static readonly byte[] Error400Bytes = "HTTP/1.1 400 Bad Request\r\nContent-Length: 0\r\n\r\n"u8.ToArray();

    static readonly byte[] Error411Bytes = "HTTP/1.1 411 Length Required\r\nContent-Length: 0\r\n\r\n"u8.ToArray();

    static readonly byte[] Error413Bytes = "HTTP/1.1 413 Payload Too Large\r\nContent-Length: 0\r\n\r\n"u8.ToArray();
    
    static async Task Main(string[] args)
    {
        await RunServerAsync();
    }

    static async Task RunServerAsync()
    {
        var endpoint = new IPEndPoint(IPAddress.Any, 6969);
        var listener = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        listener.Bind(endpoint);
        listener.Listen(1000);

        Console.WriteLine("Server is listening on port 6969");

        while (true)
        {
            var clientSocket = await listener.AcceptAsync();
            _ = Task.Run(() => HandleClient(clientSocket));
        }
    }

    static void HandleClient(Socket clientSocket)
    {
        const int bufferSize = 8192;
        var buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
        var bytesReceived = 0;

        try
        {
            var headerEndIndex = -1;

            while (true)
            {
                var bytesRead =
                    clientSocket.Receive(buffer, bytesReceived, bufferSize - bytesReceived, SocketFlags.None);
                if (bytesRead == 0)
                    break;

                bytesReceived += bytesRead;

                headerEndIndex = IndexOf(buffer, HeaderTerminator, 0, bytesReceived);

                if (headerEndIndex != -1)
                    break;

                if (bytesReceived == bufferSize)
                {
                    clientSocket.Send(Error413Bytes);
                    return;
                }
            }

            if (headerEndIndex == -1)
            {
                clientSocket.Send(Error400Bytes);
                return;
            }

            var requestLineEndIndex = IndexOf(buffer, CRLF, 0, bytesReceived);
            if (requestLineEndIndex == -1)
            {
                clientSocket.Send(Error400Bytes);
                return;
            }

            var methodEndIndex = Array.IndexOf(buffer, (byte)' ', 0, requestLineEndIndex);
            if (methodEndIndex == -1)
            {
                clientSocket.Send(Error400Bytes);
                return;
            }

            var methodSpan = new ArraySegment<byte>(buffer, 0, methodEndIndex);

            var isGet = SequenceEqual(methodSpan, GetMethodBytes);
            var isPost = SequenceEqual(methodSpan, PostMethodBytes);

            int contentLength;
            var totalHeaderLength = headerEndIndex + 4;

            if (isPost)
            {
                var contentLengthHeaderIndex = IndexOf(buffer, ContentLengthHeader, 0, bytesReceived);

                if (contentLengthHeaderIndex != -1)
                {
                    var contentLengthValueStart = contentLengthHeaderIndex + ContentLengthHeader.Length;
                    var contentLengthValueEnd = Array.IndexOf(buffer, (byte)'\r', contentLengthValueStart,
                        bytesReceived - contentLengthValueStart);

                    if (contentLengthValueEnd == -1)
                    {
                        clientSocket.Send(Error400Bytes);
                        return;
                    }

                    var contentLengthValueLength = contentLengthValueEnd - contentLengthValueStart;
                    var contentLengthValueSegment =
                        new ArraySegment<byte>(buffer, contentLengthValueStart, contentLengthValueLength);
                    contentLength = ParseIntFromAscii(contentLengthValueSegment);

                    if (contentLength == -1)
                    {
                        clientSocket.Send(Error400Bytes);
                        return;
                    }

                    var bodyBytesAvailable = bytesReceived - totalHeaderLength;
                    var totalBodyBytesNeeded = contentLength - bodyBytesAvailable;

                    while (totalBodyBytesNeeded > 0)
                    {
                        if (bytesReceived == bufferSize)
                        {
                            clientSocket.Send(Error413Bytes);
                            return;
                        }

                        var bytesRead = clientSocket.Receive(buffer, bytesReceived, bufferSize - bytesReceived,
                            SocketFlags.None);
                        if (bytesRead == 0)
                            break;

                        bytesReceived += bytesRead;
                        totalBodyBytesNeeded -= bytesRead;
                    }
                }
                else
                {
                    clientSocket.Send(Error411Bytes);
                    return;
                }
            }

            clientSocket.Send(ResponseBytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex}");
        }
        finally
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    static int ParseIntFromAscii(ArraySegment<byte> segment)
    {
        if (segment.Array == null)
        {
            return -1;
        }
        
        var value = 0;
        for (var i = 0; i < segment.Count; i++)
        {
            var b = segment.Array[segment.Offset + i];
            if (b >= '0' && b <= '9')
                value = value * 10 + (b - '0');
            else
                return -1;
        }

        return value;
    }

    private static int IndexOf(byte[] buffer, byte[] value, int startIndex, int count)
    {
        var endIndex = startIndex + count - value.Length + 1;
        for (var i = startIndex; i < endIndex; i++)
        {
            var found = !value.Where((t, j) => buffer[i + j] != t).Any();
            if (found)
                return i;
        }

        return -1;
    }

    static bool SequenceEqual(ArraySegment<byte> segment, byte[] value)
    {
        if (segment.Array == null)
        {
            return false;
        }
        
        if (segment.Count != value.Length)
            return false;

        return !value.Where((t, i) => segment.Array[segment.Offset + i] != t).Any();
    }
}