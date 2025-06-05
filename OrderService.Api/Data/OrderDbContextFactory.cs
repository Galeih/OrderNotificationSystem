using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace OrderService.Api.Data;

public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
{
    public OrderDbContext CreateDbContext(string[] args)
    {
        string basePath = Directory.GetCurrentDirectory();

        IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets<OrderDbContextFactory>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        string? connectionString = config.GetConnectionString("DefaultConnection");

        DbContextOptionsBuilder<OrderDbContext> optionsBuilder = new();
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new OrderDbContext(optionsBuilder.Options);
    }
}
