use std::{
    io::prelude::*,
    net::{TcpListener, TcpStream},
    thread,
};

mod request;
mod response;
mod parser;

use response::Response;

fn handle_connection(mut stream: TcpStream) {
    let _req = parser::collect_request(&stream);
    let mut response = Response::new();

    response.set_body("hello from the server");

    stream.write_all(&response.build_request()).unwrap();
}

fn main()
{
    let listener = TcpListener::bind("0.0.0.0:80").unwrap();

    for stream in listener.incoming() {
        let stream = stream.unwrap();

        thread::spawn(move || {
            handle_connection(stream);
        });
    }
    println!("Connection end");
    return ();
}
