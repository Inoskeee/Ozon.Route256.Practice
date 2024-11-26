FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5801
EXPOSE 5802

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ozon.Route256.Practice.ProductCardService.Host/", "Ozon.Route256.Practice.ProductCardService.Host/"]
COPY ["Ozon.Route256.Practice.ProductCardService.Application/", "Ozon.Route256.Practice.ProductCardService.Application/"]
COPY ["Ozon.Route256.Practice.ProductCardService.Domain/", "Ozon.Route256.Practice.ProductCardService.Domain/"]
COPY ["Ozon.Route256.Practice.ProductCardService.Infrastructure/", "Ozon.Route256.Practice.ProductCardService.Infrastructure/"]
COPY ["Ozon.Route256.Practice.ProductCardService.Presentation/", "Ozon.Route256.Practice.ProductCardService.Presentation/"]
RUN dotnet restore "/src/Ozon.Route256.Practice.ProductCardService.Host/Ozon.Route256.Practice.ProductCardService.Host.csproj"
COPY . .
WORKDIR "/src/Ozon.Route256.Practice.ProductCardService.Host"
RUN dotnet build "Ozon.Route256.Practice.ProductCardService.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ozon.Route256.Practice.ProductCardService.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ozon.Route256.Practice.ProductCardService.Host.dll"]
