﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5501

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ozon.Route256.Practice.OrdersReport/", "Ozon.Route256.Practice.OrdersReport/"]
COPY ["Ozon.Route256.Practice.OrdersReport.Presentation/", "Ozon.Route256.Practice.OrdersReport.Presentation/"]
COPY ["Ozon.Route256.Practice.OrdersReport.Bll/", "Ozon.Route256.Practice.OrdersReport.Bll/"]
COPY ["Ozon.Route256.Practice.OrdersReport.Bll.Contracts/", "Ozon.Route256.Practice.OrdersReport.Bll.Contracts/"]

RUN dotnet restore "/src/Ozon.Route256.Practice.OrdersReport/Ozon.Route256.Practice.OrdersReport.csproj"
COPY . .
WORKDIR "/src/Ozon.Route256.Practice.OrdersReport"
RUN dotnet build "Ozon.Route256.Practice.OrdersReport.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ozon.Route256.Practice.OrdersReport.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ozon.Route256.Practice.OrdersReport.dll"]
