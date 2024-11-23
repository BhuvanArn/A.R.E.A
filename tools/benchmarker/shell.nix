# Shell to run main.py benchmarker

with import <nixpkgs> {};
(python311.withPackages (ps: with ps; [
  ps.pyyaml
  ps.requests
])).env
