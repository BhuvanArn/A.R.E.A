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

        if (user.Password == null || user.Salt == null)
        {
            return ("Invalid credentials", ResultType.Fail);
        }

        bool validPassword = @event.Password.VerifyPassword(user.Salt, user.Password);

        if (!validPassword)
        {
            return ("Invalid credentials", ResultType.Fail);
        }
        
        return (user.GenerateJwtToken(_configuration), ResultType.Success);
    }
}