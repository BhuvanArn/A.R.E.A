from sys import exit, argv, stderr, stdout
from yaml import safe_load
from hashlib import sha1
from threading import Thread
from time import sleep
from src import *

class ServiceConnector(object):
    pass

class UIDisplay(object):
    def __init__(self):
        self._watching = []
        self._old_lines = []
        self._lines = []

        self._state_end = None
        self.running = False

    def _watcher(self):
        for i, item in enumerate(self._watching):
            if (i >= len(self._lines)):
                continue

            if (item.alive):
                self._lines[i] = f"Benching {item.name.ljust(40)} ({str(item.iteration_count).ljust(len(str(item.iterations)))}) {round(item.elapsed_time, 1):<4}"
            else:
                self._lines[i] = f"{item.name} [{"TIMEOUT" if (item.timeout >= 0 and item.elapsed_time > item.timeout) else "FINISH"}]"

    def _writer(self):
        if (not self._lines):
            return

        stdout.write(
            (f"\x1b[{len(self._old_lines)}A" if (self._old_lines) else '') +
            (''.join((len(item) * ' ') + '\n' for item in self._old_lines)) +
            (f"\x1b[{len(self._old_lines)}A" if (self._old_lines) else '') +
            (''.join(item + '\n' for item in self._lines))
        )

        stdout.flush()

        self._old_lines = self._lines.copy()

    def updater(self, workers, _):
        for item in workers:
            if (item not in self._watching):
                self._watching.append(item)
                self._lines.append('')

    def background_start(self):
        T = Thread(target=lambda: self.start() and T.join(5))

        T.start()

    def are_all_watching_dead(self):
        self.ready = False

        def watch_state():
            self.ready = True

        self._state_end = watch_state

        while (not self.ready):
            continue

        return (all(map(lambda x: not x.alive, self._watching)))

    def start(self):
        self.running = True

        while (self.running):
            self._watcher()
            self._writer()

            if (self._state_end is not None):
                self._state_end()

            sleep(0.2)

def load_config(file_path: str) -> None:
    with open(file_path, 'r') as fp:
        return safe_load(fp)

def main() -> int:
    if (len(argv) < 2):
        stderr.write(f"{argv[0]}: not enough arguments.\n")
        return (84)

    if (len(argv) > 2):
        stderr.write(f"{argv[0]}: too much arguments.\n")
        return (84)

    config: dict = load_config(argv[1])

    if (not config):
        stdout.write(f"{argv[0]}: the configuration is empty.\n")
        return (0)

    if ("benchmarks" not in config):
        stdout.write(f"{argv[0]}: 'benchmarks' not found in config.\n")
        return (0)

    benchmarks_results = {}

    for item in config["benchmarks"]:
        if (not item):
            stderr.write("Empty benchmark, skipping.\n")
            continue

        name = item["name"] if "name" in item else ''
        hashed = sha1(str(item).encode(), usedforsecurity = False).hexdigest()
        iterations = item["iterations"] if item["iterations"] else 10000
        timeout = item["timeout"] if item["timeout"] else -1
        threads = item["threads"] if item["threads"] else 5

        if ("workers" not in item or not item["workers"]):
            stderr.write(f"No worker, skipping benchmark {name if name else hashed}{('_<' + hashed + '>') if name else ''}.\n")
            continue

        if ("iterations" not in item):
            stderr.write("No iterations, defaulting 10000.\n")
            continue

        stdout.write(f"Running benchmark {name if name else hashed}{('_<' + hashed + '>') if name else ''}.\n")

        workers = []

        for worker in item["workers"]:
            workers.append(WorkerDescriptor(worker["name"] if worker["name"] else sha1(worker["code"], usedforsecurity=False).hexdigest(), lambda: exec(f"\ndef test():\n    {worker["code"].replace('\n', '\n    ')}\n")))

        display: UIDisplay = UIDisplay()
        bm: BenchMarker = BenchMarker(*workers)

        display.background_start()
        bm.execute(iterations, timeout, threads, display.updater)

        while (not display.are_all_watching_dead()):
            continue

        display.running = False

        benchmarks_results[name] = {"iterations": iterations, "datas": bm.get_results()}

    stdout.write("\nResults:\n")
    for item in benchmarks_results:
        stdout.write(f"  * {item} ({benchmarks_results[item]["iterations"]}):\n")
        for item in benchmarks_results[item]["datas"]:
            stdout.write(f"    . {item.name.ljust(40)}\n    |- Elapsed: {item.elapsed_time}\n    |- Avg. dt: {item.average_dt}\n    \\ Status: {"FINISH" if not item.timed_out else "TIMEOUT"}\n")
            stdout.write('\n')

        stdout.write('\n')

    # bm: BenchMarker = BenchMarker(
    #     WorkerDescriptor("operation0", operation0),
    #     WorkerDescriptor("operation1", operation1)
    # )

    # bm.execute(10000, 50)

    # print(bm.get_results())

    return (0)

if (__name__ == "__main__"):
    exit(main())
