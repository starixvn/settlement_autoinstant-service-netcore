FROM hub.nexdev.net/base/dotnet/core/sdk:7.0-alpine AS builder
WORKDIR /src

COPY ./Sources/AutoInstant.Result ./AutoInstant.Result
COPY ./Sources/AutoInstant ./AutoInstant
COPY ./Sources/Fanex.AutoInstant.Service/Fanex.AutoInstant.Service.csproj ./Fanex.AutoInstant.Service/Fanex.AutoInstant.Service.csproj

WORKDIR /src/Fanex.AutoInstant.Service
RUN dotnet nuget add source http://nuget.nexdev.net/api/v2
RUN dotnet restore -v n

COPY ./Sources/Fanex.AutoInstant.Service .

RUN dotnet publish -r linux-x64 --self-contained false -c release -o /app 

FROM hub.nexdev.net/base/dotnet/core/aspnet:7.0 AS base

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.AutoInstant.Service.dll"]
