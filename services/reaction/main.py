from sys import exit
from select import select
from protocol import *
from time import sleep
from serviceconf import *
from service_mod import *
from requests import get
from json import dumps, loads
from signal import signal, SIGTERM

class RegisteredReaction(object):
    def __init__(self, service_id, reaction_id, linked_to, service, name, _vars = {}):
        self.service_id = service_id
        self.reaction_id = reaction_id
        self.linked_to = linked_to

        self.service = service
        self.name = name

        self.vars = _vars

        self.workers: list = []

    def _discard(self, handler, reaction, message):
        print(f"[PYTHON (service-reaction)] - {message}", flush=True)
        self.workers = list(filter(lambda x: not x.stopped, self.workers))

    def _valid(self, handler, reaction, data):
        if (reaction.redo_action):
            handler.connection.send_message(Message(ACTI, f"{self.linked_to}".encode()))

        self.workers = list(filter(lambda x: not x.stopped, self.workers))

    def _build_middleware(self, handler, reaction, middleware):
        return lambda x, v, f=middleware: f(self, reaction, lambda m: self._discard(handler, reaction, m), x, v)

    def _run_reaction(self, handler, data):
        reaction: Reaction = handler.services.get_service(self.service).get_reaction(self.name)
        middlewares = []

        if (not reaction):
            self._discard(handler, reaction, "reaction not found.")
            return

        if (not reaction.commit):
            self._discard(handler, reaction, "no commit entry found, skipping.")
            return

        if (reaction.commit.endpoint):
            middlewares.append(self._build_middleware(handler, reaction, handler.get_module_middleware("endpoint")))

        if (reaction.commit.middleware):
            if (handler.get_module_middleware(reaction.commit.middleware)):
                middlewares.append(self._build_middleware(handler, reaction, handler.get_module_middleware(reaction.commit.middleware)))

        middlewares.append(lambda x: self._valid(handler, reaction, x))

        for i in range(len(middlewares) - 2, -1, -1):
            call = middlewares[i]

            middlewares[i] = lambda x, f=middlewares[i + 1], c=call: c(f, x)

        middlewares[0](data)

    def start_worker(self, handler: object, data):
        temp = Worker(lambda d = data: self._run_reaction(handler, d))
        temp.start()
        self.workers.append(temp)

class Handler(object):
    def __init__(self, connection):
        self.services = Config("/var/service_storage/services.yml")
        self.registered_reactions = []
        self.connection = connection

        self.failed_update = False

        self.mods_middleware = GenericModRegister()

        self.load_module_middleware("endpoint", "base.endpoint_base_handler")
        self.load_module_middleware("log", "base.log_middelware_exemple")

    def activate(self, action_id, data):
        for item in self.registered_reactions:
            if (item.linked_to == action_id):
                item.start_worker(self, data)

    def load_module_middleware(self, name: str, real_name: str):
        self.mods_middleware.load_module(name, "service_mod." + real_name)

    def get_module_middleware(self, name: str):
        if (not self.mods_middleware.get_module(name)):
            self.load_module_middleware(name, name)

        return (self.mods_middleware.get_module(name))

    def register_reaction(self, reaction):
        self.registered_reactions.append(reaction)

    def from_request(self, url):
        self.failed_update = True

        self.registered_reactions.clear()

        req = get(url)

        if (req.status_code not in (200, 201)):
            raise RuntimeError(f"invalid status {req.status_code} {req.text}")

        json = req.json()

        for item in json:
            for action in item["reactions"]:
                _vars = {}

                #for var_name, var_value in {**loads(action["executionConfig"]), **loads(item["auth"])}.items():
                for var_name, var_value in {**loads(action["executionConfig"]), **item["auth"]}.items():
                    _vars[var_name] = var_value

                self.register_reaction(RegisteredReaction(item["id"], action["id"], action["actionId"], item["name"], action["name"], _vars))

        req.close()

        self.failed_update = False

class ReactionService(object):
    def __init__(self):
        self.connection = AreaConnect(host="service-action")
        self.handler = Handler(self.connection)

        self.running = False

        signal(SIGTERM, lambda: self.close())

        self._update_datas()
        self._connect_action()

    def _update_datas(self):
        retry = 0
        start = True

        while ((start or self.handler.failed_update)) and retry < 3:
            start = False
            try:
                self.handler.from_request("http://csharp_service:8080/area")
                print(f"[PYTHON (service-reaction)] - updated data", flush=True)
            except Exception as e:
                print(f"[PYTHON (service-reaction)] - failed to update db datas {e}", flush=True)
                retry += 1
                sleep(7.4)

        self.running = not self.handler.failed_update

    def _connect_action(self):
        connected = False
        retry = 0

        while (not connected) and retry < 3:
            try:
                self.connection.set_connect()
                if (not self.connection.connected):
                    raise RuntimeError("invalid service")
                connected = True
            except Exception as e:
                retry += 1
                print(f"[PYTHON (service-reaction)] - Failed to connect to service-action:2727 ({e}), retrying...", flush=True)
                sleep(7.4)

        self.running = connected

        if (not connected):
            print(f"[PYTHON (service-reaction)] - failed to connect to service-action:2727", flush=True)
            return

        print(f"[PYTHON (service-reaction)] - connected to service-action on port 2727", flush=True)

    def _handle_request_from_action(self):
        message = self.connection.get_message()

        if (message.type == INVM):
            print(f"[PYTHON (service-reaction)] - action disconnected, closing", flush=True)
            self.close()

        if (message.type == ACTI):
            service_id = message.payload.split(b' ')[0].decode()
            service = message.payload.split(b' ')[1].decode()
            name_id = message.payload.split(b' ')[2].decode()
            name = message.payload.split(b' ')[3].decode()
            data = b' '.join(message.payload.split(b' ')[4:]).decode()

            self.handler.activate(name_id, data)

        if (message.type == UPDT):
            print(f"[PYTHON (service-reaction)] - receive update request from action", flush=True)
            try:
                self.handler.from_request("http://csharp_service:8080/area")
            except Exception as e:
                print(f"[PYTHON (service-reaction)] - failed to update db datas {e}", flush=True)

    def mainloop(self):
        while self.running:
            read_ready_sockets, _, _ = select([self.connection.connected], [], [], None)

            if (read_ready_sockets):
                self._handle_request_from_action()

        print(f"[PYTHON (service-reaction)] - stopping service.", flush=True)

    def close(self):
        self.running = False

        self.connection.close()

def main() -> int:
    print(f"[PYTHON (service-reaction)] - starting...", flush=True)

    rs: ReactionService = ReactionService()

    #rs.handler.register_reaction(RegisteredReaction("328", "0002", "45543", "test", "log", {}))
    # args = {
    #     "channel_id": "1314613132894670981",
    #     "message": "test reaction from reaction-service.",
    #     "token": "discord_token"
    # }
    # rs.handler.register_reaction(RegisteredReaction("2343354", "0002", "45543", "discord", "send_message", args))
    # args = {
    #     "album_ids": '["2kz6FGzMkZUyGZPywlkcOu"]',
    #     "token": "token_fetched_from_api"
    # }
    # rs.handler.register_reaction(RegisteredReaction("328", "0003", "45543", "spotify", "save_albums", args))

    try:
        rs.mainloop()
    except KeyboardInterrupt:
        pass
    except Exception as e:
        print(f"[PYTHON (service-reaction)] - {e}", flush=True)
        rs.close()
        return (84)

    rs.close()

    return (0)

if (__name__ == "__main__"):
    exit(main())
