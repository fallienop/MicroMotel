
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf
RUN sed -i 's/CipherString = DEFAULT@SECLEVEL=2/CipherString = ECDHE-ECDSA-AES256-GCM-SHA384:ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES256-GCM-SHA384:ECDHE-RSA-AES128-GCM-SHA256:ECDHE-ECDSA-AES256-SHA384:ECDHE-ECDSA-AES128-SHA256:ECDHE-RSA-AES256-SHA384:ECDHE-RSA-AES128-SHA256/' /etc/ssl/openssl.cnf


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

COPY ["Shared/MicroMotel.Shared/*.csproj", "Shared/MicroMotel.Shared/"]
COPY ["Services/FakePayment/MicroMotel.Services.FakePayment/*.csproj", "Services/FakePayment/MicroMotel.Services.FakePayment/"]
RUN dotnet restore "Services/FakePayment/MicroMotel.Services.FakePayment/MicroMotel.Services.FakePayment.csproj"


COPY . .

WORKDIR "/src/Services/FakePayment/MicroMotel.Services.FakePayment"
RUN dotnet build -c Release -o /app/build


FROM build AS publish
RUN dotnet publish -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroMotel.Services.FakePayment.dll"]
