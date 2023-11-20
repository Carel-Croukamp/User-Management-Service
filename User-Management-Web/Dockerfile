#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8090
EXPOSE 8091

RUN mkdir /app/build
RUN mkdir /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["./User-Management-Web/User-Management-Web.csproj", "User-Management-Web/"]
RUN dotnet restore "./User-Management-Web/User-Management-Web.csproj"
COPY . .
WORKDIR "/src/User-Management-Web"
RUN dotnet build "./User-Management-Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./User-Management-Web.csproj" -c $BUILD_CONFIGURATION -o /app/wwwroot /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User-Management-Web.dll"]
