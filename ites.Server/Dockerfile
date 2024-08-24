FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY ["ites.Server/ites.Server.csproj", "ites.Server/"]
RUN dotnet restore "./ites.Server/ites.Server.csproj"

COPY . .
RUN dotnet publish "./ites.Server/ites.Server.csproj" -c Release -o out /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "ites.Server.dll"]
