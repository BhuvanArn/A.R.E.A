using EventBus;
using EventBus.Event;
using Microsoft.Extensions.Logging;

namespace AdvancedServices.EventHandler;

public class UserCreatedEventHandler : IIntegrationEventHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> _logger;

    public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger)
    {
        _logger = logger;
    }
    
    public Task HandleAsync(UserCreatedEvent @event)
    {
        _logger.LogInformation(@event.Username);
        return Task.CompletedTask;
    }
}