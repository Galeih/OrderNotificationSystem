using Azure.Messaging.ServiceBus;
using NotificationService.Worker;
using OrderService.Api.Data;
using Microsoft.EntityFrameworkCore;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

builder.Services.AddHostedService<Worker>();

string? serviceBusConnectionString =
    builder.Configuration["AZURE_SERVICEBUS_CONNECTIONSTRING"];

builder.Services.AddSingleton(
    new ServiceBusClient(serviceBusConnectionString)
);

IHost host = builder.Build();
host.Run();
