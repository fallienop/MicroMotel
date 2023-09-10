FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /app
EXPOSE 80
RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf
RUN sed -i 's/CipherString = DEFAULT@SECLEVEL=2/CipherString = ECDHE-ECDSA-AES256-GCM-SHA384:ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES256-GCM-SHA384:ECDHE-RSA-AES128-GCM-SHA256:ECDHE-ECDSA-AES256-SHA384:ECDHE-ECDSA-AES128-SHA256:ECDHE-RSA-AES256-SHA384:ECDHE-RSA-AES128-SHA256/' /etc/ssl/openssl.cnf

# OpenSSL 3.0 disables UnsafeLegacyRenegotiation by default, must re-enable it for some endpoints (see https://github.com/dotnet/runtime/issues/80641)
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
COPY Shared/MicroMotel.Shared/*.csproj Shared/MicroMotel.Shared/
COPY IdentityServer/MicroMotel.IdentityServer/*.csproj IdentityServer/MicroMotel.IdentityServer/  
RUN dotnet restore IdentityServer/MicroMotel.IdentityServer/*.csproj
COPY . .
RUN dotnet publish IdentityServer/MicroMotel.IdentityServer/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","MicroMotel.IdentityServer.dll" ]