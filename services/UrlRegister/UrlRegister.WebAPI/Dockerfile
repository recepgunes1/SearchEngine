FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/UrlRegister/UrlRegister.WebAPI/UrlRegister.WebAPI.csproj", "services/UrlRegister/UrlRegister.WebAPI/"]
COPY ["services/UrlRegister/UrlRegister.Infrastructure/UrlRegister.Infrastructure.csproj", "services/UrlRegister/UrlRegister.Infrastructure/"]
COPY ["services/Shared/Shared/Shared.csproj", "services/Shared/Shared/"]
RUN dotnet restore "services/UrlRegister/UrlRegister.WebAPI/UrlRegister.WebAPI.csproj"
COPY . .
WORKDIR "/src/services/UrlRegister/UrlRegister.WebAPI"
RUN dotnet build "UrlRegister.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UrlRegister.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UrlRegister.WebAPI.dll"]
