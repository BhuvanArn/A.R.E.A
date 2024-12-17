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
COPY ../../EventBusTests/EventBusTests.csproj ./EventBusTests/
COPY ../../ActionReactionService/ActionReactionService.csproj ./ActionReactionService/

RUN dotnet restore

COPY ../../ ./

RUN dotnet build -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y curl

COPY --from=build /app/build .

EXPOSE 80

ENTRYPOINT ["dotnet", "AdvancedServices.dll"]
