FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5701

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ozon.Route256.Practice.ViewOrderService/", "Ozon.Route256.Practice.ViewOrderService/"]
COPY ["Ozon.Route256.Practice.ViewOrderService.Bll/", "Ozon.Route256.Practice.ViewOrderService.Bll/"]
COPY ["Ozon.Route256.Practice.ViewOrderService.Infra.Postgres/", "Ozon.Route256.Practice.ViewOrderService.Infra.Postgres/"]
COPY ["Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator/", "Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator/"]
COPY ["Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon/", "Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon/"]
COPY ["Ozon.Route256.Practice.ViewOrderService.Infra.Kafka/", "Ozon.Route256.Practice.ViewOrderService.Infra.Kafka/"]
COPY ["Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService/", "Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService/"]

RUN dotnet restore "/src/Ozon.Route256.Practice.ViewOrderService/Ozon.Route256.Practice.ViewOrderService.csproj"
COPY . .
WORKDIR "/src/Ozon.Route256.Practice.ViewOrderService"
RUN dotnet build "Ozon.Route256.Practice.ViewOrderService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ozon.Route256.Practice.ViewOrderService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ozon.Route256.Practice.ViewOrderService.dll"]
