namespace OrderService;

public class Order
{
    private readonly List<OrderProduct> _orderProducts = new();

    public Order(int customerId, DateTime created)
    {
        Id = new Random().Next(1000,10000000); // Random is used only as an example here. Do not use this logic in production code. 
        CustomerId = customerId;
        Created = created;
    }

    public int Id { get; init; }
    public IReadOnlyCollection<OrderProduct> OrderProducts => _orderProducts.AsReadOnly();
    public int CustomerId { get; init; }
    public DateTime Created { get; init; }

    public void AddProduct(int productId, int quantity)
    {
        var orderProduct = new OrderProduct(Id, productId, quantity);
        _orderProducts.Add(orderProduct);
    }
}

public class OrderProduct
{
    internal OrderProduct(int orderId, int productId, int quantity)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
    }

    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
}
