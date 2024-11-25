defmodule ElixirRouter do
  def start(port \\ 6970) do
    {:ok, socket} = :gen_tcp.listen(port, [:binary, packet: :line, active: false, reuseaddr: true])
    IO.puts("Server started on port #{port}")
    accept_connections(socket)
  end

  defp accept_connections(socket) do
    {:ok, client} = :gen_tcp.accept(socket)
    spawn(fn -> handle_request(client) end)
    accept_connections(socket)
  end

  defp handle_request(client) do
    case :gen_tcp.recv(client, 0) do
      {:ok, request} ->
        IO.puts("Received request:\n#{request}")
        send_response(client, 200, "OK", "hello from the server")
      {:error, _reason} ->
        IO.puts("Connection error")
    end

    :gen_tcp.close(client)
  end

  defp send_response(client, status_code, status_text, body) do
    response = [
      "HTTP/1.1 #{status_code} #{status_text}\r\n",
      "Content-Length: #{byte_size(body)}\r\n",
      "Content-Type: text/plain\r\n",
      "Connection: close\r\n\r\n",
      body
    ]

    :gen_tcp.send(client, IO.iodata_to_binary(response))
  end
end

ElixirRouter.start()
