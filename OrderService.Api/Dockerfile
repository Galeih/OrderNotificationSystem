FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OrderService.Api/OrderService.Api.csproj", "OrderService.Api/"]
RUN dotnet restore "OrderService.Api/OrderService.Api.csproj"
COPY OrderService.Api/. ./OrderService.Api/
WORKDIR "/src/OrderService.Api"
RUN dotnet publish "OrderService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrderService.Api.dll"]
