# Use an official GCC image as a parent image
FROM gcc:latest

# Set the working directory
WORKDIR /usr/src/app

# Copy the current directory contents into the container at /usr/src/app
COPY . .

# Install dependencies and build the project
RUN apt-get update && \
    apt-get install -y cmake libboost-system-dev && \
    mkdir -p build && \
    cd build && \
    cmake .. && \
    make

# Expose port 80 to the outside world
EXPOSE 80

# Run the server
CMD ["./build/CppServer"]