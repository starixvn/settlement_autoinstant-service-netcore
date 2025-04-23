FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder
WORKDIR /src

COPY ./Sources/AutoInstant.Result ./AutoInstant.Result
COPY ./Sources/AutoInstant ./AutoInstant
COPY ./Sources/AutoInstant.Basketball ./AutoInstant.Basketball
COPY ./Sources/Fanex.AutoInstant.Service/Fanex.AutoInstant.Service.csproj ./Fanex.AutoInstant.Service/Fanex.AutoInstant.Service.csproj

WORKDIR /src/Fanex.AutoInstant.Service
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN --mount=type=cache,target=/root/.nuget/packages \
	dotnet restore -v n

COPY ./Sources/Fanex.AutoInstant.Service .

RUN --mount=type=cache,target=/root/.nuget/packages \
	dotnet publish -r linux-x64 --self-contained false -c release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine AS base

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.AutoInstant.Service.dll"]
