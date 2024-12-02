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

    pub fn build_request(&mut self) -> Vec<u8> {
        let mut request_base = format!("{} {} {}/{}.{}\r\n", self.method, self.route, self.protocol, self.http_version[0], self.http_version[1]);
        for (key, value) in &self.headers {
            request_base.push_str(&mut format!("{}: {}\r\n", key, value));
        }
        request_base.push_str("\r\n");

        let mut bytes = request_base.as_bytes().to_vec();
        bytes.append(&mut self.body);

        return bytes;
    }
}
