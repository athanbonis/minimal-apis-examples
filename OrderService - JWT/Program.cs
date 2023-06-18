using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService;
using System.Reflection;
using OrderService.Add;
using OrderService.Behaviors;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Format: Bearer (insert token here)",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

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
}).RequireAuthorization();

app.MapPost("/add", ([AsParameters] AddOrderRequest request, IMediator mediator) 
    => mediator.Send(request)).RequireAuthorization();

app.Run();  