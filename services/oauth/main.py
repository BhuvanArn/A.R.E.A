from flask import Flask, request, jsonify, redirect
import requests
import urllib.parse
import base64
import os

app = Flask(__name__)

SPOTIFY_CLIENT_ID = os.environ['SPOTIFY_CLIENT_ID']
SPOTIFY_CLIENT_SECRET = os.environ['SPOTIFY_CLIENT_SECRET']
SPOTIFY_AUTH_URL = "https://accounts.spotify.com/authorize"
SPOTIFY_TOKEN_URL = "https://accounts.spotify.com/api/token"

DROPBOX_CLIENT_ID = os.environ['DROPBOX_CLIENT_ID']
DROPBOX_CLIENT_SECRET = os.environ['DROPBOX_CLIENT_SECRET']
DROPBOX_AUTH_URL = "https://www.dropbox.com/oauth2/authorize"
DROPBOX_TOKEN_URL = "https://api.dropboxapi.com/oauth2/token"


@app.route("/health")
def health_check():
    return "Everything alright !"

@app.route("/oauth/dropbox/authorize", methods=["POST"])
def dropbox_authorize():
    data = request.json
    if not data or not data.get("redirect_url"):
        return jsonify({"error": "Missing required field: 'redirect_url'"}), 400

    redirect_url = urllib.parse.quote(data["redirect_url"], safe="")

    authorize_url = (
        f"{DROPBOX_AUTH_URL}?response_type=code&redirect_uri={redirect_url}&client_id={DROPBOX_CLIENT_ID}"
    )

    return jsonify({"authorize_url": authorize_url})

@app.route("/oauth/dropbox/access_token", methods=["POST"])
def dropbox_get_access_token():
    data = request.json
    if not data or not data.get("code") or not data.get("redirect_url"):
        return jsonify({"error": "Missing required field(s): 'code' and/or 'redirect_url'"}), 400

    payload = {
        'grant_type': 'authorization_code',
        'code': data["code"],
        'redirect_uri': data["redirect_url"],
        'client_id': DROPBOX_CLIENT_ID,
        'client_secret': DROPBOX_CLIENT_SECRET,
    }

    response = requests.post(url=DROPBOX_TOKEN_URL, data=payload)
    if response.status_code == 200:
        return jsonify(response.json())
    else:
        return jsonify({"error": "Failed to retrieve access token", "details": response.json()}), response.status_code

@app.route("/oauth/spotify/authorize", methods=["POST"])
def spotify_authorize():
    data = request.json
    if not data or not data.get("scopes") or not data.get("redirect_url"):
        return jsonify({"error": "Missing required fields: 'scopes' and/or 'redirect_url'"}), 400

    scopes = data["scopes"].replace(" ", "+")
    redirect_url = urllib.parse.quote(data["redirect_url"], safe="")

    authorize_url = (
        f"{SPOTIFY_AUTH_URL}?scope={scopes}&response_type=code&redirect_uri={redirect_url}&client_id={SPOTIFY_CLIENT_ID}"
    )

    return jsonify({"authorize_url": authorize_url})

@app.route("/oauth/spotify/access_token", methods=["POST"])
def spotify_get_access_token():
    data = request.json
    if not data or not data.get("code") or not data.get("redirect_url"):
        return jsonify({"error": "Missing required field: 'code'"}), 400

    auth_header = base64.urlsafe_b64encode((SPOTIFY_CLIENT_ID + ':' + SPOTIFY_CLIENT_SECRET).encode())
    headers = {
        'Content-Type': 'application/x-www-form-urlencoded',
        'Authorization': 'Basic %s' % auth_header.decode('ascii')
    }
    payload = {
        'grant_type': 'authorization_code',
        'code': data["code"],
        'redirect_uri': data["redirect_url"],
    }

    response = requests.post(url=SPOTIFY_TOKEN_URL, data=payload, headers=headers)
    if response.status_code == 200:
        return jsonify(response.json())
    else:
        return jsonify({"error": "Failed to retrieve access token", "details": response.json()}), response.status_code


if __name__ == "__main__":
    app.run(host='0.0.0.0', port=80, debug=True)
