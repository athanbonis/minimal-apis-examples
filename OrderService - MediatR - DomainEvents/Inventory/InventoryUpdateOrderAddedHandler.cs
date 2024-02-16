using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Add;
using OrderService.Database;

namespace OrderService.Inventory;

public class InventoryUpdateOrderAddedHandler : INotificationHandler<OrderAdded>
{
    private readonly IInventory _inventory;
    private readonly OrderDbContext _orderDbContext;

    public InventoryUpdateOrderAddedHandler(IInventory inventory, OrderDbContext orderDbContext)
    {
        _inventory = inventory;
        _orderDbContext = orderDbContext;
    }
    
    public async Task Handle(OrderAdded notification, CancellationToken cancellationToken)
    {
        var order = await _orderDbContext.Orders
            .Include(x => x.OrderProducts)
            .FirstOrDefaultAsync(x => x.Id == notification.OrderId, cancellationToken: cancellationToken);

        if (order is null)
        {
            throw new InvalidOperationException($"Order with Id [{notification.OrderId}] does not exist in the system.");
        }

        var pickedProducts = order.OrderProducts.ToDictionary(x => x.ProductId, x => x.Quantity);
        await _inventory.ProductsPicked(pickedProducts);
    }
}