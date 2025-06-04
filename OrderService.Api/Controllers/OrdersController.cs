using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Models;
using System.Text.Json;

namespace OrderService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ServiceBusClient _busClient;
    private readonly string _topicName = "orders";

    public OrdersController(ServiceBusClient busClient)
    {
        _busClient = busClient;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Order order)
    {
        ServiceBusSender sender = _busClient.CreateSender(_topicName);
        string jsonBody = JsonSerializer.Serialize(order);
        ServiceBusMessage message = new(jsonBody);

        await sender.SendMessageAsync(message);
        return Ok(order);
    }
}
