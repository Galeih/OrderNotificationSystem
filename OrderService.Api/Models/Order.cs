namespace OrderService.Api.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CustomerName { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;
}

