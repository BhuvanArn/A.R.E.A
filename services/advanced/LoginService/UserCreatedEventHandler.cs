using Database.Migrations;
using Database.Service;
using EventBus;
using EventBus.Event;
using Microsoft.Extensions.Logging;

namespace LoginService;

public class UserCreatedEventHandler : IIntegrationEventHandler<UserCreatedEvent, string>
{
    private readonly ILogger<UserCreatedEventHandler> _logger;
    private readonly UserService _userService;

    public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }
    
    public Task<string> HandleAsync(UserCreatedEvent @event)
    {
        _logger.LogInformation(@event.Username);
        
        // Todo : database insert
        
        return Task.FromResult("...");
    }
}