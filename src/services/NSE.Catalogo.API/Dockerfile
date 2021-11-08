#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["./services/NSE.Catalogo.API/NSE.Catalogo.API.csproj", "services/NSE.Catalogo.API/"]
COPY ["./building blocks/NSE.WebApi.Core/NSE.WebApi.Core.csproj", "building blocks/NSE.WebApi.Core/"]
COPY ["./building blocks/NSE.Core/NSE.Core.csproj", "building blocks/NSE.Core/"]
COPY ["./building blocks/NSE.MessageBus/NSE.MessageBus.csproj", "building blocks/NSE.MessageBus/"]
RUN dotnet restore "services/NSE.Catalogo.API/NSE.Catalogo.API.csproj"
COPY . .
WORKDIR "services/NSE.Catalogo.API"
RUN dotnet build "NSE.Catalogo.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NSE.Catalogo.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NSE.Catalogo.API.dll"]