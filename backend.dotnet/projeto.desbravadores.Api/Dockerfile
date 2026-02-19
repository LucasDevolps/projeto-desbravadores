FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY projeto.desbravadores.Api/projeto.desbravadores.Api.csproj projeto.desbravadores.Api/
COPY projeto.desbravadores.Application/projeto.desbravadores.Application.csproj projeto.desbravadores.Application/
COPY projeto.desbravadores.Domain/projeto.desbravadores.Domain.csproj projeto.desbravadores.Domain/
COPY projeto.desbravadores.Infrastructure/projeto.desbravadores.Infrastructure.csproj projeto.desbravadores.Infrastructure/

RUN dotnet restore projeto.desbravadores.Api/projeto.desbravadores.Api.csproj

COPY . .

RUN dotnet publish projeto.desbravadores.Api/projeto.desbravadores.Api.csproj \
    -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "projeto.desbravadores.Api.dll"]
