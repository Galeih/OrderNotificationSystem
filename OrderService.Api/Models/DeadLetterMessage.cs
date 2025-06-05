namespace OrderService.Api.Models
{
    public class DeadLetterMessage
    {
        public int Id { get; set; }

        public string MessageId { get; set; }

        public string Body { get; set; }

        public DateTime DeadLetteredAt { get; set; } = DateTime.UtcNow;

        public string ErrorReason { get; set; }

        public string ErrorDescription { get; set; }
    }
}
