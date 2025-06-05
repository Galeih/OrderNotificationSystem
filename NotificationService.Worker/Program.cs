using Azure.Messaging.ServiceBus;
using NotificationService.Worker;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

string? serviceBusConnectionString =
    builder.Configuration["AZURE_SERVICEBUS_CONNECTIONSTRING"];

builder.Services.AddSingleton(
    new ServiceBusClient(serviceBusConnectionString)
);

IHost host = builder.Build();
host.Run();
