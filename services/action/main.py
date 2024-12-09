from sys import exit
from select import select
from protocol import *

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

            if (connection.connected):
                read_ready_sockets, _, _ = select([connection.connected], [], [], 0)

                if (read_ready_sockets):
                    message = connection.get_message()

                    if (message.type == INVM):
                        connection.close_client()
                        print(f"[PYTHON (service-action)] - reaction disconnected", flush=True)
        except KeyboardInterrupt:
            break
        except Exception as e:
            print(f"[PYTHON (service-action)] - {e}", flush=True)
            return (84)

    return (0)

if (__name__ == "__main__"):
    exit(main())
