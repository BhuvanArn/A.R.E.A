use std::{
    io::prelude::*,
    net::{TcpListener, TcpStream},
    thread,
};

mod request;
mod parser;

fn handle_connection(mut stream: TcpStream) {
    let req = parser::collect_request(&stream);
    let response = "HTTP/1.1 200 OK\r\nContent-Length: 21\r\n\r\nhello from the server\r\n";

    stream.write_all(response.as_bytes()).unwrap();
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
