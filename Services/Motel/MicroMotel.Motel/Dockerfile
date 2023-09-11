# # Build Stage
# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# WORKDIR /app
# EXPOSE 80
# COPY Shared/MicroMotel.Shared/*.csproj Shared/MicroMotel.Shared/
# COPY Services/Motel/MicroMotel.Motel/*.csproj Services/Motel/MicroMotel.Motel/
# RUN dotnet restore Services/Motel/MicroMotel.Motel/*.csproj
# COPY . .
# RUN dotnet publish Services/Motel/MicroMotel.Motel/*.csproj -c Release -o out

# # Runtime Stage
# FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
# WORKDIR /app
# COPY --from=build /app/out .
# ENTRYPOINT [ "dotnet", "MicroMotel.Services.Motel.dll" ]

# Use the appropriate base image for your application
# For example, if it's an ASP.NET Core app:
# FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
# WORKDIR /app
# EXPOSE 80
# RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf
# RUN sed -i 's/CipherString = DEFAULT@SECLEVEL=2/CipherString = ECDHE-ECDSA-AES256-GCM-SHA384:ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES256-GCM-SHA384:ECDHE-RSA-AES128-GCM-SHA256:ECDHE-ECDSA-AES256-SHA384:ECDHE-ECDSA-AES128-SHA256:ECDHE-RSA-AES256-SHA384:ECDHE-RSA-AES128-SHA256/' /etc/ssl/openssl.cnf
# # Build and publish your application
# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# WORKDIR /src
# RUN sed -i 's/providers = provider_sect/providers = provider_sect\n\
# ssl_conf = ssl_sect\n\
# \n\
# [ssl_sect]\n\
# system_default = system_default_sect\n\
# \n\
# [system_default_sect]\n\
# Options = UnsafeLegacyRenegotiation/' /etc/ssl/openssl.cnf
# ENV DOTNET_RUNNING_IN_CONTAINER=true
# ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
# # Copy the project files and restore dependencies
# COPY ["Shared/MicroMotel.Shared/*.csproj", "Shared/MicroMotel.Shared/"]
# COPY ["Services/Motel/MicroMotel.Motel/*.csproj", "Services/Motel/MicroMotel.Motel/"]
# RUN dotnet restore "Services/Motel/MicroMotel.Motel/MicroMotel.Services.Motel.csproj"

# # Copy the application code and build
# COPY . .

# WORKDIR "/src/Services/Motel/MicroMotel.Motel"
# RUN dotnet build -c Release -o /app/build

# # Publish the application
# FROM build AS publish
# RUN dotnet publish -c Release -o /app/publish

# # Create the final image for running the application
# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "MicroMotel.Services.Motel.dll"]




# Use the appropriate base image for your application
# For example, if it's an ASP.NET Core app:
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf
RUN sed -i 's/CipherString = DEFAULT@SECLEVEL=2/CipherString = ECDHE-ECDSA-AES256-GCM-SHA384:ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES256-GCM-SHA384:ECDHE-RSA-AES128-GCM-SHA256:ECDHE-ECDSA-AES256-SHA384:ECDHE-ECDSA-AES128-SHA256:ECDHE-RSA-AES256-SHA384:ECDHE-RSA-AES128-SHA256/' /etc/ssl/openssl.cnf

# Build and publish your application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
RUN sed -i 's/providers = provider_sect/providers = provider_sect\n\
ssl_conf = ssl_sect\n\
\n\
[ssl_sect]\n\
system_default = system_default_sect\n\
\n\
[system_default_sect]\n\
Options = UnsafeLegacyRenegotiation/' /etc/ssl/openssl.cnf
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Copy the project files and restore dependencies
COPY ["Shared/MicroMotel.Shared/*.csproj", "Shared/MicroMotel.Shared/"]
COPY ["Services/Motel/MicroMotel.Motel/*.csproj", "Services/Motel/MicroMotel.Motel/"]
RUN dotnet restore "Services/Motel/MicroMotel.Motel/MicroMotel.Services.Motel.csproj"

# Copy the application code and build
COPY . .

WORKDIR "/src/Services/Motel/MicroMotel.Motel"
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Create the final image for running the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroMotel.Services.Motel.dll"]
