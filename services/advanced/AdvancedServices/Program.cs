using AdvancedServices.EventHandler;
using EventBus;
using EventBus.Event;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AdvancedServices;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddLogging(configure => configure.AddConsole());

        services.AddSingleton<IEventBus, EventBus.EventBus>();

        services.AddTransient<IIntegrationEventHandler<UserCreatedEvent>, UserCreatedEventHandler>();

        var serviceProvider = services.BuildServiceProvider();

        var eventBus = serviceProvider.GetRequiredService<IEventBus>();

        var userCreatedHandler = serviceProvider.GetRequiredService<IIntegrationEventHandler<UserCreatedEvent>>();
        eventBus.Subscribe(userCreatedHandler);

        await eventBus.PublishAsync(new UserCreatedEvent { Username = "test1" });
        await eventBus.PublishAsync(new UserCreatedEvent { Username = "test2" });
    }
}