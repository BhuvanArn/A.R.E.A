{
  description = '':
    AREA Web Client Flake
  '';

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable";
  };

  outputs = { self , nixpkgs ,... }:
    let
      system = "x86_64-linux";
    in {
      devShells."${system}".default = let
        pkgs = import nixpkgs {
          inherit system;
        };
      in pkgs.mkShell {
        packages = with pkgs; [
          bashInteractiveFHS
          pkg-config

          nodejs
          vue-language-server
        ];

        shellHook = ''
          echo Custom node.js nix-shell !
        '';
      };
    };
}
