FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5001
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY [".", "Ozon.Route256.Practice.CustomerService/"]
RUN dotnet restore "/src/Ozon.Route256.Practice.CustomerService/Ozon.Route256.Practice.CustomerService.csproj"
COPY . .
WORKDIR "/src/Ozon.Route256.Practice.CustomerService"
RUN dotnet build "Ozon.Route256.Practice.CustomerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ozon.Route256.Practice.CustomerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ozon.Route256.Practice.CustomerService.dll"]

