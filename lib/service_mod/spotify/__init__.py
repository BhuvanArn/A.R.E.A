from serviceconf import Action
import json

def middleware_playback_playing(register: object, action: Action, discard, valid, data = None):
    if data:
        json_data = json.loads(data)
        if json_data["is_playing"] == True:
            valid(json_data["is_playing"])
        else:
            register.cache = None
    valid(None)

def strategy_playback_playing(register: object, data: bytes):
    if (register.cache and data):
        return (None)
    if (data):
        register.cache = data
        return (True)
    return (None)
