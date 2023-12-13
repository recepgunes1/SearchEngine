FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/SearchHub/SearchHub.WebAPI/SearchHub.WebAPI.csproj", "services/SearchHub/SearchHub.WebAPI/"]
COPY ["services/SearchHub/SearchHub.Infrastructure/SearchHub.Infrastructure.csproj", "services/SearchHub/SearchHub.Infrastructure/"]
COPY ["services/Shared/Shared/Shared.csproj", "services/Shared/Shared/"]
RUN dotnet restore "services/SearchHub/SearchHub.WebAPI/SearchHub.WebAPI.csproj"
COPY . .
WORKDIR "/src/services/SearchHub/SearchHub.WebAPI"
RUN dotnet build "SearchHub.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SearchHub.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SearchHub.WebAPI.dll"]
