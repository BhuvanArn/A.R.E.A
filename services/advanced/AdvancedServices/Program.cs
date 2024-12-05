using Database;
using Database.Dao;
using Database.Service;
using EventBus;
using EventBus.Event;
using LoginService;
using Microsoft.EntityFrameworkCore;

namespace AdvancedServices;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var configuration = builder.Configuration;
        
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddControllers();
        builder.Services.AddLogging(configure => configure.AddConsole());
        builder.Services.AddSingleton<IEventBus, EventBus.EventBus>();
        builder.Services.AddTransient<IIntegrationEventHandler<UserCreatedEvent, (string, ResultType)>, UserCreatedEventHandler>();
        
        builder.Services.AddScoped<DaoFactory>();
        builder.Services.AddScoped<UserService>();
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
        
        var app = builder.Build();

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        var eventBus = app.Services.GetRequiredService<IEventBus>();
        var userCreatedHandler = app.Services.GetRequiredService<IIntegrationEventHandler<UserCreatedEvent, (string, ResultType)>>();
        eventBus.Subscribe(userCreatedHandler);

        await app.RunAsync();
    }
}