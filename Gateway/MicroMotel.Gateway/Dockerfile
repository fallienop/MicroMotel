
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY ["Gateway/MicroMotel.Gateway/*.csproj", "Gateway/MicroMotel.Gateway/"]
RUN dotnet restore "Gateway/MicroMotel.Gateway/MicroMotel.Gateway.csproj"


COPY . .

WORKDIR "/src/Gateway/MicroMotel.Gateway"
RUN dotnet build -c Release -o /app/build


FROM build AS publish
RUN dotnet publish -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroMotel.Gateway.dll"]
