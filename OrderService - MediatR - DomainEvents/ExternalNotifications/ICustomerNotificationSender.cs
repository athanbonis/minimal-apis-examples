namespace OrderService.ExternalNotifications;

public interface ICustomerNotificationSender
{
    public Task SendOrderConfirmed(int orderId);
}

public class CustomerNotificationSender : ICustomerNotificationSender
{
    public Task SendOrderConfirmed(int orderId)
    {
        return Task.CompletedTask;
    }
}