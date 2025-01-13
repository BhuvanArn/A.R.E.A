from serviceconf import Action
import json
import sys

def middleware_file_search(register: object, action: Action, discard, valid, data = None):
    searched_file = register.vars["file_name"]
    if data:
        json_data = json.loads(data)
        for match in json_data["matches"]:
            if match["metadata"]["metadata"]["name"] == searched_file:
                valid(match["metadata"]["metadata"]["name"])
                return
        register.cache = None
    valid(None)

def strategy_file_search(register: object, data: bytes):
    if (register.cache and data):
        return (None)
    if (data):
        register.cache = data
        return (True)
    return (None)
