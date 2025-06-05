using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using OrderService.Api.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string? serviceBusConnectionString =
    builder.Configuration["AZURE_SERVICEBUS_CONNECTIONSTRING"];

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

builder.Services.AddSingleton(
    new ServiceBusClient(serviceBusConnectionString)
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? test = builder.Configuration.GetConnectionString("DefaultConnection");

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
