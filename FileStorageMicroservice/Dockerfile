# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FileStorageMicroservice/FileStorageMicroservice.csproj", "FileStorageMicroservice/"]
RUN dotnet restore "FileStorageMicroservice/FileStorageMicroservice.csproj"
COPY . .

WORKDIR "/src/FileStorageMicroservice"
RUN dotnet build "FileStorageMicroservice.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "FileStorageMicroservice.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage - runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Optional: Set environment variables here if necessary
# ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "FileStorageMicroservice.dll"]
