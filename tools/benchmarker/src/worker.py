from time import time
from threading import Thread

class BenchmarkWorker(object):
    def __init__(self, name: str, iterations: int, timeout: float, callback, on_finish):
        self.name: str = name

        self.alive: bool = False
        self.joined: bool = True

        self.iterations: int = iterations
        self.timeout: float = timeout

        self.dt: float = 0.0
        self.elapsed_time: float = 0.0
        self.average_dt: float = 0.0
        self.iteration_count: int = 0

        self._average_dt_list = []

        self._start_clock: float = None
        self._dt_clock: float = None

        self.callback = callback
        self.on_finish = on_finish

        self._thread = Thread(target=self._work_script)

    def _work_script(self) -> None:
        self.joined: bool = False
        self.alive: bool = True

        self.dt: float = 0.0
        self.average_dt: float = 0.0
        self.elapsed_time: float = 0.0

        self._average_dt_list.clear()

        self.iteration_count: int = 0

        self._start_clock: float = time()
        self._dt_clock: float = time()

        callback = self.callback

        for i in range(self.iterations):
            callback()

            iteration_end: float = time()

            self.dt: float = iteration_end - self._dt_clock
            self.elapsed_time: float = iteration_end - self._start_clock
            self.iteration_count += 1
            self._dt_clock: float = iteration_end

            if (self.iterations - i < 30):
                self._average_dt_list.append(self.dt)

            if ((self.timeout >= 0 and self.elapsed_time > self.timeout) or not self.alive):
                break

        self.average_dt = (sum(self._average_dt_list) / len(self._average_dt_list))

        self.on_finish(self)

        try:
            self._thread.join(5)
            self.joined = True
        except Exception:
            pass

        self.alive = False

    def start(self) -> None:
        self._thread.start()

    def stop(self) -> None:
        self.alive = False

    def destroy(self):
        while (self.alive):
            self.alive = False

        if (not self.joined):
            self._thread.join(10)

    def __del__(self):
        self.destroy()

class WorkerDescriptor(object):
    def __init__(self, name: str, callback = None, on_finish = None):
        self.name: str = name
        self.callback = callback if callback is not None else WorkerDescriptor._default_callback
        self.on_finish = on_finish if on_finish is not None else WorkerDescriptor._default_callback

    @staticmethod
    def _default_callback(*args, **kwargs):
        pass
