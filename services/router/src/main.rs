extern crate serde;
extern crate serde_json;

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

use serde::{Deserialize, Serialize};


#[derive(Serialize, Deserialize)]
struct OpenApiSpec {
    openapi: String,
    info: Info,
    paths: serde_json::Value,
}

#[derive(Serialize, Deserialize)]
struct Info {
    title: String,
    version: String,
}

fn generate_openapi_spec(routes: &[Route]) -> OpenApiSpec {
    let mut paths = serde_json::Map::new();

    for route in routes {
        let path_item = serde_json::json!({
            &route.method.to_lowercase(): {
                "summary": route.name,
                "responses": {
                    "200": {
                        "description": "Successful response"
                    }
                }
            }
        });

        paths.insert(route.path.clone(), path_item);
    }

    OpenApiSpec {
        openapi: "3.0.0".to_string(),
        info: Info {
            title: "API Documentation".to_string(),
            version: "1.0.0".to_string(),
        },
        paths: serde_json::Value::Object(paths),
    }
}

fn handle_connection(mut stream: TcpStream) {
    static CELL: OnceLock<Mutex<RouteLoader>> =  OnceLock::new();
    let mut req = parser::collect_request(&stream);
    let mut response = Response::new();

    if req.method == "OPTIONS" {
        response.headers.insert("Access-Control-Allow-Origin".to_string(), "*".to_string());
        response.headers.insert("Access-Control-Allow-Headers".to_string(), "Content-Type, Authorization".to_string());

        response.code = 204;
        response.message = String::from("No Content");
        stream.write_all(&response.build_request()).unwrap();
        return;
    }

    if req.route == "/swagger" {
        let router = CELL.get_or_init(|| Mutex::new(RouteLoader::new("routes.yml")));
        let locked_router = router.lock().unwrap();
        let routes = locked_router.get_all_routes();
        let openapi_spec = generate_openapi_spec(routes);
        let swagger_json = serde_json::to_string_pretty(&openapi_spec).unwrap();

        response.set_body(&swagger_json);
        response.headers.insert("Content-Type".to_string(), "application/json".to_string());
        response.code = 200;
        response.message = String::from("OK");

        eprintln!("Generated OpenAPI spec: {}", swagger_json);

        stream.write_all(&response.build_request()).unwrap();
        return ();
    }

    let router = CELL.get_or_init(|| Mutex::new(RouteLoader::new("routes.yml")));

    match router.lock().unwrap().get_route(&req.route) {
        Some(&ref route) => {
            let ip_lookup = format!("{}:{}", route.host, route.port).to_socket_addrs().expect("failed to connect to service").next().unwrap();

            match TcpStream::connect_timeout(&ip_lookup, Duration::from_millis(1024 * 10)) {
                Ok(mut socket) => {
                    socket.write_all(&req.build_request()).unwrap();

                    let mut res = parser::collect_response(&socket);

                    stream.write_all(&res.build_request()).unwrap();
                    return ();
                }
                Err(e) => {
                    eprintln!("Failed to connect to service at {}: {}: {}", route.host, route.port, e);
                    response.set_body("Service Unavailable");
                    response.code = 503;
                    response.message = String::from("SERVICE UNAVAILABLE");

                    stream.write_all(&response.build_request()).unwrap();
                }
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
