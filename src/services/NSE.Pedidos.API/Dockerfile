#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["./services/NSE.Pedidos.API/NSE.Pedidos.API.csproj", "services/NSE.Pedidos.API/"]
COPY ["./building blocks/NSE.WebApi.Core/NSE.WebApi.Core.csproj", "building blocks/NSE.WebApi.Core/"]
COPY ["./building blocks/NSE.Core/NSE.Core.csproj", "building blocks/NSE.Core/"]
COPY ["./services/NSE.Pedidos.Infra/NSE.Pedidos.Infra.csproj", "services/NSE.Pedidos.Infra/"]
COPY ["./services/NSE.Pedidos.Domain/NSE.Pedidos.Domain.csproj", "services/NSE.Pedidos.Domain/"]
COPY ["./building blocks/NSE.MessageBus/NSE.MessageBus.csproj", "building blocks/NSE.MessageBus/"]

RUN dotnet restore "services/NSE.Pedidos.API/NSE.Pedidos.API.csproj"
COPY . .
WORKDIR "services/NSE.Pedidos.API"
RUN dotnet build "NSE.Pedidos.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NSE.Pedidos.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NSE.Pedidos.API.dll"]