// NotificationService.Worker/Services/EmailSender.cs
using MailKit.Net.Smtp;
using MimeKit;

namespace NotificationService.Worker.Services;

public interface IEmailSender
{
    Task SendOrderNotificationAsync(string recipientEmail, string orderBody);
}

public class EmailSender : IEmailSender
{
    private readonly string _smtpHost = "sandbox.smtp.mailtrap.io"; // Mailtrap, par exemple
    private readonly int _smtpPort = 587;
    private readonly string _smtpUser = "TON_USER";
    private readonly string _smtpPass = "TON_PASS";

    public async Task SendOrderNotificationAsync(string recipientEmail, string orderBody)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse("noreply@demo-order.com"));
        message.To.Add(MailboxAddress.Parse(recipientEmail));
        message.Subject = "Nouvelle commande reçue !";
        message.Body = new TextPart("plain")
        {
            Text = orderBody
        };

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(_smtpHost, _smtpPort, false);
            await smtp.AuthenticateAsync(_smtpUser, _smtpPass);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            // On laisse l’exception se propager pour tester les dead letters !
            throw new InvalidOperationException($"Erreur lors de l'envoi d'email : {ex.Message}", ex);
        }
    }
}
