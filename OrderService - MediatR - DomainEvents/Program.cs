using MediatR;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OrderService.Add;
using OrderService.Behaviors;
using OrderService.Database;
using OrderService.ExternalNotifications;
using OrderService.Inventory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddDbContext<OrderDbContext>();
builder.Services.AddScoped<IInventory, Inventory>();
builder.Services.AddScoped<ICustomerNotificationSender, CustomerNotificationSender>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/",  (int? productId, OrderDbContext dbContext) =>
{
    return productId.HasValue
        ? dbContext.OrderProducts.Where(x => x.ProductId == productId.Value).ToArrayAsync()
        : dbContext.OrderProducts.ToArrayAsync();
});

app.MapPost("/add", ([AsParameters] AddOrderRequest request, IMediator mediator) 
    => mediator.Send(request));

app.Run();  