using Database.Entities;
using Database.Service;
using EventBus;
using EventBus.Event;
using Extension;
using Microsoft.Extensions.Logging;

namespace LoginService;

public class UserCreatedEventHandler : IIntegrationEventHandler<UserCreatedEvent, (string, ResultType)>
{
    private readonly ILogger<UserCreatedEventHandler> _logger;
    private readonly UserService _userService;

    public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger, UserService userService)
    {
        _logger = logger;
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
        
        return ("todo jwt", ResultType.Success);
    }
}