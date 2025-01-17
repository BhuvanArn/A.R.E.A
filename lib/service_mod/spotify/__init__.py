import json

def strategy_playback_playing_continuously(register: object, data: bytes):
    if data:
        json_data = json.loads(data)
        if json_data["is_playing"] == True:
            return ("playback_action_triggered")
    return (None)

def strategy_playback_playing_once(register: object, data: bytes):
    if data:
        json_data = json.loads(data)
        if json_data["is_playing"] == True:
            if not register.cache:
                register.cache = "playback_action_triggered"
                return ("playback_action_triggered")
        else:
            register.cache = None
    return (None)
