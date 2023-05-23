using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/add", (int productId, int quantity, [FromHeader] int customerId) => {
    var random = new Random();
    return new Order(random.Next(), productId, quantity, customerId, DateTime.UtcNow);
});

app.Run();

public record Order(int id, int productId, int quantity, int customerId, DateTime created);