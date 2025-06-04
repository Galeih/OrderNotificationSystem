using NotificationService.Worker;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

string? serviceBusConnectionString =
    Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_CONNECTIONSTRING")
    ?? builder.Configuration["ServiceBus:ConnectionString"];

builder.Services.AddSingleton(
    new Azure.Messaging.ServiceBus.ServiceBusClient(serviceBusConnectionString)
);

IHost host = builder.Build();
host.Run();
