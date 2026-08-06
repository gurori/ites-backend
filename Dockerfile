FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ites.Application/ites.Application.csproj ites.Application/
COPY ites.Core/ites.Core.csproj ites.Core/
COPY ites.DataAccess/ites.DataAccess.csproj ites.DataAccess/
COPY ites.Infrastructure/ites.Infrastructure.csproj ites.Infrastructure/
COPY ites.Server/ites.Server.csproj ites.Server/

RUN dotnet restore ites.Server/ites.Server.csproj

COPY . .
WORKDIR /src/ites.Server
RUN dotnet publish -c Release --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/ ./

ENTRYPOINT ["dotnet", "ites.Server.dll"]