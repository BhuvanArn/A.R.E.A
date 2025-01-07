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

def middleware_on_time_daily(register: object, action: Action, discard, valid, data = None):
    actual_time = datetime.now()
    looking_for = datetime.fromisoformat(f'{actual_time.year}-{actual_time.month:02}-{actual_time.day:02}T{register.vars["marker"]}Z')

    if (actual_time.timestamp() >= looking_for):
        valid(actual_time)
        return
    valid(None)

def strategy_on_time_daily(register: object, data: bytes):
    if (register.cache and data and (datetime.fromisoformat(register.cache).weekday() == data.weekday())):
        return (None)

    if (data):
        register.cache = data.isoformat()
        return (register.cache)
    else:
        return (None)

def middleware_on_date_monthly(register: object, action: Action, discard, valid, data = None):
    actual_time = datetime.now()
    looking_for = datetime.fromisoformat(f'{actual_time.year}-{actual_time.month:02}-{register.vars["marker"]}Z')

    if (actual_time.timestamp() >= looking_for):
        valid(actual_time)
        return
    valid(None)

def strategy_on_date_monthly(register: object, data: bytes):
    if (register.cache and data and (datetime.fromisoformat(register.cache).month == data.month)):
        return (None)

    if (data):
        register.cache = data.isoformat()
        return (register.cache)
    else:
        return (None)


def middleware_on_date_yearly(register: object, action: Action, discard, valid, data = None):
    actual_time = datetime.now()
    looking_for = datetime.fromisoformat(f'{actual_time.year}-{register.vars["marker"]}Z')

    if (actual_time.timestamp() >= looking_for):
        valid(actual_time)
        return
    valid(None)

def strategy_on_date_yearly(register: object, data: bytes):
    if (register.cache and data and (datetime.fromisoformat(register.cache).year == data.year)):
        return (None)

    if (data):
        register.cache = data.isoformat()
        return (register.cache)
    else:
        return (None)

def middleware_on_date_yearly(register: object, action: Action, discard, valid, data = None):
    actual_time = datetime.now()

    if (("monday", "tuesday", "wednesay", "thursday", "friday", "saturday", "sunday")[actual_time.weekday()] == register.vars["marker"].tolower()):
        valid(actual_time)
        return
    valid(None)

def strategy_on_date_yearly(register: object, data: bytes):
    if (register.cache and data and (datetime.fromisoformat(register.cache).isocalendar()[1] == data.isocalendar()[1])):
        return (None)

    if (data):
        register.cache = data.isoformat()
        return (register.cache)
    else:
        return (None)
