from sys import exit
from select import select
from protocol import *

class Action(object):
    pass

class Watcher(object):
    pass

def main() -> int:
    print(f"[PYTHON (service-action)] - starting...", flush=True)

    connection = AreaConnect()
    connection.set_listen()

    print(f"[PYTHON (service-action)] - lisening on {connection.port}", flush=True)

    while 1:
        try:
            read_ready_sockets, _, _ = select([connection.socket], [], [], 0)

            if (read_ready_sockets):
                connection.accept_if_not_connected()
                print(f"[PYTHON (service-action)] - connected to reaction", flush=True)
        except KeyboardInterrupt:
            break
        except Exception as e:
            print(e)

    return (0)

if (__name__ == "__main__"):
    exit(main())
