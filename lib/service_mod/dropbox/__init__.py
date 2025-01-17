from serviceconf import Action
import json

def strategy_file_search(register: object, data: bytes):
    searched_file = register.vars["file_name"]
    if data:
        json_data = json.loads(data)
        for match in json_data["matches"]:
            if match["metadata"]["metadata"]["name"] == searched_file:
                if not register.cache:
                    register.cache = "file_found"
                    return ("file_found")
                return (None)
        register.cache = None
    return (None)
