FROM debian:bullseye-slim AS builder
WORKDIR /usr/src/router
RUN apt-get update && apt-get install -y \
    curl \
    gcc \
    libpq-dev \
    make \
    pkg-config \
    && curl --proto '=https' --tlsv1.2 -sSf https://sh.rustup.rs | sh -s -- -y
ENV PATH="/root/.cargo/bin:${PATH}"
COPY . .
RUN cargo build --release

FROM debian:bullseye-slim
RUN apt-get update && apt-get install -y \
    libpq-dev \
    gcc \
    pkg-config \
    && rm -rf /var/lib/apt/lists/*
COPY --from=builder /usr/src/router/target/release/router /usr/local/bin/router

EXPOSE 8000

CMD ["router"]
