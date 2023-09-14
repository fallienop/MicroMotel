#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MicroMotel.Web/MicroMotel.Web.csproj", "MicroMotel.Web/"]
COPY ["Shared/MicroMotel.Shared/MicroMotel.Shared.csproj", "Shared/MicroMotel.Shared/"]
RUN dotnet restore "MicroMotel.Web/MicroMotel.Web.csproj"
COPY . .
WORKDIR "/src/MicroMotel.Web"
RUN dotnet build "MicroMotel.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroMotel.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroMotel.Web.dll"]