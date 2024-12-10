from sys import exit
from socket import socket, SOL_SOCKET, SO_REUSEADDR, AF_INET, SOCK_STREAM
from src import *
from select import select
from threading import Thread
from json import dumps
from time import time
from serviceconf import *

class TextIOSearch(TextIOBytesLocal):
    def __init__(self, encoding="utf-8", errors="strict", maxlines=9999):
        super().__init__(encoding, errors, maxlines)

    def size(self):
        self.flush()

        return (len(self.file))

    def read_until(self, _s: bytes = b"\r\n\r\n") -> str:
        self.flush()

        temp: bytes = bytes(self.file)
        index: int = temp.find(_s)

        if (index == -1):
            return (None)
        return (temp[:index + len(_s)])

class Client(object):
    def __init__(self, socket, client_address):
        self.socket = socket
        self.client_address = client_address

        self.protocol: str = "HTTP"
        self.method: str = "GET"
        self.route: str = "/"
        self.http_version: list = [1, 1]

        self.headers: dict = {}
        self.body: bytes = b''

        self.buffer: TextIOBytesLocal = TextIOSearch()

        self.closed: bool = False

        self._recv_request()

    def __str__(self) -> str:
        headers = "  " + "\n  ".join(f"{item}: {self.headers[item]}" for item in self.headers)
        return (f"<Client@{self.socket.getsockname()[0]}:{self.socket.getsockname()[1]}-({self.socket.fileno()})\nheaders:\n{headers}\n\nbody:\n  {self.body}\n>")

    def __repr__(self) -> str:
        return (self.__str__())

    def _read_until_eos(self, blck_size = 2048):
        self.buffer.write(self.socket.recv(blck_size))

        while (not self.buffer.read_until(b'\r\n\r\n')):
            self.buffer.write(self.socket.recv(blck_size))

    def _recv_request(self):
        self._read_until_eos()

        for i, item in enumerate(self.buffer.read_until(b'\r\n\r\n').decode().split('\r\n')):
            if (i == 0):
                splitted: list = item.split(' ')

                self.http_version = list(map(int, splitted[2].split('/')[1].split('.')))
                self.protocol = splitted[2].split('/')[0]
                self.method = splitted[0]
                self.route = splitted[1]
                continue

            if (':' not in item):
                continue

            self.headers[item.split(':')[0].lower().strip()] = item.split(':')[1].strip()

        if ("content-length" in self.headers):
            to_read = int(self.headers["content-length"]) - (self.buffer.size() - len(self.buffer.read_until(b'\r\n\r\n')))
            self.buffer.read(len(self.buffer.read_until(b'\r\n\r\n')), True)

            if (to_read):
                self.buffer.write(self.socket.recv(to_read))
                self.buffer.flush()

            self.body = self.buffer.read(destroy=True)

    def close(self):
        if (self.closed):
            return

        self.closed: bool = True
        self.socket.close()

    def __del__(self):
        self.close()


class HTTPServerBase(object):
    def __init__(self, host: str = "0.0.0.0", port: int = 80, socket_base: socket = socket):
        self.host = host
        self.port = port

        self.closed = False

        self.socket: socket = socket_base(AF_INET, SOCK_STREAM)
        self.socket.setsockopt(SOL_SOCKET, SO_REUSEADDR, 1)
        self.socket.bind((self.host, self.port))

    def set_listen(self, backport: int = 14):
        self.socket.listen(backport)

    def accept(self):
        client: Client = Client(*self.socket.accept())
        config: Config = Config("/var/service_storage/services.yml")

        print(client, flush=True)

        json = {
            "client": {
                "host": client.headers["x-router-forwarded"]
            },
            "server": {
                "current_time": int(time()),
                "services": [
                    {
                        "name": item.name,
                        "vars": [
                            {
                                "name": data.name,
                                "regex": data.regex
                            } for data in item.credentials
                        ],
                        "actions": [
                            {
                                "name": action.name,
                                "description": action.description,
                                "inputs": [
                                    {
                                        "name": data.name,
                                        "regex": data.regex
                                    } for data in action.inputs
                                ]
                            } for action in item.actions
                        ],
                        "reactions": [
                            {
                                "name": reaction.name,
                                "description": reaction.description,
                                "inputs": [
                                    {
                                        "name": data.name,
                                        "regex": data.regex
                                    } for data in reaction.inputs
                                ]
                            } for reaction in item.reactions
                        ]
                    } for item in config.services
                ]
            }
        }

        json = dumps(json)

        response = f'HTTP/1.0 200 OK\r\ncontent-type: application/json\r\ncontent-length: {len(json)}\r\n\r\n{json}'

        client.socket.sendall(response.encode())
        client.close()

    def close(self):
        if (self.closed):
            return

        self.closed = True
        self.socket.close()

    def __del__(self):
        self.close()

class Worker(object):
    def __init__(self, callback):
        self.stopped = True
        self.joined = False

        self.thread = Thread(target=self._start)
        self.callback = callback

    def _start(self):
        self.stopped = False
        self.callback()
        self.stopped = True

    def start(self):
        self.stopped = False
        self.thread.start()

    def close(self):
        self.stopped = True
        if (self.joined):
            return

        self.thread.join(5)
        self.joined = True

    def __del__(self):
        self.close()


def main() -> int:
    server = HTTPServerBase("0.0.0.0", 80)
    server.set_listen()
    threads = []
    max_threads = 10

    print(f"[PYTHON (service-about)] - listening on: 0.0.0.0:80", flush=True)
    while 1:
        try:
            read_ready_sockets, _, _ = select([server.socket], [], [], 0)

            if (read_ready_sockets and len(threads) < (max_threads + 1)):
                threads.append(Worker(server.accept))
                threads[-1].start()

            for i, item in enumerate(threads):
                if item.stopped:
                    threads[i] = None

            threads = [item for item in threads if item is not None]
        except KeyboardInterrupt:
            break
        except Exception as e:
            print(e)
    server.close()
    return (0)

if (__name__ == "__main__"):
    exit(main())
