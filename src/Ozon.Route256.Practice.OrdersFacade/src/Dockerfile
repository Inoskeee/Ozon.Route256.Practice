FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5101
EXPOSE 5102

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ozon.Route256.Practice.OrdersFacade/", "Ozon.Route256.Practice.OrdersFacade/"]
COPY ["Ozon.Route256.Practice.OrdersFacade.Bll/", "Ozon.Route256.Practice.OrdersFacade.Bll/"]
COPY ["Ozon.Route256.Practice.OrdersFacade.Bll.Grpc/", "Ozon.Route256.Practice.OrdersFacade.Bll.Grpc/"]
COPY ["Ozon.Route256.Practice.OrdersFacade.Bll.Http/", "Ozon.Route256.Practice.OrdersFacade.Bll.Http/"]
COPY ["Ozon.Route256.Practice.OrdersFacade.Presentation/", "Ozon.Route256.Practice.OrdersFacade.Presentation/"]
RUN dotnet restore "/src/Ozon.Route256.Practice.OrdersFacade/Ozon.Route256.Practice.OrdersFacade.csproj"
COPY . .
WORKDIR "/src/Ozon.Route256.Practice.OrdersFacade"
RUN dotnet build "Ozon.Route256.Practice.OrdersFacade.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ozon.Route256.Practice.OrdersFacade.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ozon.Route256.Practice.OrdersFacade.dll"]
