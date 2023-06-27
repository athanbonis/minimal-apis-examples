using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using OrderService.Add;
using OrderService.Behaviors;
using Microsoft.OpenApi.Models;
using OrderService.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Authentication:Schemes:Bearer:SigningKeys:0:Value"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Authentication:Schemes:Bearer:ValidIssuer"],
            ValidateAudience = true,
            ValidAudiences = builder.Configuration.GetSection("Authentication:Schemes:Bearer:ValidAudiences").Get<List<string>>(),
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? throw new InvalidOperationException("Signing Key not found."))),
            ValidateIssuerSigningKey = true,
        };
    });

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

app.MapPost("/login", async ([FromBody] LoginRequest request, IMediator mediator) =>
{
    var loginResponse = await mediator.Send(request);

    return loginResponse.Success 
        ? Results.Ok(loginResponse.Token) 
        : Results.BadRequest(loginResponse.Error);
});

app.MapGet("/",  (int? productId, OrderDbContext dbContext) =>
{
    return productId.HasValue
        ? dbContext.Orders.Where(x => x.ProductId == productId.Value).ToArrayAsync()
        : dbContext.Orders.ToArrayAsync();
}).RequireAuthorization();

app.MapPost("/add", ([AsParameters] AddOrderRequest request, IMediator mediator) 
    => mediator.Send(request)).RequireAuthorization();

app.Run();  