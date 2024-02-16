using MediatR;
using OrderService.Database;

namespace OrderService.Add;

public class AddOrderHandler : IRequestHandler<AddOrderRequest, Order>
{
    private readonly IPublisher _publisher;
    private readonly OrderDbContext _dbContext;

    public AddOrderHandler(OrderDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task<Order> Handle(AddOrderRequest request, CancellationToken cancellationToken)
    {
        var order = new Order(request.CustomerId, DateTime.UtcNow);
        foreach (var op in request.Body.OrderProducts)
        {
            order.AddProduct(op.ProductId, op.Quantity);
        }

        _dbContext.Orders.Add(order);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new OrderAdded(order.Id), cancellationToken);

        return order;
    }
}