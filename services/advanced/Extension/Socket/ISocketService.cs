namespace Extension.Socket;

public interface ISocketService
{
    void OpenSocket();
    void SendHandshake();
    void NotifyChange();
    void CloseSocket();
}
