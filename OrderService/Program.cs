using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrderDbContext>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/",  (int? productId, OrderDbContext dbContext) =>
{
    return productId.HasValue
        ? dbContext.Orders.Where(x => x.ProductId == productId.Value).ToArrayAsync()
        : dbContext.Orders.ToArrayAsync();
});

app.MapPost("/add", async (int productId, int quantity, [FromHeader] int customerId, OrderDbContext dbContext) => {
    var random = new Random();

    var order = new Order(random.Next(), productId, quantity, customerId, DateTime.UtcNow);

    dbContext.Orders.Add(order);
    await dbContext.SaveChangesAsync();

    return order;
});

app.Run();