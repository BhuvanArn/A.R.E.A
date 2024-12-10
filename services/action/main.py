from sys import exit
from select import select
from protocol import *
from serviceconf import *
from time import time
from service_mod import *

class RegisteredAction(object):
    def __init__(self, service_id, action_id, service, name, _vars = {}):
        self.service_id = service_id
        self.action_id = action_id

        self.service = service
        self.name = name

        self.vars = _vars

        self.worker: Worker = None

        self.last_update = 0

        self.cache = None

    def _discard(self, watcher, action, message, update=True):
        print(f"[PYTHON (service-action)] - {message}", flush=True)
        if (update):
            self.last_update = time()

    def _valid(self, watcher, action, data, update=True):
        if (not watcher.get_module_strategy(action.fetch.strategy)):
            return (self._discard(watcher, action, f"strategy {action.fetch.strategy} not found or not set, skipping."))

        value = watcher.get_module_strategy(action.fetch.strategy)(self, data)

        if (not value):
            self.last_update = time()
            return

        if (update):
            self.last_update = time()

            if (not watcher.connection.connected):
                watcher.waiting_requests.append(Message(ACTI, f"{self.service_id} {self.service} {self.action_id} {self.name} {str(value)}".encode()))
            else:
                watcher.connection.send_message(Message(ACTI, f"{self.service_id} {self.service} {self.action_id} {self.name} {str(value)}".encode()))
        else:
            print(f"[PYTHON (service-action)] - update_done", flush=True)

    def _build_middleware(self, watcher, action, middleware, update = True):
        return lambda x, v, f=middleware: f(self, action, lambda m: self._discard(watcher, action, m, update), x, v)

    def _run_action(self, watcher):
        action: Action = watcher.services.get_service(self.service).get_action(self.name)
        middlewares = []

        if (not action.fetch):
            self._discard(watcher, action, "no fetch entry found, skipping.")

        if (action.fetch.endpoint):
            middlewares.append(self._build_middleware(watcher, action, watcher.get_module_middleware("endpoint")))

        if (action.fetch.middleware):
            if (watcher.get_module_middleware(action.fetch.middleware)):
                middlewares.append(self._build_middleware(watcher, action, watcher.get_module_middleware(action.fetch.middleware)))

        middlewares.append(lambda x: self._valid(watcher, action, x))

        for i in range(len(middlewares) - 2, -1, -1):
            call = middlewares[i]

            middlewares[i] = lambda x, f=middlewares[i + 1], c=call: c(f, x)

        middlewares[0](None)

    def _update_action(self, watcher):
        action: Action = watcher.services.get_service(self.service).get_action(self.name)
        middlewares = []

        if (not action.fetch):
            self._discard(watcher, action, "no fetch entry found, skipping.")

        if (action.fetch.endpoint):
            middlewares.append(self._build_middleware(watcher, action, watcher.get_module_middleware("endpoint"), False))

        if (action.fetch.middleware):
            if (watcher.get_module_middleware(action.fetch.middleware)):
                middlewares.append(self._build_middleware(watcher, action, watcher.get_module_middleware(action.fetch.middleware), False))

        middlewares.append(lambda x: self._valid(watcher, action, x, False))

        for i in range(len(middlewares) - 2, -1, -1):
            call = middlewares[i]

            middlewares[i] = lambda x, f=middlewares[i + 1], c=call: c(f, x)

        middlewares[0](None)

    def update(self, watcher):
        while (self.worker and not self.worker.stopped):
            continue

        self.worker = Worker(lambda: self._update_action(watcher))
        self.worker.start()

    def start_worker(self, watcher: object):
        if (self.worker and not self.worker.stopped):
            return

        if (watcher.services.get_service(self.service).get_action(self.name).time >= (time() - self.last_update)):
            return

        self.worker = Worker(lambda: self._run_action(watcher))
        self.worker.start()

class Watcher(object):
    def __init__(self, connection):
        self.services = Config("/var/service_storage/services.yml")
        self.registered_actions = []
        self.waiting_requests = []
        self.connection = connection
        self.mods_strategy = GenericModRegister()
        self.mods_middleware = GenericModRegister()

        self.load_module_strategy("hash", "base.hash_strategy_base_handler")
        self.load_module_middleware("endpoint", "base.endpoint_base_handler")
        self.load_module_middleware("log", "base.log_middelware_exemple")

    def load_module_strategy(self, name: str, real_name: str):
        self.mods_strategy.load_module(name, "service_mod." + real_name)

    def load_module_middleware(self, name: str, real_name: str):
        self.mods_middleware.load_module(name, "service_mod." + real_name)

    def get_module_strategy(self, name: str):
        if (not self.mods_strategy.get_module(name)):
            self.mods_strategy.load_module(name, name)
        else:
            return (self.mods_strategy.get_module(name))

    def get_module_middleware(self, name: str):
        if (not self.mods_middleware.get_module(name)):
            self.mods_middleware.load_module(name, name)
        else:
            return (self.mods_middleware.get_module(name))

    def update(self, name_id):
        for item in self.registered_actions:
            if (item.action_id == name_id):
                item.update(self)

    def watch(self):
        if (self.waiting_requests and self.connection.connected):
            for i, item in enumerate(self.waiting_requests):
                self.connection.send_message(item)
                self.waiting_requests[i] = None
            self.waiting_requests = list(filter(lambda x: x is not None, self.waiting_requests))

        for item in self.registered_actions:
            item.start_worker(self)

    def register_action(self, action):
        self.registered_actions.append(action)

def main() -> int:
    print(f"[PYTHON (service-action)] - starting...", flush=True)

    connection = AreaConnect()
    connection.set_listen()

    watcher = Watcher(connection)
    watcher.register_action(RegisteredAction("2343354", "45543", "discord", "new_message", {"channel_id": "1303739948171526246", "token": ""}))

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

                    if (message.type == ACTI):
                        name_id = message.payload.split(b' ')[0].decode()

                        watcher.update(name_id)

            watcher.watch()
        except KeyboardInterrupt:
            break
        except Exception as e:
            print(f"[PYTHON (service-action)] - {e}", flush=True)
            return (84)

    return (0)

if (__name__ == "__main__"):
    exit(main())