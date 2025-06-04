using Azure.Messaging.ServiceBus;

namespace NotificationService.Worker;

public class Worker : BackgroundService
{
    private readonly ServiceBusClient _client;
    private readonly string _topicName = "orders";
    private readonly string _subscriptionName = "notifworker";

    public Worker(ServiceBusClient client)
    {
        _client = client;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ServiceBusProcessor processor = _client.CreateProcessor(_topicName, _subscriptionName, new ServiceBusProcessorOptions());
        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;
        await processor.StartProcessingAsync(stoppingToken);

        // Permet de garder le worker en vie
        await Task.Delay(-1, stoppingToken);
    }

    async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string json = args.Message.Body.ToString();
        Console.WriteLine($"[Notification] Nouvelle commande reçue : {json}");
        await args.CompleteMessageAsync(args.Message);
    }

    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}
