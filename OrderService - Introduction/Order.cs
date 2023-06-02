using Microsoft.EntityFrameworkCore;
namespace OrderService;

public class OrderDbContext : DbContext
{
    protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "OrderDb");
    }

    public DbSet<Order> Orders { get; set; }
}

public record Order(int Id, int ProductId, int Quantity, int CustomerId, DateTime Created);