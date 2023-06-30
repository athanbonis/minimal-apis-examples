using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Add;

public class AddOrderRequest : IRequest<Order>
{
    [FromQuery]
    public int ProductId { get; set; }
    [FromQuery]
    public int Quantity { get; set; }
    [FromHeader]
    public int CustomerId { get; set; }
}