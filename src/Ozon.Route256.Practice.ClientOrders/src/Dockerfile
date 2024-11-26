FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5601
EXPOSE 5602

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ozon.Route256.Practice.ClientOrders/", "Ozon.Route256.Practice.ClientOrders/"]
COPY ["Ozon.Route256.Practice.ClientOrders.Bll/", "Ozon.Route256.Practice.ClientOrders.Bll/"]
COPY ["Ozon.Route256.Practice.ClientOrders.Infra.Kafka/", "Ozon.Route256.Practice.ClientOrders.Infra.Kafka/"]
COPY ["Ozon.Route256.Practice.ClientOrders.Infra.Postgres/", "Ozon.Route256.Practice.ClientOrders.Infra.Postgres/"]
COPY ["Ozon.Route256.Practice.ClientOrders.Presentation/", "Ozon.Route256.Practice.ClientOrders.Presentation/"]
COPY ["Ozon.Route256.Practice.ClientOrders.Infra.CustomerService/", "Ozon.Route256.Practice.ClientOrders.Infra.CustomerService/"]
COPY ["Ozon.Route256.Practice.ClientOrders.Infra.OrdersService/", "Ozon.Route256.Practice.ClientOrders.Infra.OrdersService/"]

RUN dotnet restore "/src/Ozon.Route256.Practice.ClientOrders/Ozon.Route256.Practice.ClientOrders.csproj"
COPY . .
WORKDIR "/src/Ozon.Route256.Practice.ClientOrders"
RUN dotnet build "Ozon.Route256.Practice.ClientOrders.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ozon.Route256.Practice.ClientOrders.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ozon.Route256.Practice.ClientOrders.dll"]
