using MediatR;

namespace OrderService.Add;

public class AddOrderHandler : IRequestHandler<AddOrderRequest, Order>
{
    private readonly OrderDbContext _dbContext;

    public AddOrderHandler(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> Handle(AddOrderRequest request, CancellationToken cancellationToken)
    {
        var random = new Random();

        var order = new Order(random.Next(), request.ProductId, request.Quantity, request.CustomerId, DateTime.UtcNow);

        _dbContext.Orders.Add(order);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return order;
    }
}