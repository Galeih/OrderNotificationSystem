namespace NotificationService.Worker.Services;

public class MockEmailSender : IEmailSender
{
    private static readonly Random _random = new();

    public Task SendOrderNotificationAsync(string recipientEmail, string orderBody)
    {
        // Simule une erreur 50% du temps (à ajuster selon ta démo)
        if (_random.NextDouble() < 0.5)
        {
            Console.WriteLine($"[MOCK EMAIL] Échec simulé d'envoi à {recipientEmail}");
            throw new InvalidOperationException("Erreur simulée d’envoi d’email (mock)");
        }

        Console.WriteLine($"[MOCK EMAIL] Envoi réussi à {recipientEmail} : {orderBody}");
        return Task.CompletedTask;
    }
}
