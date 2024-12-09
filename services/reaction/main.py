from sys import exit
from select import select
from protocol import *
from time import sleep

def main() -> int:
    print(f"[PYTHON (service-reaction)] - starting...", flush=True)
    connected = False
    retry = 0

    connection = AreaConnect(host="service-action")
    while (not connected) and retry < 3:
        try:
            connection.set_connect()
            if (not connection.connected):
                raise RuntimeError("invalid service")
            connected = True
        except Exception as e:
            retry += 1
            print(f"[PYTHON (service-reaction)] - Failed to connect to service-action:2727 ({e}), retrying...", flush=True)
            sleep(7.4)

    if (not connected):
        print(f"[PYTHON (service-reaction)] - failed to connect to service-action:2727", flush=True)
        return (84)

    print(f"[PYTHON (service-reaction)] - connected to service-action on port 2727", flush=True)

    while 1:
        try:
            read_ready_sockets, _, _ = select([connection.connected], [], [], 0)

            if (read_ready_sockets):
                message = connection.get_message()

                if (message.type == INVM):
                    print(f"[PYTHON (service-reaction)] - action disconnected, closing", flush=True)
                    connection.close_client()
                    return (0)
        except KeyboardInterrupt:
            break
        except Exception as e:
            print(f"[PYTHON (service-reaction)] - {e}", flush=True)
            return (84)

    return (0)

if (__name__ == "__main__"):
    exit(main())
