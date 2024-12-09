from socket import socket, AF_INET, SOCK_STREAM, SOL_SOCKET, SO_REUSEADDR
from select import select

INVM = -1
NOOP = 0
CONH = 1
CONR = 2
TAKN = 3

class Message(object):
    def __init__(self, message_type = NOOP, payload = b"") -> None:
        self.type = message_type
        self.payload = payload

    def to_bytes(self) -> bytes:
        base_message: bytearray = bytearray()
        final_message: bytearray = bytearray()

        base_message.extend(self.payload)

        final_message.extend(b"\xa4\xea")
        final_message.extend(self.type.to_bytes(1, "big"))
        final_message.extend(len(base_message).to_bytes(8, "big"))
        final_message.extend(base_message)
        return (final_message)

    @staticmethod
    def from_byte(_b: bytes) -> object:
        if (_b[:2] != b"\xa4\xea"):
            return (Message(INVM))

        message_type = int.from_bytes(_b[2:3], "big")
        message_size = int.from_bytes(_b[3:11], "big")

        payload = _b[11:11 + message_size]

        return (Message(message_type, payload))

class AreaConnect(object):
    def __init__(self, socket_base: socket = socket, host: str = "0.0.0.0", port: int = 2727):
        self.host = host
        self.port = port

        self.closed = False
        self.connected = None

        self.socket: socket = socket_base(AF_INET, SOCK_STREAM)
        self.socket.setsockopt(SOL_SOCKET, SO_REUSEADDR, 1)

    def set_listen(self, backport: int = 5):
        self.socket.bind((self.host, self.port))
        self.socket.listen(backport)

    def set_connect(self):
        self.socket.connect((self.host, self.port))
        self.connected = self.socket

        self.send_message(Message(CONH))

        response = self.get_message()

        if (response.type != CONR):
            self.close_client()
            return

    def send_message(self, message: Message = Message(NOOP)):
        if (self.connected):
            self.connected.sendall(message.to_bytes() + (b"\0" * (1024 - len(message.to_bytes()))))

    def get_message(self) -> Message:
        if (self.connected):
            message: bytes = self.connected.recv(1024)

            if (Message.from_byte(message) == INVM):
                return (Message(INVM))
            return (Message.from_byte(message))
        return (None)

    def accept(self):
        self.connected = self.socket.accept()[0]

        response = self.get_message()

        if (response.type != CONH):
            self.close_client()
            return

        self.send_message(Message(CONR))

    def throw_away(self):
        client_socket, address = self.socket.accept()

        client_socket.write(Message(TAKN, b"service already connected").to_bytes())

        client_socket.close()

    def accept_if_not_connected(self):
        if (not self.connected):
            self.accept()
        else:
            self.throw_away()

    def close_client(self):
        if (self.connected):
            self.connected.close()
            self.connected = None

    def close(self):
        self.close_client()
        self.socket.close()

    def __del__(self):
        self.close()
