FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/SyncElastic/SyncElastic.Worker/SyncElastic.Worker.csproj", "services/SyncElastic/SyncElastic.Worker/"]
COPY ["services/SyncElastic/SyncElastic.Infrastructure/SyncElastic.Infrastructure.csproj", "services/SyncElastic/SyncElastic.Infrastructure/"]
COPY ["services/Shared/Shared/Shared.csproj", "services/Shared/Shared/"]
RUN dotnet restore "services/SyncElastic/SyncElastic.Worker/SyncElastic.Worker.csproj"
COPY . .
WORKDIR "/src/services/SyncElastic/SyncElastic.Worker"
RUN dotnet build "SyncElastic.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SyncElastic.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SyncElastic.Worker.dll"]
