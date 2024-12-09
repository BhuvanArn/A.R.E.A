from sys import exit
from select import select
from protocol import *
from serviceconf import *
from threading import Thread
from time import time
from requests import get, post, patch, put, delete
from json import loads
from hashlib import sha256

METHODS = {
    "get": get,
    "post": post,
    "patch": patch,
    "put": put,
    "delete": delete
}

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

class RegisteredAction(object):
    def __init__(self, service_id, service, name, _vars = {}):
        self.service_id = service_id
        self.service = service
        self.name = name

        self.vars = _vars

        self.worker: Worker = None

        self.last_update = 0

        self.cache = None

    def _run_action(self, service_config):
        action: Action = service_config.get_service(self.service).get_action(self.name)

        if (not action.fetch):
            self.last_update = time()
            return

        if (action.fetch.endpoint):
            if (action.fetch.endpoint.method.lower() not in METHODS):
                print(f"[PYTHON (service-action)] - invalid fetch method {action.fetch.endpoint.method.upper()}", flush=True)
                self.last_update = time()
                return

            url = action.fetch.endpoint.url
            headers = action.fetch.endpoint.headers
            data = action.fetch.endpoint.data

            for item in self.vars:
                url = url.replace(f"$+{item}", self.vars[item])
                headers = headers.replace(f"$+{item}", self.vars[item])
                if (data):
                    data = data.replace(f"$+{item}", self.vars[item])

            headers = loads(headers)

            req = METHODS[action.fetch.endpoint.method.lower()](url, data=data, headers=headers)

            if (req.status_code not in (200, 201)):
                print(f"[PYTHON (service-action)] - error requesting {url}", flush=True)
                self.last_update = time()
                return

            if (action.fetch.strategy == "hash"):
                if (not self.cache or self.cache != sha256(req.text.encode(), usedforsecurity=False).hexdigest()):
                    self.cache = sha256(req.text.encode(), usedforsecurity=False).hexdigest()
                    print(self.cache, flush=True)
            req.close()

        self.last_update = time()

    def start_worker(self, service_config: Config):
        if (self.worker and not self.worker.stopped):
            return

        if (service_config.get_service(self.service).get_action(self.name).time >= (time() - self.last_update)):
            return

        self.worker = Worker(lambda: self._run_action(service_config))
        self.worker.start()

class Watcher(object):
    def __init__(self):
        self.services = Config("/var/service_storage/services.yml")
        self.registered_actions = []

    def watch(self):
        for item in self.registered_actions:
            item.start_worker(self.services)

    def register_action(self, action):
        self.registered_actions.append(action)

def main() -> int:
    print(f"[PYTHON (service-action)] - starting...", flush=True)

    connection = AreaConnect()
    connection.set_listen()

    watcher = Watcher()
    watcher.register_action(RegisteredAction("2343354", "discord", "new_message", {"channel_id": "1303739948171526246", "token": "le_token_del_reffe"}))

    print(f"[PYTHON (service-action)] - lisening on {connection.port}", flush=True)

    while 1:
        try:
            read_ready_sockets, _, _ = select([connection.socket], [], [], 0)

            if (read_ready_sockets):
                connection.accept_if_not_connected()
                print(f"[PYTHON (service-action)] - connected to reaction", flush=True)

            if (connection.connected):
                read_ready_sockets, _, _ = select([connection.connected], [], [], 0)

                if (read_ready_sockets):
                    message = connection.get_message()

                    if (message.type == INVM):
                        connection.close_client()
                        print(f"[PYTHON (service-action)] - reaction disconnected", flush=True)

            watcher.watch()
        except KeyboardInterrupt:
            break
        except Exception as e:
            print(f"[PYTHON (service-action)] - {e}", flush=True)
            return (84)

    return (0)

if (__name__ == "__main__"):
    exit(main())
