extern crate yaml_rust;
extern crate regex;

use std::{io::Read, fs::File};
use self::regex::Regex;
use self::yaml_rust::YamlLoader;

pub struct Route {
    pub regex: Regex,
    pub host: String,
    pub port: u16,
    pub name: String,
    pub method: String,
    pub path: String,
}

impl Route {
    pub fn new() -> Route {
        return Route {
            regex: Regex::new("^*$").unwrap(),
            host: String::from("127.0.0.1"),
            port: 80,
            name: String::from("default"),
            method: String::from("GET"),
            path: String::from("/"),
        }
    }
}

pub struct RouteLoader {
    pub file_path: String,

    routes: Vec<Route>,
}

impl RouteLoader {
    pub fn new(path: &str) -> RouteLoader {
        let mut route_loader = RouteLoader {
            file_path: String::from(path),

            routes: Vec::new(),
        };
        let mut fp = File::open(String::from(path)).expect("Unable to open file");
        let mut contents = String::new();

        fp.read_to_string(&mut contents).expect("Unable to read file");

        let docs = YamlLoader::load_from_str(&contents).unwrap();
        let doc = &docs[0];
        let routes = &doc["routes"];

        if routes.is_null() || routes.is_badvalue() || !routes.is_array() {
            return route_loader;
        }

        for route in routes.as_vec().unwrap() {
            let mut created_route = Route::new();

            created_route.regex = Regex::new(route["condition"].as_str().expect("cannot parse regex")).unwrap();
            created_route.host = route["host"].as_str().expect("cannot parse host").to_string();
            created_route.port = route["port"].as_i64().expect("cannot parse port") as u16;
            created_route.name = route["name"].as_str().expect("cannot parse name").to_string();
            created_route.method = route["method"].as_str().unwrap_or("GET").to_string();
            created_route.path = route["path"].as_str().unwrap_or("/").to_string();

            route_loader.routes.push(created_route);
        }

        return route_loader;
    }

    pub fn get_route(&self, route_str: &str) -> Option<&Route> {
        for route in &self.routes {
            if !route.regex.is_match(route_str) {
                continue;
            };
            return Some(&route);
        }
        return None
    }

    pub fn get_all_routes(&self) -> &[Route] {
        &self.routes
    }
}