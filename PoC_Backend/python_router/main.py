from http.server import HTTPServer, BaseHTTPRequestHandler
from sys import exit

class S(BaseHTTPRequestHandler):
    def _set_response(self):
        self.send_response(200)
        self.send_header("Content-Length", "21")
        self.end_headers()

    def do_GET(self):
        self._set_response()
        self.wfile.write("hello from the server".encode('utf-8'))

    def do_POST(self):
        content_length = int(self.headers['Content-Length'])
        post_data = self.rfile.read(content_length)

        self._set_response()
        self.wfile.write("hello from the server".encode('utf-8'))

def run(server_class=HTTPServer, handler_class=S):
    server_address = ('0.0.0.0', 80)
    httpd = server_class(server_address, handler_class)
    httpd.serve_forever()
    httpd.server_close()

def main() -> int:
    try:
        run()
    except KeyboardInterrupt:
        return (130)
    return (0)

if (__name__ == "__main__"):
    exit(main())
