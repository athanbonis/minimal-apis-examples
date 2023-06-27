using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace OrderService.Authentication
{
    public class LoginRequestHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly IConfiguration _configuration;

        public LoginRequestHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            LoginResponse response;

            await Task.Delay(50, cancellationToken); // simulate request
            if (request is { Username: "test", Password: "test" }) // fake check for demo purposes
            {
                var token = GenerateJtwBearer(request.Username);

                response = new LoginResponse
                {
                    Success = true,
                    Token = token
                };
            }
            else
            {
                response = new LoginResponse
                {
                    Success = false,
                    Error =
                        "The credentials that you provided are invalid. Please check the username and the password and try again."
                };
            }

            return response;
        }

        private string GenerateJtwBearer(string username)
        {
            var singingKey = _configuration["Authentication:Schemes:Bearer:SigningKeys:0:Value"];
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(singingKey ?? throw new InvalidOperationException("Signing key not found.")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var issuer = _configuration["Authentication:Schemes:Bearer:ValidIssuer"];
            var audiences = _configuration.GetSection("Authentication:Schemes:Bearer:ValidAudiences").Get<string[]>();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            token.Payload["aud"] = audiences; // Assign list of audiences to the 'aud' claim

            var bearer = new JwtSecurityTokenHandler().WriteToken(token);

            return bearer;
        }
    }
}
