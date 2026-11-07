FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Services.API/Services.API.csproj src/Services.API/
COPY src/Services.Application/Services.Application.csproj src/Services.Application/
COPY src/Services.Domain/Services.Domain.csproj src/Services.Domain/
COPY src/Services.Infrastructure/Services.Infrastructure.csproj src/Services.Infrastructure/

RUN dotnet restore src/Services.API/Services.API.csproj

COPY src/ ./src/
RUN dotnet publish src/Services.API/Services.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Services.API.dll"]
