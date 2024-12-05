using EventBus;
using EventBus.Event;

namespace AdvancedServices.EventHandler;

public class UserCreatedEventHandler : IIntegrationEventHandler<UserCreatedEvent>
{
    public Task HandleAsync(UserCreatedEvent @event)
    {
        Console.WriteLine(@event.Username);
        return Task.CompletedTask;
    }
}