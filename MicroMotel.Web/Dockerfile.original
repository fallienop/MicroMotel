
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY ["Shared/MicroMotel.Shared/*.csproj", "Shared/MicroMotel.Shared/"]
COPY ["MicroMotel.Web/*.csproj", "MicroMotel.Web/"]
RUN dotnet restore "MicroMotel.Web/MicroMotel.Web.csproj"


COPY . .

WORKDIR "/src/MicroMotel.Web"
RUN dotnet build -c Release -o /app/build


FROM build AS publish
RUN dotnet publish -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroMotel.Web.dll"]
