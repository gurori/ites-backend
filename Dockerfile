FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ./ites.Server/ites.Server.csproj ./ites.Server/
RUN dotnet restore "./ites.Server/ites.Server.csproj"

COPY . .
WORKDIR /src/ites.Server
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "ites.Server.dll"]