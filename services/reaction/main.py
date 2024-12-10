from sys import exit
from select import select
from protocol import *
from time import sleep
from serviceconf import *
from service_mod import *

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

        if (not reaction.commit):
            self._discard(handler, reaction, "no commit entry found, skipping.")

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
        self.workers.append(Worker(lambda d = data: self._run_reaction(handler, d)))
        self.workers[-1].start()

class Handler(object):
    def __init__(self, connection):
        self.services = Config("/var/service_storage/services.yml")
        self.registered_reactions = []
        self.connection = connection

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
            self.mods_middleware.load_module(name, name)
        else:
            return (self.mods_middleware.get_module(name))

    def register_reaction(self, reaction):
        self.registered_reactions.append(reaction)

def main() -> int:
    print(f"[PYTHON (service-reaction)] - starting...", flush=True)
    connected = False
    retry = 0

    connection = AreaConnect(host="service-action")
    while (not connected) and retry < 3:
        try:
            connection.set_connect()
            if (not connection.connected):
                raise RuntimeError("invalid service")
            connected = True
        except Exception as e:
            retry += 1
            print(f"[PYTHON (service-reaction)] - Failed to connect to service-action:2727 ({e}), retrying...", flush=True)
            sleep(7.4)

    if (not connected):
        print(f"[PYTHON (service-reaction)] - failed to connect to service-action:2727", flush=True)
        return (84)

    print(f"[PYTHON (service-reaction)] - connected to service-action on port 2727", flush=True)

    handler = Handler(connection)
    #handler.register_reaction(RegisteredReaction("2343354", "0002", "45543", "discord", "send_message", {"channel_id": "1303739948171526246", "message": "test reaction from reaction-service.", "token": ""}))

    while 1:
        try:
            read_ready_sockets, _, _ = select([connection.connected], [], [], 0)

            if (read_ready_sockets):
                message = connection.get_message()

                if (message.type == INVM):
                    print(f"[PYTHON (service-reaction)] - action disconnected, closing", flush=True)
                    connection.close_client()
                    return (0)

                if (message.type == ACTI):
                    service_id = message.payload.split(b' ')[0].decode()
                    service = message.payload.split(b' ')[1].decode()
                    name_id = message.payload.split(b' ')[2].decode()
                    name = message.payload.split(b' ')[3].decode()
                    data = b' '.join(message.payload.split(b' ')[4:]).decode()

                    handler.activate(name_id, data)

                    #print(data, flush=True)

                if (connection.get_message() == UPDT):
                    print(f"[PYTHON (service-reaction)] - receive update request from action", flush=True)
        except KeyboardInterrupt:
            break
        except Exception as e:
            print(f"[PYTHON (service-reaction)] - {e}", flush=True)
            return (84)

    return (0)

if (__name__ == "__main__"):
    exit(main())
