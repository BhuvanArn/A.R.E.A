from serviceconf import Action
from time import time
from datetime import datetime

def middleware_only_time(register: object, action: Action, discard, valid, data = None):
    valid(datetime.now().timestamp() > datetime.fromisoformat(register.vars["marker"]).timestamp())

def strategy_only_time(register: object, data: bytes):
    if (register.cache):
        return (None)

    if (data):
        register.cache = str(data)
        return (register.cache)
    else:
        return (None)
