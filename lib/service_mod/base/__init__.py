from serviceconf import Action
from json import loads
from hashlib import sha256
from requests import get, post, patch, put, delete

METHODS = {
    "get": get,
    "post": post,
    "patch": patch,
    "put": put,
    "delete": delete
}

def log_middelware_exemple(register: object, action: Action, discard, valid, data = None):
    print(f"[LOG MIDDLEWARE ({action.name})] - {str(data)[:100]}", flush=True)
    valid(data)

def hash_strategy_base_handler(register: object, data: bytes):
    if (not isinstance(data, (bytes, bytearray))):
        data = str(data).encode()

    data = bytes(data)

    if (data is None):
        register.cache = data

    if (not register.cache or register.cache != sha256(data, usedforsecurity=False).hexdigest()):
        register.cache = sha256(data, usedforsecurity=False).hexdigest()
        print(register.cache, flush=True)

        return (register.cache)

    return (None)

def endpoint_base_handler(register: object, action: Action, discard, valid, data = None):
    if (isinstance(action, Action)):
        if (action.fetch.endpoint.method.lower() not in METHODS):
            return discard(f"invalid fetch method {action.fetch.endpoint.method.upper()}")

        url = action.fetch.endpoint.url
        headers = action.fetch.endpoint.headers
        data = action.fetch.endpoint.data

        for item in register.vars:
            url = url.replace(f"$+{item}", register.vars[item])
            headers = headers.replace(f"$+{item}", register.vars[item])
            if (data):
                data = data.replace(f"$+{item}", register.vars[item])

        headers = loads(headers)

        try:
            req = METHODS[action.fetch.endpoint.method.lower()](url, data=data, headers=headers)
        except Exception as e:
            return (discard(f"errror requesting {url} ({e})"))

        if (req.status_code not in (200, 201)):
            return discard(f"error requesting {url} (invalid status_code {req.status_code})")

        valid(req.text)
        req.close()
    else:
        if (action.commit.endpoint.method.lower() not in METHODS):
            return discard(f"invalid commit method {action.commit.endpoint.method.upper()}")

        url = action.commit.endpoint.url
        headers = action.commit.endpoint.headers
        data = action.commit.endpoint.data

        for item in register.vars:
            url = url.replace(f"$+{item}", register.vars[item])
            headers = headers.replace(f"$+{item}", register.vars[item])
            if (data):
                data = data.replace(f"$+{item}", register.vars[item])

        headers = loads(headers)

        try:
            req = METHODS[action.commit.endpoint.method.lower()](url, data=data, headers=headers)
        except Exception as e:
            return (discard(f"errror requesting {url} ({e})"))

        if (req.status_code not in (200, 201)):
            return discard(f"error requesting {url} (invalid status_code {req.text} {req.status_code})")

        valid(req.text)
        req.close()