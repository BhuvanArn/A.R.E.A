use std::{
    io::prelude::*,
    net::{TcpListener, TcpStream, ToSocketAddrs},
    thread,
    sync::{OnceLock, Mutex},
    time::Duration,
};

mod request;
mod response;
mod parser;
mod routes;

use response::Response;
use routes::{RouteLoader, Route};

fn handle_connection(mut stream: TcpStream) {
    static CELL: OnceLock<Mutex<RouteLoader>> =  OnceLock::new();
    let mut req = parser::collect_request(&stream);
    let mut response = Response::new();

    req.headers.insert("x-router-forwarded".to_string(), stream.peer_addr().unwrap().ip().to_string());

    if req.method == "OPTIONS" {
        response.headers.insert("Access-Control-Allow-Origin".to_string(), "*".to_string());
        response.headers.insert("Access-Control-Allow-Headers".to_string(), "Content-Type, Authorization, X-User-Token".to_string());
        response.headers.insert("Access-Control-Allow-Methods".to_string(), "GET, POST, PUT, DELETE, OPTIONS".to_string());

        response.code = 204;
        response.message = String::from("No Content");
        stream.write_all(&response.build_request()).unwrap();
        return;
    }

    let router = CELL.get_or_init(|| Mutex::new(RouteLoader::new("routes.yml")));

    match router.lock().unwrap().get_route(&req.route) {
        Some(&ref route) => {
            let ip_lookup = format!("{}:{}", route.host, route.port).to_socket_addrs();

            if ip_lookup.is_ok() {
                let ip_lookup_value = ip_lookup.unwrap().next().unwrap();

                match TcpStream::connect_timeout(&ip_lookup_value, Duration::from_millis(1024 * 10)) {
                    Ok(mut socket) => {
                        socket.write_all(&req.build_request()).unwrap();

                        let mut res = parser::collect_response(&socket);

                        res.headers.insert("access-control-allow-origin".to_string(), "*".to_string());

                        stream.write_all(&res.build_request()).unwrap();
                        return ();
                    }
                    Err(e) => {
                        eprintln!("Failed to connect to service at {}: {}: {}", route.host, route.port, e);
                        response.set_body("Service Unavailable");
                        response.code = 503;
                        response.message = String::from("SERVICE UNAVAILABLE");

                        stream.write_all(&response.build_request()).unwrap();
                        return ();
                    }
                }
            } else {
                eprintln!("Failed to connect to service at {}: {}", route.host, route.port);
                response.set_body("Service Unavailable");
                response.code = 503;
                response.message = String::from("SERVICE UNAVAILABLE");

                stream.write_all(&response.build_request()).unwrap();
            }
        },
        _ => {
            if req.route == "/health" {
                response.set_body("OK");
                response.code = 200;
                response.message = String::from("OK");

                stream.write_all(&response.build_request()).unwrap();
                return ();
            }
            response.set_body("Not found");
            response.code = 404;
            response.message = String::from("NOT FOUND");

            stream.write_all(&response.build_request()).unwrap();
        },
    }
}

fn main()
{
    let listener = TcpListener::bind("0.0.0.0:80").unwrap();

    println!("[RUST (service-router)] - listening on: 0.0.0.0:80");

    for stream in listener.incoming() {
        let stream = stream.unwrap();

        thread::spawn(
            || handle_connection(stream)
        ).join().unwrap();
    }
    return ();
}
