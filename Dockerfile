FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src
COPY ["StudentManagementSystem.csproj", "./"]
RUN dotnet restore "./StudentManagementSystem.csproj"

COPY . .
WORKDIR "/src"
RUN dotnet build "StudentManagementSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StudentManagementSystem.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentManagementSystem.dll"]

ENV ASPNETCORE_URLS=https://+:80
