using System.Reflection;
using System.Text;
using ActionReactionService;
using ActionReactionService.AboutParser;
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
        builder.Services.AddHttpClient("ServiceAbout", client =>
        {
            client.BaseAddress = new Uri("http://service-about:80");
        });
        builder.Services.AddLogging(configure => configure.AddConsole());
        builder.Services.AddSingleton<IEventBus, EventBus.EventBus>();
        builder.Services.Scan(scan =>
            scan.FromAssemblies(
                    typeof(ActionReactionEventHandler).Assembly,
                    typeof(ForgotPasswordEventHandler).Assembly
                )
                .AddClasses(classes => classes.AssignableTo(typeof(IIntegrationEventHandler<,>)))
                .AsImplementedInterfaces()
                .AsSelf()
                .WithTransientLifetime()
        );
        builder.Services.AddScoped<IDatabaseHandler, DatabaseHandler>();
        builder.Services.AddSingleton<IAboutParserService, AboutParserService>();
        
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
            
            var aboutParserService = services.GetRequiredService<IAboutParserService>();
            await aboutParserService.ParseAndStoreAboutJsonAsync();
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
        var assembliesToScan = new[]
        {
            typeof(ActionReactionEventHandler).Assembly,
            typeof(ForgotPasswordEventHandler).Assembly
        };
        
        var handlerTypes = assembliesToScan
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType &&
                          i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<,>)))
            .ToList();
        
        foreach (var handlerType in handlerTypes)
        {
            var handlerInstance = app.Services.GetRequiredService(handlerType);

            var interfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<,>))
                .ToList();

            foreach (var iface in interfaces)
            {
                var genericArgs = iface.GetGenericArguments();
                var eventType = genericArgs[0];
                var resultType = genericArgs[1];

                var subscribeMethod = typeof(IEventBus).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(m =>
                        m.Name == "Subscribe"
                        && m.IsGenericMethod
                        && m.GetGenericArguments().Length == 2
                    );

                if (subscribeMethod == null)
                    continue;

                var genericSubscribeMethod = subscribeMethod.MakeGenericMethod(eventType, resultType);

                genericSubscribeMethod.Invoke(eventBus, new[] { handlerInstance });
            }
        }

        await app.RunAsync();
    }
}
