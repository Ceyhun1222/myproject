FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 65230
EXPOSE 44342

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY OmegaObstacleChecker/OmegaObstacleChecker.csproj OmegaObstacleChecker/
RUN dotnet restore OmegaObstacleChecker/OmegaObstacleChecker.csproj
COPY . .
WORKDIR /src/OmegaObstacleChecker
RUN dotnet build OmegaObstacleChecker.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish OmegaObstacleChecker.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OmegaObstacleChecker.dll"]
