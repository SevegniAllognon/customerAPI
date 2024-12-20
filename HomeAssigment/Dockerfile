#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat


FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
# Expose HTTP  ports
EXPOSE 8080

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
# Copy the project file and restore dependencies
WORKDIR /src
COPY ["HomeAssigment.csproj", "."]
RUN dotnet restore "./HomeAssigment.csproj"
# Copy all files and publish the application
COPY . .
WORKDIR "/src/."
RUN dotnet build "./HomeAssigment.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HomeAssigment.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
# Stage 2: Runtime
WORKDIR /app
COPY --from=publish /app/publish .
# Run the API
ENTRYPOINT ["dotnet", "HomeAssigment.dll"]









