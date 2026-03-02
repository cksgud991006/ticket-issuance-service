# --- Stage 1: Build ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["TicketServer.csproj", "./TicketServer"]
RUN dotnet restore "TicketServer/TicketServer.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish to /app/out
RUN dotnet publish "TicketServer/TicketServer.csproj" -c Release -o /app/out

# --- Stage 2: Runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/out .

# Run the app
ENTRYPOINT ["dotnet", "TicketServer.dll"]