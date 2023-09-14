#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/PhotoStock/MicroMotel.Services.PhotoStock/MicroMotel.Services.PhotoStock.csproj", "Services/PhotoStock/MicroMotel.Services.PhotoStock/"]
COPY ["Shared/MicroMotel.Shared/MicroMotel.Shared.csproj", "Shared/MicroMotel.Shared/"]
RUN dotnet restore "Services/PhotoStock/MicroMotel.Services.PhotoStock/MicroMotel.Services.PhotoStock.csproj"
COPY . .
WORKDIR "/src/Services/PhotoStock/MicroMotel.Services.PhotoStock"
RUN dotnet build "MicroMotel.Services.PhotoStock.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroMotel.Services.PhotoStock.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroMotel.Services.PhotoStock.dll"]