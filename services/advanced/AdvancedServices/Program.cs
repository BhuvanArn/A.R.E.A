using System.Text;
using ActionReactionService;
using AuthService;
using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Action = Database.Entities.Action;

namespace AdvancedServices;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = builder.Configuration;

        builder.Services.AddDbContextFactory<DatabaseContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                };
            });

        builder.Services.AddControllers();
        builder.Services.AddLogging(configure => configure.AddConsole());
        builder.Services.AddSingleton<IEventBus, EventBus.EventBus>();
        builder.Services.AddTransient<IIntegrationEventHandler<UserCreatedEvent, (string, ResultType)>, UserCreatedEventHandler>();
        builder.Services.AddTransient<IIntegrationEventHandler<UserRegisteredEvent, (string, ResultType)>, UserRegisteredEventHandler>();
        builder.Services.AddTransient<IIntegrationEventHandler<UserResetPasswordEvent, (string, ResultType)>, UserResetPasswordEventHandler>();
        builder.Services.AddTransient<IIntegrationEventHandler<ActionReactionEvent, (List<Service>, ResultType)>, ActionReactionEventHandler>();
        builder.Services.AddTransient<IIntegrationEventHandler<GetServiceEvent, (List<Service>, ResultType)>, GetServicesEventHandler>();
        builder.Services.AddTransient<IIntegrationEventHandler<SubscribeServiceEvent, (List<Service>, ResultType)>, SubscribeServiceEventHandler>();
        builder.Services.AddTransient<IIntegrationEventHandler<GetActionsReactionsEvent, (GetActionsReactionsEventHandler.ActionsReactionsResponse, ResultType)>, GetActionsReactionsEventHandler>();
        builder.Services.AddTransient<IIntegrationEventHandler<GetActionEvent, (List<Action>, ResultType)>, GetActionEventHandler>();
        builder.Services.AddTransient<IIntegrationEventHandler<GetReactionEvent, (List<Reaction>, ResultType)>, GetReactionEventHandler>();
        builder.Services.AddTransient<IIntegrationEventHandler<GoogleLoginEvent, (string, ResultType)>, GoogleLoginEventHandler>();
        builder.Services.AddScoped<IDatabaseHandler, DatabaseHandler>();
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                corsBuilder =>
                {
                    corsBuilder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<DatabaseContext>();
            await context.Database.MigrateAsync();
        }

        app.UseMiddleware<ResponseBufferingMiddleware>();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        var eventBus = app.Services.GetRequiredService<IEventBus>();
        var userLoginHandler = app.Services.GetRequiredService<IIntegrationEventHandler<UserCreatedEvent, (string, ResultType)>>();
        eventBus.Subscribe(userLoginHandler);
        var userRegisteredHandler = app.Services.GetRequiredService<IIntegrationEventHandler<UserRegisteredEvent, (string, ResultType)>>();
        eventBus.Subscribe(userRegisteredHandler);
        var userResetPasswordHandler = app.Services.GetRequiredService<IIntegrationEventHandler<UserResetPasswordEvent, (string, ResultType)>>();
        eventBus.Subscribe(userResetPasswordHandler);
        var actionReactionHandler = app.Services.GetRequiredService<IIntegrationEventHandler<ActionReactionEvent, (List<Service>, ResultType)>>();
        eventBus.Subscribe(actionReactionHandler);
        var getServiceHandler = app.Services.GetRequiredService<IIntegrationEventHandler<GetServiceEvent, (List<Service>, ResultType)>>();
        eventBus.Subscribe(getServiceHandler);
        var subscribeServiceHandler = app.Services.GetRequiredService<IIntegrationEventHandler<SubscribeServiceEvent, (List<Service>, ResultType)>>();
        eventBus.Subscribe(subscribeServiceHandler);
        var getActionsReactionsHandler = app.Services.GetRequiredService<IIntegrationEventHandler<GetActionsReactionsEvent, (GetActionsReactionsEventHandler.ActionsReactionsResponse, ResultType)>>();
        eventBus.Subscribe(getActionsReactionsHandler);
        var getActionHandler = app.Services.GetRequiredService<IIntegrationEventHandler<GetActionEvent, (List<Action>, ResultType)>>();
        eventBus.Subscribe(getActionHandler);
        var getReactionHandler = app.Services.GetRequiredService<IIntegrationEventHandler<GetReactionEvent, (List<Reaction>, ResultType)>>();
        eventBus.Subscribe(getReactionHandler);
        var googleLoginEventHandler = app.Services.GetRequiredService<IIntegrationEventHandler<GoogleLoginEvent, (string, ResultType)>>();
        eventBus.Subscribe(googleLoginEventHandler);

        await app.RunAsync();
    }
}
