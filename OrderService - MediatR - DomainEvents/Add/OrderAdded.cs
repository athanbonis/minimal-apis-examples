using MediatR;

namespace OrderService.Add;

public class OrderAdded : INotification
{
    public OrderAdded(int orderId)
    {
        OrderId = orderId;
        Created = DateTime.UtcNow;
    }
    public int OrderId { get; }
    public DateTime Created { get; }
}