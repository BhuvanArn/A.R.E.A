use actix_web::{post, delete, get, web, App, HttpResponse, HttpServer, Responder};
use actix_cors::Cors;
use rand::Rng;
use serde::{Deserialize, Serialize};
use tokio_postgres::{Client, NoTls};
use std::sync::Arc;


#[derive(Deserialize)]
struct RequestData {
    msg: String,
}

#[derive(Serialize)]
struct ResponseData {
    id: i32,
    name: String,
    message: String,
}

#[post("/")]
async fn handle_post(data: web::Json<RequestData>, db_client: web::Data<Arc<Client>>) -> impl Responder {
    let msg = &data.msg;
    let name = generate_random_name();

    let row = db_client
        .query_one(
            "INSERT INTO users (name, message) VALUES ($1, $2) RETURNING id, name, message",
            &[&name, &msg],
        )
        .await
        .expect("Failed to insert data");

    let id: i32 = row.get(0);
    let name: String = row.get(1);
    let message: String = row.get(2);

    HttpResponse::Ok().json(ResponseData { id, name, message })
}

#[get("/users")]
async fn get_users(db_client: web::Data<Arc<Client>>) -> impl Responder {
    let rows = db_client
        .query("SELECT * FROM users", &[])
        .await
        .expect("Failed to fetch data");

    let users: Vec<ResponseData> = rows
        .iter()
        .map(|row| ResponseData {
            id: row.get(0),
            name: row.get(1),
            message: row.get(2),
        })
        .collect();

    HttpResponse::Ok().json(users)
}

#[delete("/users/{id}")]
async fn delete_user(db_client: web::Data<Arc<Client>>, id: web::Path<i32>) -> impl Responder {
    let id = id.into_inner();
    db_client
        .execute("DELETE FROM users WHERE id = $1", &[&id])
        .await
        .expect("Failed to delete data");

    println!("Deleted user with id: {}", id);

    HttpResponse::Ok().finish()
}

fn generate_random_name() -> String {
    let mut rng = rand::thread_rng();
    let length = rng.gen_range(4..=8);
    (0..length)
        .map(|_| (rng.gen_range(b'a'..=b'z') as char))
        .collect()
}

#[actix_web::main]
async fn main() -> std::io::Result<()> {
    let (client, connection) = tokio_postgres::connect(
        "host=db port=5432 user=postgres password=password dbname=postgres",
        NoTls,
    )
    .await
    .expect("Failed to connect to database");

    // Spawn the connection in a separate task
    tokio::spawn(async move {
        if let Err(e) = connection.await {
            eprintln!("Connection error: {}", e);
        }
    });

    let client = Arc::new(client);

    HttpServer::new(move || {
        let cors = Cors::default()
            .allow_any_origin()
            .allow_any_method()
            .allow_any_header()
            .max_age(3600);
        App::new()
            .wrap(cors)
            .app_data(web::Data::new(client.clone()))
            .service(handle_post)
            .service(get_users)
            .service(delete_user)
    })
    .bind("0.0.0.0:8000")?
    .run()
    .await;

    Ok(())
}
