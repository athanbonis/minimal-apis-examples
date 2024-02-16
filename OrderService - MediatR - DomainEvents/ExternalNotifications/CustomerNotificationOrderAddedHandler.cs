using MediatR;
using OrderService.Add;

namespace OrderService.ExternalNotifications;

public class CustomerNotificationOrderAddedHandler : INotificationHandler<OrderAdded>
{
    private readonly ICustomerNotificationSender _notificationSender;

    public CustomerNotificationOrderAddedHandler(ICustomerNotificationSender notificationSender)
    {
        _notificationSender = notificationSender;
    }

    public Task Handle(OrderAdded notification, CancellationToken cancellationToken)
    {
        return _notificationSender.SendOrderConfirmed(notification.OrderId);
    }
}