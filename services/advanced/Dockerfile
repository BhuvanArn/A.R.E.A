FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ../../AdvancedServices.sln ./
COPY ../../AdvancedServices/AdvancedServices.csproj ./AdvancedServices/
COPY ../../Database/Database.csproj ./Database/
COPY ../../EventBus/EventBus.csproj ./EventBus/
COPY ../../LoginService/LoginService.csproj ./LoginService/
COPY ../../RegisterService/RegisterService.csproj ./RegisterService/
COPY ../../Extension/Extension.csproj ./Extension/
COPY ../../DatabaseTests/DatabaseTests.csproj ./DatabaseTests/

RUN dotnet restore

COPY ../../ ./

RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

RUN apt-get update && apt-get install -y curl

COPY --from=publish /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "AdvancedServices.dll"]
