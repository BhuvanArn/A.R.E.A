use std::collections::HashMap;

pub struct Response {
    pub protocol: String,
    pub http_version: Vec<u8>,
    pub code: u16,
    pub message: String,

    pub headers: HashMap<String, String>,

    body: Vec<u8>
}

impl Response {
    pub fn new() -> Response {
        return Response {
            protocol: String::from("HTTP"),
            http_version: [1, 1].to_vec(),
            code: 200,
            message: String::from("OK"),

            headers: HashMap::new(),

            body: Vec::new(),
        };
    }

    pub fn set_body(&mut self, text: &str) {
        self.body = text.as_bytes().to_vec();

        match self.headers.get("content-length") {
            Some(_) => {
                self.headers.remove("content-length");
            },
            _ => {},
        }

        self.headers.insert("content-length".to_owned(), self.body.len().to_string());
    }

    pub fn set_body_as_vec(&mut self, text: &Vec<u8>) {
        self.body = text.to_vec();

        match self.headers.get("content-length") {
            Some(_) => {
                self.headers.remove("content-length");
            },
            _ => {},
        }

        self.headers.insert("content-length".to_owned(), self.body.len().to_string());
    }

    pub fn get_body(&mut self) -> &Vec<u8> {
        return &self.body;
    }

    pub fn build_request(&mut self) -> Vec<u8> {
        let mut request_base = format!("{}/{}.{} {} {}\r\n", self.protocol, self.http_version[0], self.http_version[1], self.code, self.message);
        for (key, value) in &self.headers {
            request_base.push_str(&mut format!("{}: {}\r\n", key, value));
        }
        request_base.push_str("\r\n");

        let mut bytes = request_base.as_bytes().to_vec();
        bytes.append(&mut self.body);

        return bytes;
    }
}
