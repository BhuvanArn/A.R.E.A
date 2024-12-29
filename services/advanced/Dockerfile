FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ../../AdvancedServices.sln ./
COPY ../../AdvancedServices/AdvancedServices.csproj ./AdvancedServices/
COPY ../../Database/Database.csproj ./Database/
COPY ../../EventBus/EventBus.csproj ./EventBus/
COPY ../../AuthService/AuthService.csproj ./AuthService/
COPY ../../Extension/Extension.csproj ./Extension/
COPY ../../DatabaseTests/DatabaseTests.csproj ./DatabaseTests/
COPY ../../EventBusTests/EventBusTests.csproj ./EventBusTests/
COPY ../../ActionReactionService/ActionReactionService.csproj ./ActionReactionService/

RUN dotnet restore

COPY ../../ ./

RUN dotnet build -c Release -o /app/build

RUN dotnet test ./DatabaseTests/DatabaseTests.csproj --logger "trx;LogFileName=/test-results/test-results.trx" --results-directory /test-results

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

RUN apt-get update && apt-get install -y curl

COPY --from=publish /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "AdvancedServices.dll"]
