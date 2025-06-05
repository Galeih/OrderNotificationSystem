using Azure.Messaging.ServiceBus;
using NotificationService.Worker;
using OrderService.Api.Data;
using Microsoft.EntityFrameworkCore;
using NotificationService.Worker.Services;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

string? serviceBusConnectionString =
    builder.Configuration["AZURE_SERVICEBUS_CONNECTIONSTRING"];

builder.Services.AddSingleton(
    new ServiceBusClient(serviceBusConnectionString)
);

IHost host = builder.Build();
host.Run();
