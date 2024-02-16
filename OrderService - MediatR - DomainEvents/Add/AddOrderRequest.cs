using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Add;

public class AddOrderRequest : IRequest<Order>
{
    [FromHeader]
    public int CustomerId { get; set; }
    
    [FromBody]
    public AddOrderRequestBody Body { get; set; }
}

public class AddOrderRequestBody
{
    public RequestOrderProduct[] OrderProducts { get; init; } 
}

public record RequestOrderProduct(int ProductId, int Quantity);