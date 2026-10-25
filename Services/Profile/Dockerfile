FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Profile.API/Profile.API.csproj src/Profile.API/
COPY src/Profile.Application/Profile.Application.csproj src/Profile.Application/
COPY src/Profile.Domain/Profile.Domain.csproj src/Profile.Domain/
COPY src/Profile.Infrastructure/Profile.Infrastructure.csproj src/Profile.Infrastructure/

RUN dotnet restore src/Profile.API/Profile.API.csproj

COPY src/ ./src/
RUN dotnet publish src/Profile.API/Profile.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Profile.API.dll"]
