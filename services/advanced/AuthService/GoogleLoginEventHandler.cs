using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace AuthService;

public class GoogleLoginEventHandler : IIntegrationEventHandler<GoogleLoginEvent, (string, ResultType)>
{
    private readonly IConfiguration _configuration;
    private readonly IDatabaseHandler _dbHandler;

    public GoogleLoginEventHandler(IConfiguration configuration, IDatabaseHandler dbHandler)
    {
        _configuration = configuration;
        _dbHandler = dbHandler;
    }
    
    public async Task<(string, ResultType)> HandleAsync(GoogleLoginEvent @event)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(@event.Token);

        string token = string.Empty;
        
        User? foundUser = (await _dbHandler.GetAsync<User>(s => s.Username == payload.Email)).FirstOrDefault();
        
        if (foundUser == null)
        {
            User user = new()
            {
                Email = payload.Email,
                Username = payload.Email.Split('@')[0]
            };
            
            token = user.GenerateJwtToken(_configuration);
        }
        else
        {
            token = foundUser.GenerateJwtToken(_configuration);
        }
        
        return (token, ResultType.Success);
    }
}