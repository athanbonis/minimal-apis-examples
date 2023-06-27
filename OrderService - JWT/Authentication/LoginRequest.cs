using MediatR;

namespace OrderService.Authentication;

public class LoginRequest : IRequest<LoginResponse>
{
    public string Username { get; set; }
    public string Password { get; set; }
}