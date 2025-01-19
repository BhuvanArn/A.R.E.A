import json

def strategy_new_post_in_subreddit(register: object, data: bytes):
    if data:
        json_data = json.loads(data)
        if len(json_data["data"]["children"]) != 1:
            return (None)
        new_post_title = json_data["data"]["children"][0]["data"]["title"]
        if new_post_title:
            if not register.cache:
                register.cache = new_post_title
                return (None)
            elif new_post_title != register.cache:
                register.cache = new_post_title
                return(f"new post title : {new_post_title}")
    return (None)
