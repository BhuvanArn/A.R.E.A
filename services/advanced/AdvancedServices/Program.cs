using Database.Service;
using EventBus;
using EventBus.Event;
using LoginService;

namespace AdvancedServices;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        builder.Services.AddLogging(configure => configure.AddConsole());
        builder.Services.AddSingleton<IEventBus, EventBus.EventBus>();
        builder.Services.AddTransient<IIntegrationEventHandler<UserCreatedEvent, string>, UserCreatedEventHandler>();
        builder.Services.AddScoped<UserService>();
        
        var app = builder.Build();

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        var eventBus = app.Services.GetRequiredService<IEventBus>();
        var userCreatedHandler = app.Services.GetRequiredService<IIntegrationEventHandler<UserCreatedEvent, string>>();
        eventBus.Subscribe(userCreatedHandler);

        await app.RunAsync();
    }
}