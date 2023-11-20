#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/mssql/server:2019-latest

ENV SA_PASSWORD <Password123$>
ENV ACCEPT_EULA Y

# If we use volumes, the owner of this directory
# is root.  So we need to set them to mssql, but
# at this point they do not yet exist.
# Create the directory first, and set the permissions.
USER root
RUN mkdir -p /mssql/data
RUN chown -R mssql: mssql/data

USER mssql

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["UserManagement-API/User-Management-Web-Api.csproj", "UserManagement-API/"]
RUN dotnet restore "./UserManagement-API/./User-Management-Web-Api.csproj"
COPY . .
WORKDIR "/src/UserManagement-API"
RUN dotnet build "./User-Management-Web-Api.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./User-Management-Web-Api.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

RUN dotnet dev-certs https

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User-Management-Web-Api.dll"]