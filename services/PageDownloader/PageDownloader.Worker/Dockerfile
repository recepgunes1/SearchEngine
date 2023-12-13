FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/PageDownloader/PageDownloader.Worker/PageDownloader.Worker.csproj", "services/PageDownloader/PageDownloader.Worker/"]
COPY ["services/PageDownloader/PageDownloader.Infrastructure/PageDownloader.Infrastructure.csproj", "services/PageDownloader/PageDownloader.Infrastructure/"]
COPY ["services/Shared/Shared/Shared.csproj", "services/Shared/Shared/"]
RUN dotnet restore "services/PageDownloader/PageDownloader.Worker/PageDownloader.Worker.csproj"
COPY . .
WORKDIR "/src/services/PageDownloader/PageDownloader.Worker"
RUN dotnet build "PageDownloader.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PageDownloader.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PageDownloader.Worker.dll"]
