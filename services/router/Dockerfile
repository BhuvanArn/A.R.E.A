FROM rust:1.70 AS builder
WORKDIR /usr/src/router
COPY . .
RUN cargo install --path .

FROM debian:bullseye-slim
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
COPY --from=builder /usr/local/cargo/bin/router /usr/local/bin/router
COPY routes.yml .

EXPOSE 80

CMD ["router"]
