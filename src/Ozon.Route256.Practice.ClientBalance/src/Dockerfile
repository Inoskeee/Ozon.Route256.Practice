FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5401
EXPOSE 5402

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ozon.Route256.Practice.ClientBalance/", "Ozon.Route256.Practice.ClientBalance/"]
COPY ["Ozon.Route256.Practice.ClientBalance.Bll/", "Ozon.Route256.Practice.ClientBalance.Bll/"]
COPY ["Ozon.Route256.Practice.ClientBalance.Dal/", "Ozon.Route256.Practice.ClientBalance.Dal/"]
COPY ["Ozon.Route256.Practice.ClientBalance.Presentation/", "Ozon.Route256.Practice.ClientBalance.Presentation/"]

RUN dotnet restore "/src/Ozon.Route256.Practice.ClientBalance/Ozon.Route256.Practice.ClientBalance.csproj"
COPY . .
WORKDIR "/src/Ozon.Route256.Practice.ClientBalance"
RUN dotnet build "Ozon.Route256.Practice.ClientBalance.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ozon.Route256.Practice.ClientBalance.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ozon.Route256.Practice.ClientBalance.dll"]
