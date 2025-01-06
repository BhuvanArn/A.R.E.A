from serviceconf import Action
from time import time
from datetime import datetime

def middleware_only_time(register: object, action: Action, discard, valid, data = None):
    valid(register.vars["marker"] if datetime.now().timestamp() > datetime.fromisoformat(register.vars["marker"]).timestamp() else None)

def strategy_only_time(register: object, data: bytes):
    if (register.cache and datetime.fromisoformat(register.cache).timestamp() >= datetime.fromisoformat(register.vars["marker"]).timestamp()):
        return (None)

    if (data):
        register.cache = str(data)
        return (register.cache)
    else:
        return (None)


def middleware_every(register: object, action: Action, discard, valid, data = None):
    if ("__time_actual" not in register.vars):
        register.vars["__time_actual"] = time() if not register.cache else float(register.cache)

    actual_time = time()

    if ((actual_time - register.vars["__time_actual"]) >= float(register.vars["time"])):
        valid(actual_time)
        return

    valid(None)

def strategy_every(register: object, data: bytes):
    if (register.cache and (time() - float(register.cache)) < float(register.vars["time"])):
        return (None)

    if (data):
        register.vars["__time_actual"] = data
        register.cache = str(data)
        return (register.cache)
    else:
        return (None)

