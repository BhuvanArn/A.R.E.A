{ pkgs ? import <nixpkgs> {} }:

pkgs.mkShell {
  buildInputs = [
    pkgs.boost
    pkgs.cmake
    pkgs.gcc
    pkgs.python311
  ];

  shellHook = ''
    mkdir -p build
    cd build
    cmake ..
    make
    echo "Run the server with: sudo ./CppServer"
  '';
}