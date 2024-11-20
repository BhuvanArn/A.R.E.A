from .worker import BenchmarkWorker, WorkerDescriptor

class BenchmarkResult(object):
    def __init__(self, name: str, timed_out: bool = False, elapsed_time: float = 0.0, average_dt: float = 0.0):
        self.name: str = name
        self.timed_out: bool = timed_out
        self.elapsed_time: float = elapsed_time
        self.average_dt: float = average_dt

    def __str__(self):
        return (f"<Result#{self.name} elapsed_time={self.elapsed_time} status={'timeout' if self.timed_out else 'finish'} average_dt={self.average_dt}>")

    def __repr__(self):
        return (self.__str__())

class BenchMarker(object):
    def __init__(self, *args):
        self._descriptor_list = args
        self._result_list = []

    def _store_result(self, worker: BenchmarkWorker, callback):
        self._result_list.append(BenchmarkResult(worker.name, worker.timeout >= 0 and worker.elapsed_time > worker.timeout, worker.elapsed_time, worker.average_dt))

        callback(worker)

    def execute(self, iterations: int, timeout: float = -1, threads: int = 5, on_update = None) -> None:
        workers = []

        if (threads < 1):
            raise ValueError("threads should be greater than 0.")

        for item in self._descriptor_list:
            workers.append(BenchmarkWorker(item.name, iterations, timeout, item.callback, lambda x, on_finish = item.on_finish: self._store_result(x, on_finish)))
            workers[-1].start()

            if (on_update is not None):
                on_update(workers.copy(), self)

            while (len(workers) >= threads):
                if (any(map(lambda x: not x.alive, workers))):
                    workers = [item for item in workers if item.alive]

        while (not all(map(lambda x: not x.alive, workers))):
            continue

        workers.clear()

    def get_results(self) -> list:
        return (self._result_list.copy())