from threading import Thread
from importlib import import_module

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

class GenericModRegister(object):
    def __init__(self):
        self.mods = {}

    def load_module(self, name: str, real_name: str):
        try:
            self.mods[name] = getattr(import_module('.'.join(real_name.split('.')[:-1])), real_name.split('.')[-1])
        except Exception as e:
            print(f"[MODULE] - {real_name} - {e}", flush=True)

    def get_module(self, name: str):
        return (self.mods[name] if name in self.mods else None)
