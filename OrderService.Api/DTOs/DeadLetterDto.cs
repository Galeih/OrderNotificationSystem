namespace OrderService.Api.DTOs;

public class DeadLetterDto
{
    public int Id { get; set; }

    public string MessageId { get; set; }

    public string Body { get; set; }

    public DateTime DeadLetteredAt { get; set; }

    public string ErrorReason { get; set; }

    public string ErrorDescription { get; set; }
}
