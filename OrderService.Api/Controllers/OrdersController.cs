using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Api.Data;
using OrderService.Api.Models;
using System.Text.Json;

namespace OrderService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ServiceBusClient _busClient;
    private readonly string _topicName = "orders";
    private readonly OrderDbContext _dbContext;

    public OrdersController(OrderDbContext dbContext, ServiceBusClient busClient)
    {
        _dbContext = dbContext;
        _busClient = busClient;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Order order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();

        ServiceBusSender sender = _busClient.CreateSender("orders");
        string jsonBody = JsonSerializer.Serialize(order);

        ServiceBusMessage message = new(jsonBody);
        await sender.SendMessageAsync(message);

        return Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        List<Order> orders = await _dbContext.Orders.ToListAsync();
        return Ok(orders);
    }

}
