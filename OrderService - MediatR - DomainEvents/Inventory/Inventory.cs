namespace OrderService.Inventory;

public interface IInventory
{
    Task ProductsPicked(IDictionary<int, int> productsWithQuantity);
}

public class Inventory : IInventory
{
    public Task ProductsPicked(IDictionary<int, int> productsWithQuantity)
    {
        return Task.CompletedTask;
    }
}