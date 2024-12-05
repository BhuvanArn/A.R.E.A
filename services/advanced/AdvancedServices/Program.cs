using System.Text;
using Database;
using Database.Dao;
using Database.Service;
using EventBus;
using EventBus.Event;
using LoginService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AdvancedServices;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var configuration = builder.Configuration;
        
        builder.Services.AddDbContext<DatabaseContext>(options =>
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
        
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<DatabaseContext>();

            await context.Database.MigrateAsync();
        }

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