# Stage 1: Build application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["auramode.csproj", "./"]
RUN dotnet restore "./auramode.csproj"

COPY . .
RUN dotnet publish "auramode.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Production runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "auramode.dll"]
