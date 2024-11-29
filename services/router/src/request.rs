use std::collections::HashMap;

pub struct Request {
    pub protocol: String,
    pub http_version: Vec<u8>,
    pub method: String,
    pub route: String,

    pub headers: HashMap<String, String>,

    pub body: Vec<u8>
}

impl Request {
    pub fn new() -> Request {
        return Request {
            protocol: String::from("HTTP"),
            http_version: [1, 1].to_vec(),
            method: String::from("GET"),
            route: String::from("/"),

            headers: HashMap::new(),

            body: Vec::new(),
        };
    }
}
