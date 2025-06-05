using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using NotificationService.Worker;
using NotificationService.Worker.Services;
using OrderService.Api.Data;
using OrderService.Api.Models;

namespace NotificationService.Worker;

public class Worker : BackgroundService
{
    private readonly ServiceBusClient _client;
    private readonly string _topicName = "orders";
    private readonly string _subscriptionName = "notifworker";
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IEmailSender _emailSender;

    public Worker(ServiceBusClient client, IServiceScopeFactory scopeFactory, IEmailSender emailSender)
    {
        _client = client;
        _scopeFactory = scopeFactory;
        _emailSender = emailSender;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = ProcessDeadLettersAsync(stoppingToken);

        ServiceBusProcessorOptions processorOptions = new()
        {
            AutoCompleteMessages = false
        };
        ServiceBusProcessor processor = _client.CreateProcessor(_topicName, _subscriptionName, processorOptions);

        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;
        await processor.StartProcessingAsync(stoppingToken);

        await Task.Delay(-1, stoppingToken);
    }

    async Task MessageHandler(ProcessMessageEventArgs args)
    {
        try
        {
            string json = args.Message.Body.ToString();
            Console.WriteLine($"[Notification] Nouvelle commande reçue : {json}");

            // Simuler un destinataire (exemple simple)
            string recipient = "test@demo.fr";

            // Tente d’envoyer un email (vrai ou simulé)
            await _emailSender.SendOrderNotificationAsync(recipient, json);

            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Impossible de traiter le message (mail) : {ex.Message}");
            // On laisse l’erreur générer un dead letter automatiquement
            await args.AbandonMessageAsync(args.Message);
        }
    }

    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }

    private async Task ProcessDeadLettersAsync(CancellationToken stoppingToken)
    {
        ServiceBusReceiver deadLetterReceiver = _client.CreateReceiver(_topicName, _subscriptionName, new ServiceBusReceiverOptions
        {
            SubQueue = SubQueue.DeadLetter
        });

        while (!stoppingToken.IsCancellationRequested)
        {
            ServiceBusReceivedMessage message = await deadLetterReceiver.ReceiveMessageAsync(TimeSpan.FromSeconds(5));

            if (message != null)
            {

                DeadLetterMessage deadLetter = new()
                {
                    MessageId = message.MessageId,
                    Body = message.Body.ToString(),
                    ErrorReason = message.DeadLetterReason,
                    ErrorDescription = message.DeadLetterErrorDescription
                };

                using (IServiceScope scope = _scopeFactory.CreateScope())
                {
                    OrderDbContext db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                    db.DeadLetters.Add(deadLetter);
                    await db.SaveChangesAsync();
                }

                await deadLetterReceiver.CompleteMessageAsync(message);
            }
            else
            {
                await Task.Delay(5000);
            }
        }
    }

}
