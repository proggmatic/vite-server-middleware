FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["examples/Proggmatic.ViteAspNetExample/Proggmatic.ViteAspNetExample.csproj", "examples/Proggmatic.ViteAspNetExample/"]
RUN dotnet restore "examples/Proggmatic.ViteAspNetExample/Proggmatic.ViteAspNetExample.csproj"
COPY . .
WORKDIR "/src/examples/Proggmatic.ViteAspNetExample"
RUN dotnet build "Proggmatic.ViteAspNetExample.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Proggmatic.ViteAspNetExample.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Proggmatic.ViteAspNetExample.dll"]
