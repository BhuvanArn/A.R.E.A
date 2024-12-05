using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Database.Entities;
using Database.Service;
using EventBus;
using EventBus.Event;
using Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace LoginService;

public class UserCreatedEventHandler : IIntegrationEventHandler<UserCreatedEvent, (string, ResultType)>
{
    private readonly IConfiguration _configuration;
    private readonly UserService _userService;

    public UserCreatedEventHandler(IConfiguration configuration, UserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }
    
    public async Task<(string, ResultType)> HandleAsync(UserCreatedEvent @event)
    {
        User? user = (await _userService.FindUsersAsync(s => s.Email == @event.Email)).FirstOrDefault();

        if (user == null)
        {
            return ("Invalid credentials", ResultType.Fail);
        }

        bool validPassword = @event.Password.VerifyPassword(user.Salt, user.Password);

        if (!validPassword)
        {
            return ("Invalid credentials", ResultType.Fail);
        }
        
        return (GenerateJwtToken(user), ResultType.Success);
    }
    
    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["TokenLifetimeMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}