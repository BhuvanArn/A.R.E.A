FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /usr/src/app

COPY *.csproj ./
RUN dotnet restore

COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:7.0

WORKDIR /usr/src/app

COPY --from=build /usr/src/app/out ./

CMD ["dotnet", "Program.dll"]
