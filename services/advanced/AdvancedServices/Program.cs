using AdvancedServices.EventHandler;
using EventBus;
using EventBus.Event;

namespace AdvancedServices;

class Program
{
    static async Task Main(string[] args)
    {
        IEventBus eventBus = new EventBus.EventBus();

        var userCreatedHandler = new UserCreatedEventHandler();

        eventBus.Subscribe(userCreatedHandler);

        await eventBus.PublishAsync(new UserCreatedEvent { Username = "test1" });
        await eventBus.PublishAsync(new UserCreatedEvent { Username = "test2" });
    }
}