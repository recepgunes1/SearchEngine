FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/Gateway/Gateway.WebAPI/Gateway.WebAPI.csproj", "services/Gateway/Gateway.WebAPI/"]
RUN dotnet restore "services/Gateway/Gateway.WebAPI/Gateway.WebAPI.csproj"
COPY . .
WORKDIR "/src/services/Gateway/Gateway.WebAPI"
RUN dotnet build "Gateway.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Gateway.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.WebAPI.dll"]
