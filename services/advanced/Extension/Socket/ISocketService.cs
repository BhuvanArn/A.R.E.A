namespace Extension.Socket;

public interface ISocketService
{
    void OpenSocket();
    void SendHandshakeAndNotifyChange();
    void CloseSocket();
}
