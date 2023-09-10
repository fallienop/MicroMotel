# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80
COPY Shared/MicroMotel.Shared/*.csproj Shared/MicroMotel.Shared/
COPY Services/Motel/MicroMotel.Motel/*.csproj Services/Motel/MicroMotel.Motel/
RUN dotnet restore Services/Motel/MicroMotel.Motel/*.csproj
COPY . .
RUN dotnet publish Services/Motel/MicroMotel.Motel/*.csproj -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "MicroMotel.Services.Motel.dll" ]
