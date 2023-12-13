FROM mcr.microsoft.com/dotnet/runtime:8.0-jammy-chiseled AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/TagExtractor/TagExtractor.Worker/TagExtractor.Worker.csproj", "services/TagExtractor/TagExtractor.Worker/"]
COPY ["services/TagExtractor/TagExtractor.Infrastructure/TagExtractor.Infrastructure.csproj", "services/TagExtractor/TagExtractor.Infrastructure/"]
COPY ["services/Shared/Shared/Shared.csproj", "services/Shared/Shared/"]
RUN dotnet restore "services/TagExtractor/TagExtractor.Worker/TagExtractor.Worker.csproj"
COPY . .
WORKDIR "/src/services/TagExtractor/TagExtractor.Worker"
RUN dotnet build "TagExtractor.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TagExtractor.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TagExtractor.Worker.dll"]
