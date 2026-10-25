FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Office.API/Office.API.csproj src/Office.API/
COPY src/Office.Application/Office.Application.csproj src/Office.Application/
COPY src/Office.Domain/Office.Domain.csproj src/Office.Domain/
COPY src/Office.Infrastructure/Office.Infrastructure.csproj src/Office.Infrastructure/

RUN dotnet restore src/Office.API/Office.API.csproj

COPY src/ ./src/
RUN dotnet publish src/Office.API/Office.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Office.API.dll"]
