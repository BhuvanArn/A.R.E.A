from sys import exit
from select import select
from protocol import *
from serviceconf import *
from time import time
from service_mod import *
from requests import get
from json import dumps
from time import sleep
from genericpath import isdir, isfile
from os import mkdir
from signal import signal, SIGTERM

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

        self.read_cache()

    def write_cache(self):
        if (not isdir("/var/service_storage/caches")):
            mkdir("/var/service_storage/caches")
        if (not isdir(f'/var/service_storage/caches/{self.action_id}')):
            mkdir(f'/var/service_storage/caches/{self.action_id}')
        if (self.cache):
            value = self.cache
            if (isinstance(value, bytes)):
                value = value.decode()
                print(f"[PYTHON (service-action)] - saving bytes data as str, they will be read back as str, this may cause problems in the future when using your cache.", flush=True)
            if (not isinstance(value, str)):
                value = str(value)
                print(f"[PYTHON (service-action)] - saving other type data as str, they will be read back as str, this may cause problems in the future when using your cache.", flush=True)
            with open(f'/var/service_storage/caches/{self.action_id}/cache.tmp', 'w') as fp:
                fp.write(value)

    def read_cache(self):
        if (not isfile(f"/var/service_storage/caches/{self.action_id}/cache.tmp")):
            return

        with open(f'/var/service_storage/caches/{self.action_id}/cache.tmp', 'r') as fp:
            self.cache = fp.read()

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

        if (not action):
            self._discard(watcher, action, "action not found.")
            return

        if (not action.fetch):
            self._discard(watcher, action, "no fetch entry found, skipping.")
            return

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

    def __del__(self):
        self.write_cache()

class Watcher(object):
    def __init__(self, connection):
        self.services = Config("/var/service_storage/services.yml")
        self.registered_actions = []
        self.waiting_requests = []
        self.connection = connection
        self.mods_strategy = GenericModRegister()
        self.mods_middleware = GenericModRegister()

        self.failed_update = False

        self.load_module_strategy("hash", "base.hash_strategy_base_handler")
        self.load_module_middleware("endpoint", "base.endpoint_base_handler")
        self.load_module_middleware("log", "base.log_middelware_exemple")

    def load_module_strategy(self, name: str, real_name: str):
        self.mods_strategy.load_module(name, "service_mod." + real_name)

    def load_module_middleware(self, name: str, real_name: str):
        self.mods_middleware.load_module(name, "service_mod." + real_name)

    def get_module_strategy(self, name: str):
        if (not self.mods_strategy.get_module(name)):
            self.load_module_strategy(name, name)

        return (self.mods_strategy.get_module(name))

    def get_module_middleware(self, name: str):
        if (not self.mods_middleware.get_module(name)):
            self.load_module_middleware(name, name)

        return (self.mods_middleware.get_module(name))

    def from_request(self, url):
        self.failed_update = True

        self.save()

        self.registered_actions.clear()

        req = get(url)

        if (req.status_code not in (200, 201)):
            raise RuntimeError(f"invalid status {req.status_code} {req.text}")

        json = req.json()

        for item in json:
            for action in item["Actions"]:
                _vars = {}

                for var_name, var_value in {**dumps(item["TriggerConfig"]), **dumps(item["Auth"])}.items():
                    _vars[var_name] = var_value

                self.register_action(RegisteredAction(item["Id"], action["Id"], item["Name"], action["Name"], _vars))

        req.close()

        self.failed_update = False

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

    def save(self):
        for item in self.registered_actions:
            item.write_cache()

    def __del__(self):
        self.save()

class ActionService(object):
    def __init__(self):
        self.connection = AreaConnect()
        self.connection.set_listen()

        print(f"[PYTHON (service-action)] - lisening on {self.connection.port}", flush=True)

        self.db_connect = AreaConnect(port=2728)
        self.db_connect.set_listen()

        print(f"[PYTHON (service-action)] - lisening for db on {self.db_connect.port}", flush=True)

        self.watcher = Watcher(self.connection)

        self.running = False

        signal(SIGTERM, lambda: self.close())

        self._update_datas()

    def _update_datas(self):
        retry = 0
        start = True
        while (start or (self.watcher.failed_update)) and retry < 3:
            start = False
            try:
                self.watcher.from_request("http://csharp_service:8080/area")
                print(f"[PYTHON (service-action)] - updated data", flush=True)
            except Exception as e:
                print(f"[PYTHON (service-action)] - failed to update db datas {e}", flush=True)
                retry += 1
                sleep(7.4)

        self.running = not self.watcher.failed_update

    def _handle_request_from_reaction(self):
        message = self.connection.get_message()

        if (message.type == INVM):
            self.connection.close_client()
            print(f"[PYTHON (service-action)] - reaction disconnected (invalid message received)", flush=True)

        if (message.type == ACTI):
            name_id = message.payload.split(b' ')[0].decode()

            self.watcher.update(name_id)

    def _handle_request_from_db(self):
        self.db_connect.accept_if_not_connected()
        print(f"[PYTHON (service-action)] - db requested connection", flush=True)

        message = self.db_connect.get_message()

        if (self.db_connect.get_message() == INVM):
            self.db_connect.close_client()
            print(f"[PYTHON (service-action)] - closing db connect", flush=True)

        if (self.db_connect.get_message() == UPDT):
            print(f"[PYTHON (service-action)] - db requested data update", flush=True)
            try:
                self.watcher.from_request("http://csharp_service:8080/area")
            except Exception as e:
                print(f"[PYTHON (service-action)] - failed to update db datas {e}", flush=True)
            self.db_connect.close_client()
            print(f"[PYTHON (service-action)] - closing db connect", flush=True)
            self.connection.send_message(Message(UPDT))
            print(f"[PYTHON (service-action)] - forwading to reaction", flush=True)

    def mainloop(self):
        while (self.running):
            read_ready_sockets, _, _ = select(
                [self.connection.socket, self.db_connect.socket] if not self.connection.connected else [self.connection.socket, self.db_connect.socket, self.connection.connected]
            , [], [], 0)

            if (self.connection.socket in read_ready_sockets):
                self.connection.accept_if_not_connected()
                print(f"[PYTHON (service-action)] - connected to reaction", flush=True)

            if (self.connection.connected and self.connection.connected in read_ready_sockets):
                self._handle_request_from_reaction()

            self.watcher.watch()

            if (self.db_connect.socket in read_ready_sockets):
                self._handle_request_from_db()

        print(f"[PYTHON (service-action)] - stopping service.", flush=True)

    def close(self):
        self.watcher.save()

        self.db_connect.close()
        self.connection.close()

        self.running = False

def main() -> int:
    print(f"[PYTHON (service-action)] - starting...", flush=True)

    asrv: ActionService = ActionService()

    #watcher.register_action(RegisteredAction("2343354", "45543", "timer", "only_time", {"marker": "2025-01-05T16:09:00.0Z"}))
    asrv.watcher.register_action(RegisteredAction("2343354", "45543", "timer", "every", {"time": "5"}))

    try:
        asrv.mainloop()
    except KeyboardInterrupt:
        pass
    except Exception as e:
        print(f"[PYTHON (service-action)] - {e}", flush=True)
        asrv.close()
        return (84)

    asrv.close()

    return (0)

if (__name__ == "__main__"):
    exit(main())
