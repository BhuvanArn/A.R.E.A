{
  description = '':
    PocFrontend Flake
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
          nodePackages.typescript
          nodePackages.typescript-language-server
          nodePackages."@angular/cli"
        ];

        shellHook = ''
          echo Custom angular nix-shell !
        '';
      };
    };
}
