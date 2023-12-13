FROM mcr.microsoft.com/dotnet/runtime:8.0-jammy-chiseled AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/UrlExtractor/UrlExtractor.Worker/UrlExtractor.Worker.csproj", "services/UrlExtractor/UrlExtractor.Worker/"]
COPY ["services/UrlExtractor/UrlExtractor.Infrastructure/UrlExtractor.Infrastructure.csproj", "services/UrlExtractor/UrlExtractor.Infrastructure/"]
COPY ["services/Shared/Shared/Shared.csproj", "services/Shared/Shared/"]
RUN dotnet restore "services/UrlExtractor/UrlExtractor.Worker/UrlExtractor.Worker.csproj"
COPY . .
WORKDIR "/src/services/UrlExtractor/UrlExtractor.Worker"
RUN dotnet build "UrlExtractor.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UrlExtractor.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UrlExtractor.Worker.dll"]
