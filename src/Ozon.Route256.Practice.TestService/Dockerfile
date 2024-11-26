FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Directory.Build.targets", "."]
COPY ["Directory.Packages.props", "."]
COPY ["src/Ozon.Route256.Practice.TestService/", "Ozon.Route256.Practice.TestService/"]
COPY ["src/Ozon.Route256.Practice.TestService.Common.Kafka/", "Ozon.Route256.Practice.TestService.Common.Kafka/"]
COPY ["src/Ozon.Route256.Practice.TestService.Data.Customers/", "Ozon.Route256.Practice.TestService.Data.Customers/"]
COPY ["src/Ozon.Route256.Practice.TestService.Common.Data/", "Ozon.Route256.Practice.TestService.Common.Data/"]
COPY ["src/Ozon.Route256.Practice.TestService.Data.Orders/", "Ozon.Route256.Practice.TestService.Data.Orders/"]
COPY ["src/Ozon.Route256.Practice.TestService.Domain/", "Ozon.Route256.Practice.TestService.Domain/"]
COPY ["src/Ozon.Route256.Practice.TestService.Data/", "Ozon.Route256.Practice.TestService.Data/"]
COPY ["src/Ozon.Route256.Practice.TestService.Integrations/", "Ozon.Route256.Practice.TestService.Integrations/"]
RUN dotnet restore "/src/Ozon.Route256.Practice.TestService/Ozon.Route256.Practice.TestService.csproj"
COPY . .
WORKDIR "/src/Ozon.Route256.Practice.TestService"
RUN dotnet build "Ozon.Route256.Practice.TestService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ozon.Route256.Practice.TestService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ozon.Route256.Practice.TestService.dll"]
