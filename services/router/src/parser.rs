use std::{
    io::{prelude::*, BufReader},
    net::TcpStream,
};

extern crate itertools;

use self::itertools::join;

use super::request::*;
use super::response::*;

pub fn collect_request(mut stream: &TcpStream) -> Request {
    let mut buf_reader = BufReader::new(&mut stream);
    let mut req = Request::new();
    let mut index = 0;

    for item in buf_reader.by_ref().lines().into_iter().map(|line| line.unwrap()) {
        if item.is_empty() {
            break;
        }

        if index == 0 {
            let splitted = item.split(' ').collect::<Vec<&str>>();
            let http_and_version = splitted[2].split('/').collect::<Vec<&str>>();
            let version = http_and_version[1].split('.').collect::<Vec<&str>>();

            req.protocol = http_and_version[0].to_string();
            req.http_version = [version[0].parse::<u8>().unwrap(), version[1].parse::<u8>().unwrap()].to_vec();
            req.method = splitted[0].to_string();
            req.route = splitted[1].to_string();
        } else {
            let splitted = item.split(':').collect::<Vec<&str>>();

            req.headers.insert(splitted[0].trim().to_lowercase(), splitted[1].trim().to_string());
        }
        index += 1;
    }

    match req.headers.get("content-length") {
        Some(&ref content_length) => {
            req.body = vec![0u8; content_length.trim().parse::<usize>().unwrap()];
            buf_reader.by_ref().read_exact(&mut req.body);
        },
        _ => {},
    }

    return req;
}

pub fn collect_response(mut stream: &TcpStream) -> Response {
    let mut buf_reader = BufReader::new(&mut stream);
    let mut res = Response::new();
    let mut index = 0;

    for item in buf_reader.by_ref().lines().into_iter().map(|line| line.unwrap()) {
        if item.is_empty() {
            break;
        }

        if index == 0 {
            let splitted = item.split(' ').collect::<Vec<&str>>();
            let http_and_version = splitted[0].split('/').collect::<Vec<&str>>();
            let version = http_and_version[1].split('.').collect::<Vec<&str>>();

            res.protocol = http_and_version[0].to_string();
            res.http_version = [version[0].parse::<u8>().unwrap(), version[1].parse::<u8>().unwrap()].to_vec();
            res.code = splitted[1].parse::<u16>().unwrap();
            res.message = join(splitted.into_iter().enumerate().filter(|&(i, _)| i >= 2 ).map(|(_, e)| e).collect::<Vec<&str>>(), " ");
        } else {
            let splitted = item.split(':').collect::<Vec<&str>>();

            res.headers.insert(splitted[0].trim().to_lowercase(), splitted[1].trim().to_string());
        }
        index += 1;
    }

    match res.headers.get("content-length") {
        Some(&ref content_length) => {
            let mut body = vec![0u8; content_length.trim().parse::<usize>().unwrap()];
            buf_reader.by_ref().read_exact(&mut body);

            res.set_body_as_vec(&body);
        },
        _ => {},
    }

    return res;
}
