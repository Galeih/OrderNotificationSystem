FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["NotificationService.Worker/NotificationService.Worker.csproj", "NotificationService.Worker/"]
COPY ["OrderService.Api/OrderService.Api.csproj", "OrderService.Api/"]

RUN dotnet restore "NotificationService.Worker/NotificationService.Worker.csproj"

COPY NotificationService.Worker/. ./NotificationService.Worker/
COPY OrderService.Api/. ./OrderService.Api/

WORKDIR "/src/NotificationService.Worker"
RUN dotnet publish "NotificationService.Worker.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.Worker.dll"]
