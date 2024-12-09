from yaml import safe_load

class Input(object):
    def __init__(self, name: str, regex: str):
        self.name: str = name
        self.regex: str = regex

    def __repr__(self) -> str:
        return (self.__str__())

    def __str__(self) -> str:
        return (f"{self.name}: {self.regex}")

class Endpoint(object):
    def __init__(self, url: str, method: str, headers: str, data: str):
        self.url: str = url
        self.method: str = method
        self.headers: str = headers
        self.data: str = data

    def __repr__(self) -> str:
        return (self.__str__())

    def __str__(self) -> str:
        return (f"{self.method.upper()} {self.url}")

class Fetch(object):
    def __init__(self, strategy: str, endpoint: Endpoint):
        self.strategy: str = strategy

        self.endpoint: Endpoint = endpoint

    def __repr__(self) -> str:
        return (self.__str__())

    def __str__(self) -> str:
        return (f"strategy({self.strategy}) {self.endpoint}")

class Commit(object):
    def __init__(self, endpoint: Endpoint):
        self.endpoint: Endpoint = endpoint

    def __repr__(self) -> str:
        return (self.__str__())

    def __str__(self) -> str:
        return (f"{self.endpoint}")

class Reaction(object):
    def __init__(self, name: str, description: str, commit: Commit):
        self.name: str = name
        self.description: str = description

        self.commit: Commit = commit

        self.inputs: list = []

    def __repr__(self) -> str:
        return (self.__str__())

    def __str__(self) -> str:
        inputs = '\n'.join(map(lambda x: '  ' + '\n    '.join(str(x).split('\n')), self.inputs))
        return (f"{self.name} ({self.description}):\ncommit: {str(self.commit)}\ninputs:\n{inputs}")

    def add_input(self, _input: Input):
        self.inputs.append(_input)

class Action(object):
    def __init__(self, name: str, description: str, time: float, fetch: Fetch):
        self.name: str = name
        self.description: str = description

        self.fetch: Fetch = fetch
        self.time = time

        self.inputs: list = []

    def __repr__(self) -> str:
        return (self.__str__())

    def __str__(self) -> str:
        inputs = '\n'.join(map(lambda x: '  ' + '\n    '.join(str(x).split('\n')), self.inputs))
        return (f"{self.name} ({self.description}):\nfetch: {str(self.fetch)}\ninputs:\n{inputs}")

    def add_input(self, _input: Input):
        self.inputs.append(_input)

class Service(object):
    def __init__(self, name: str):
        self.name: str = name

        self.actions: list = []
        self.reactions: list = []

    def __repr__(self) -> str:
        return (self.__str__())

    def __str__(self) -> str:
        actions = '\n'.join(map(lambda x: '  ' + '\n    '.join(str(x).split('\n')), self.actions))
        reactions = '\n'.join(map(lambda x: '  ' + '\n    '.join(str(x).split('\n')), self.reactions))
        return (f"{self.name}:\nactions:\n{actions}\nreactions:\n{reactions}")

    def get_action(self, name: str) -> Action:
        temp = tuple(filter(lambda x: x.name == name, self.actions))

        if (temp):
            return (temp[0])
        return (None)

    def get_reaction(self, name: str) -> Reaction:
        temp = tuple(filter(lambda x: x.name == name, self.reactions))

        if (temp):
            return (temp[0])
        return (None)

    def add_action(self, _action: Action):
        self.actions.append(_action)

    def add_reaction(self, _reaction: Reaction):
        self.reactions.append(_reaction)

class Config(object):
    def __init__(self, path: str):
        self.path: str = path

        self.services: list = []

        with open(self.path, 'r') as fp:
            self._factory(safe_load(fp))

    def __repr__(self) -> str:
        return (self.__str__())

    def __str__(self) -> str:
        services = '\n\n'.join(map(lambda x: '  ' + '\n    '.join(str(x).split('\n')), self.services))
        return (f"config:\n{services}")

    def get_service(self, name: str) -> Service:
        temp = tuple(filter(lambda x: x.name == name, self.services))

        if (temp):
            return (temp[0])
        return (None)

    def _factory(self, datas: dict):
        if ("services" not in datas):
            return

        for item in datas["services"]:
            name = item["name"] if "name" in item else "new_service"
            self.services.append(Service(name))

            if ("actions" in item):
                for action in item["actions"]:
                    name = action["name"] if "name" in action else "new_action"
                    description = action["description"] if "description" in action else "placeholder"
                    time = float(action["time"]) if "time" in action else 5

                    fetch = None
                    if ("fetch" in action):
                        strategy: str = action["fetch"]["strategy"] if "strategy" in action["fetch"] else "custom"
                        endpoint = None

                        if ("endpoint" in action["fetch"]):
                            url = action["fetch"]["endpoint"]["url"] if "url" in action["fetch"]["endpoint"] else "localhost"
                            method = action["fetch"]["endpoint"]["method"] if "method" in action["fetch"]["endpoint"] else "get"
                            headers = action["fetch"]["endpoint"]["headers"] if "headers" in action["fetch"]["endpoint"] else "{}"
                            data =  action["fetch"]["endpoint"]["data"] if "data" in action["fetch"]["endpoint"] else None

                            endpoint = Endpoint(url, method, headers, data)
                        fetch = Fetch(strategy, endpoint)

                    self.services[-1].add_action(Action(name, description, time, fetch))

                    if ("inputs" in action):
                        for _input in action["inputs"]:
                            input_name = _input["name"] if "name" in _input else "new_input"
                            input_regex = _input["regex"] if "regex" in _input else r"^*$"

                            self.services[-1].actions[-1].add_input(Input(input_name, input_regex))

            if ("reactions" in item):
                for reaction in item["reactions"]:
                    name = reaction["name"] if "name" in reaction else "new_reaction"
                    description = reaction["description"] if "description" in reaction else "placeholder"

                    commit = None
                    if ("commit" in reaction):
                        endpoint = None

                        if ("endpoint" in reaction["commit"]):
                            url = reaction["commit"]["endpoint"]["url"] if "url" in reaction["commit"]["endpoint"] else "localhost"
                            method = reaction["commit"]["endpoint"]["method"] if "method" in reaction["commit"]["endpoint"] else "get"
                            headers = reaction["commit"]["endpoint"]["headers"] if "headers" in reaction["commit"]["endpoint"] else "{}"
                            data =  reaction["commit"]["endpoint"]["data"] if "data" in reaction["commit"]["endpoint"] else None

                            endpoint = Endpoint(url, method, headers, data)
                        commit = Commit(endpoint)

                    self.services[-1].add_reaction(Reaction(name, description, commit))

                    if ("inputs" in reaction):
                        for _input in reaction["inputs"]:
                            input_name = _input["name"] if "name" in _input else "new_input"
                            input_regex = _input["regex"] if "regex" in _input else r"^*$"

                            self.services[-1].reactions[-1].add_input(Input(input_name, input_regex))

if (__name__ == "__main__"):
    c = Config("./services.yml")
    print(c)