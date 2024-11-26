FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Ozon.Route256.Practice.DataGenerator/", "Ozon.Route256.Practice.DataGenerator/Ozon.Route256.Practice.DataGenerator/"]
COPY ["src/Ozon.Route256.Practice.DataGenerator.Infra/", "Ozon.Route256.Practice.DataGenerator/Ozon.Route256.Practice.DataGenerator.Infra/"]
COPY ["src/Ozon.Route256.Practice.DataGenerator.Bill/", "Ozon.Route256.Practice.DataGenerator/Ozon.Route256.Practice.DataGenerator.Bill/"]
COPY ["src/Ozon.Route256.Practice.DataGenerator.Proto/", "Ozon.Route256.Practice.DataGenerator/Ozon.Route256.Practice.DataGenerator.Proto/"]
RUN dotnet restore "/src/Ozon.Route256.Practice.DataGenerator/Ozon.Route256.Practice.DataGenerator/Ozon.Route256.Practice.DataGenerator.csproj"
COPY . .
WORKDIR "/src/Ozon.Route256.Practice.DataGenerator/Ozon.Route256.Practice.DataGenerator"
RUN dotnet build "Ozon.Route256.Practice.DataGenerator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ozon.Route256.Practice.DataGenerator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ozon.Route256.Practice.DataGenerator.dll"]
