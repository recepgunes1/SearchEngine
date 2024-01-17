FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["clients/Web/Web.App/Web.App.csproj", "clients/Web/Web.App/"]
COPY ["clients/Web/Web.Infrastructure/Web.Infrastructure.csproj", "clients/Web/Web.Infrastructure/"]
RUN dotnet restore "clients/Web/Web.App/Web.App.csproj"
COPY . .
WORKDIR "/src/clients/Web/Web.App"
RUN dotnet build "Web.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Web.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.App.dll"]
