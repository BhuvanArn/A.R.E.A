class TextIOLocal(object):
    def __init__(self, encoding = "utf-8", errors = "strict", maxlines = 9999) -> None:
        self.closed: bool = False
        self.buffer: str = ""
        self.encoding = encoding
        self.errors = errors
        self.line_buffering = True
        self.mode = "rw"

        self.maxlines = maxlines

        self.name = "LocalIO"

        self.file = bytearray()

    def detach(self):
        return (None)

    def fileno(self) -> int:
        return (-1)

    def isatty(self) -> bool:
        return (False)

    def readable(self) -> bool:
        return True

    def writable(self) -> bool:
        return True

    def close(self) -> None:
        self.file = bytearray()
        self.buffer = ""
        self.closed = True

    def read(self, size: int = -1, destroy = True) -> str:
        value = bytes(self.file).decode(self.encoding, errors=self.errors)

        if (destroy):
            self.file = bytearray() if size == -1 else bytearray(value[size:].encode())

        return value if size == -1 else value[:size]

    def write(self, s: str) -> int:
        self.buffer += s

        if ('\n' in s):
            self.flush()

    def flush(self) -> None:
        self.file.extend(self.buffer.encode(self.encoding, errors=self.errors))
        self.buffer = ""

        temp = bytes(self.file).split(b'\n')
        while (self.maxlines != -1 and len(temp) > self.maxlines):
            temp = temp[1:]

        self.file = bytearray(b'\n'.join(temp))

    def __del__(self):
        self.close()

class TextIOBytesLocal(object):
    def __init__(self, encoding = "utf-8", errors = "strict", maxlines = 9999) -> None:
        self.closed: bool = False
        self.buffer: bytearray = bytearray()
        self.encoding = encoding
        self.errors = errors
        self.line_buffering = True
        self.mode = "rw"

        self.maxlines = maxlines

        self.name = "LocalIO"

        self.file = bytearray()

    def detach(self):
        return (None)

    def fileno(self) -> int:
        return (-1)

    def isatty(self) -> bool:
        return (False)

    def readable(self) -> bool:
        return True

    def writable(self) -> bool:
        return True

    def close(self) -> None:
        self.file = bytearray()
        self.buffer = bytearray()
        self.closed = True

    def read(self, size: int = -1, destroy: bool = False) -> str:
        value = bytes(self.file)

        if (destroy):
            self.file = bytearray() if size == -1 else bytearray(value[size:])

        return value if size == -1 else value[:size]

    def write(self, s: bytes) -> int:
        self.buffer.extend(s)

        if (b'\n' in s):
            self.flush()

    def flush(self) -> None:
        self.file.extend(self.buffer)
        self.buffer = bytearray()

        temp = bytes(self.file).split(b'\n')
        while (self.maxlines != -1 and len(temp) > self.maxlines):
            temp = temp[1:]

        self.file = bytearray(b'\n'.join(temp))

    def __del__(self):
        self.close()
