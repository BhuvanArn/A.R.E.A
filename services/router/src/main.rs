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
    static CELL: OnceLock<Mutex<RouteLoader>> =  OnceLock::new();
    let mut req = parser::collect_request(&stream);
    let mut response = Response::new();

    let router = CELL.get_or_init(|| Mutex::new(RouteLoader::new("routes.yml")));

    match router.lock().unwrap().get_route(&req.route) {
        Some(&ref route) => {
            let ip_lookup = format!("{}:{}", route.host, route.port).to_socket_addrs().expect("failed to connect to service").next().unwrap();

            let mut socket = TcpStream::connect_timeout(&ip_lookup, Duration::from_millis(1024 * 10)).unwrap();

            socket.write_all(&req.build_request());

            let mut response = Vec::<u8>::new();
            socket.read(&mut response);

            stream.write_all(&response).unwrap();
        },
        _ => {
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

    for stream in listener.incoming() {
        let stream = stream.unwrap();

        thread::spawn(
            || handle_connection(stream)
        ).join().unwrap();
    }
    return ();
}
