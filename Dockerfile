# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and csproj files first (better Docker layer caching)
COPY IMS.sln ./
COPY IMS.Api/IMS.Api.csproj IMS.Api/
COPY IMS.Application/IMS.Application.csproj IMS.Application/
COPY IMS.Domain/IMS.Domain.csproj IMS.Domain/
COPY IMS.Infrastructure/IMS.Infrastructure.csproj IMS.Infrastructure/

# Restore dependencies
RUN dotnet restore IMS.sln

# Copy the rest of the source
COPY . ./

# Publish the API
RUN dotnet publish IMS.Api/IMS.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# App listens on 8080 inside container
ENV ASPNETCORE_URLS=http://+:8080

# Expose container port
EXPOSE 8080

# Copy published output
COPY --from=build /app/publish ./

# Run
ENTRYPOINT ["dotnet", "IMS.Api.dll"]
