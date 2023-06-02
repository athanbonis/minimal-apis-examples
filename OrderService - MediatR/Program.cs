using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService;
using System.Reflection;
using OrderService.Add;
using OrderService.Behaviors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
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

app.MapPost("/add", ([AsParameters] AddOrderRequest request, IMediator mediator) 
    => mediator.Send(request));

app.Run();  