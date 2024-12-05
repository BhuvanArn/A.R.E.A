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
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        builder.Services.AddLogging(configure => configure.AddConsole());
        builder.Services.AddSingleton<IEventBus, EventBus.EventBus>();
        builder.Services.AddTransient<IIntegrationEventHandler<UserCreatedEvent>, UserCreatedEventHandler>();
        
        var app = builder.Build();

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        var eventBus = app.Services.GetRequiredService<IEventBus>();
        var userCreatedHandler = app.Services.GetRequiredService<IIntegrationEventHandler<UserCreatedEvent>>();
        eventBus.Subscribe(userCreatedHandler);

        await app.RunAsync();
    }
}