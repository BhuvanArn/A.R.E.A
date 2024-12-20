using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthService;

public class UserCreatedEventHandler : IIntegrationEventHandler<UserCreatedEvent, (string, ResultType)>
{
    private readonly IConfiguration _configuration;
    private readonly IDatabaseHandler _dbHandler;

    public UserCreatedEventHandler(IConfiguration configuration, IDatabaseHandler dbHandler)
    {
        _configuration = configuration;
        _dbHandler = dbHandler;
    }
    
    public async Task<(string, ResultType)> HandleAsync(UserCreatedEvent @event)
    {
        User? user = (await _dbHandler.GetAsync<User>(s => s.Email == @event.Email)).FirstOrDefault();

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
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
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