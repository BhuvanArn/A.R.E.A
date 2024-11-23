/*
** EPITECH PROJECT, 2024
** A.R.E.A
** File description:
** c++ server test, only responding 'hello from the server' atm
*/

#include <boost/asio.hpp>
#include <iostream>

using boost::asio::ip::tcp;

void handle_request(tcp::socket& socket) {
    boost::system::error_code error;
    boost::asio::streambuf request;
    boost::asio::read_until(socket, request, "\r\n", error);

    if (!error) {
        std::string response =
            "HTTP/1.1 200 OK\r\n"
            "Content-Length: 21\r\n"
            "Content-Type: text/plain\r\n"
            "\r\n"
            "hello from the server";

        boost::asio::write(socket, boost::asio::buffer(response), error);
    }
}

int main(void) {
    try {
        boost::asio::io_context io_context;
        tcp::acceptor acceptor(io_context, tcp::endpoint(tcp::v4(), 80));

        while (1) {
            tcp::socket socket(io_context);
            acceptor.accept(socket);
            handle_request(socket);
        }
    } catch (const std::exception& e) {
        std::cerr << "Exception: " << e.what() << "\n";
    }

    return 0;
}
