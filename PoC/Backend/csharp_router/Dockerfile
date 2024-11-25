FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /usr/src/app

COPY router.sln ./
COPY router ./router

RUN dotnet restore

RUN dotnet publish router.sln -c Release -o /usr/src/app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /usr/src/app

COPY --from=build /usr/src/app/out .

CMD ["dotnet", "router.dll"]
