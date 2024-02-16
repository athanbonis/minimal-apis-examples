using Microsoft.EntityFrameworkCore;

namespace OrderService.Database;

public class OrderDbContext : DbContext
{
    protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "OrderDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderProduct>().HasKey(x => new { x.OrderId, x.ProductId });
        modelBuilder.Entity<Order>().HasMany(x => x.OrderProducts).WithOne().HasForeignKey(o => o.OrderId);
        
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
}
