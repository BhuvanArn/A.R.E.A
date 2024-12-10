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
use routes::RouteLoader;

fn handle_connection(mut stream: TcpStream) {
    static CELL: OnceLock<Mutex<RouteLoader>> = OnceLock::new();
    let mut req = parser::collect_request(&stream);
    let mut response = Response::new();

    if req.method == "OPTIONS" {
        response.code = 204;
        response.message = String::from("No Content");
        response.headers.insert("Access-Control-Allow-Origin".to_string(), "*".to_string());
        response.headers.insert("Access-Control-Allow-Methods".to_string(), "GET, POST, PUT, DELETE, OPTIONS".to_string());
        
        if let Some(requested_headers) = req.headers.get("access-control-request-headers") {
            response.headers.insert(
                "Access-Control-Allow-Headers".to_string(),
                requested_headers.clone(),
            );
        } else {
            response.headers.insert(
                "Access-Control-Allow-Headers".to_string(),
                "*".to_string(),
            );
        }
    
        response.headers.insert("Access-Control-Allow-Credentials".to_string(), "true".to_string());
        stream.write_all(&response.build_request()).unwrap();
        return;
    }    

    let router = CELL.get_or_init(|| Mutex::new(RouteLoader::new("routes.yml")));

    match router.lock().unwrap().get_route(&req.route) {
        Some(&ref route) => {
            let ip_lookup = format!("{}:{}", route.host, route.port)
                .to_socket_addrs()
                .expect("failed to connect to service")
                .next()
                .unwrap();

            match TcpStream::connect_timeout(&ip_lookup, Duration::from_secs(10)) {
                Ok(mut socket) => {
                    socket.write_all(&req.build_request()).unwrap();

                    let mut res = parser::collect_response(&socket);

                    stream.write_all(&res.build_request()).unwrap();
                    return;
                }
                Err(e) => {
                    eprintln!("Failed to connect to service at {}: {}: {}", route.host, route.port, e);
                    response.set_body("Service Unavailable");
                    response.code = 503;
                    response.message = String::from("SERVICE UNAVAILABLE");

                    stream.write_all(&response.build_request()).unwrap();
                }
            }
        }
        None => {
            if req.route == "/health" {
                response.set_body("OK");
                response.code = 200;
                response.message = String::from("OK");

                stream.write_all(&response.build_request()).unwrap();
                return;
            }
            response.set_body("Not Found");
            response.code = 404;
            response.message = String::from("NOT FOUND");

            stream.write_all(&response.build_request()).unwrap();
        }
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
