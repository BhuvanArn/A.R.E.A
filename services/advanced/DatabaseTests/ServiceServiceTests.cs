using System;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class ServiceDatabaseHandlerTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;
    private readonly IDatabaseHandler _databaseHandler;

    public ServiceDatabaseHandlerTests()
    {
        var services = new ServiceCollection();

        services.AddDbContextFactory<DatabaseContext>(options =>
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        );

        services.AddScoped<IDatabaseHandler, DatabaseHandler>();

        _serviceProvider = services.BuildServiceProvider();

        _contextFactory = _serviceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        _databaseHandler = _serviceProvider.GetRequiredService<IDatabaseHandler>();
    }

    [Fact]
    public async Task CreateServiceAsync_WithValidService_AddsServiceToDatabase()
    {
        var service = new Service
        {
            Name = "TestService",
            Auth = "{\"client_id\": \"testClient\", \"client_secret\": \"testSecret\"}"
        };

        await _databaseHandler.AddAsync(service);

        using var context = _contextFactory.CreateDbContext();
        var createdService = await context.Services.FirstOrDefaultAsync(s => s.Name == "TestService");

        Assert.NotNull(createdService);
        Assert.Equal("TestService", createdService.Name);
        Assert.Equal("{\"client_id\": \"testClient\", \"client_secret\": \"testSecret\"}", createdService.Auth);
    }

    [Fact]
    public async Task GetServiceByIdAsync_WithExistingService_ReturnsService()
    {
        var service = new Service
        {
            Name = "TestServiceById",
            Auth = "{\"key\": \"value\"}"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Services.Add(service);
            await context.SaveChangesAsync();
        }

        var retrievedService = (await _databaseHandler.GetAsync<Service>(s => s.Id == service.Id)).FirstOrDefault();

        Assert.NotNull(retrievedService);
        Assert.Equal(service.Id, retrievedService.Id);
        Assert.Equal("TestServiceById", retrievedService.Name);
        Assert.Equal("{\"key\": \"value\"}", retrievedService.Auth);
    }

    [Fact]
    public async Task GetAllServicesAsync_ReturnsAllServices()
    {
        var service1 = new Service
        {
            Name = "Service1",
            Auth = "{\"key1\": \"value1\"}"
        };
        var service2 = new Service
        {
            Name = "Service2",
            Auth = "{\"key2\": \"value2\"}"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Services.AddRange(service1, service2);
            await context.SaveChangesAsync();
        }

        var services = await _databaseHandler.GetAllAsync<Service>();

        Assert.NotNull(services);
        Assert.Equal(2, services.Count());
        Assert.Contains(services, s => s.Name == "Service1" && s.Auth == "{\"key1\": \"value1\"}");
        Assert.Contains(services, s => s.Name == "Service2" && s.Auth == "{\"key2\": \"value2\"}");
    }

    [Fact]
    public async Task UpdateServiceAsync_WithExistingService_UpdatesServiceInDatabase()
    {
        var service = new Service
        {
            Name = "UpdateService",
            Auth = "{\"key\": \"value\"}"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Services.Add(service);
            await context.SaveChangesAsync();
        }

        service.Name = "UpdatedService";
        service.Auth = "{\"key\": \"newValue\"}";

        await _databaseHandler.UpdateAsync(service);

        using (var context = _contextFactory.CreateDbContext())
        {
            var updatedService = await context.Services.FindAsync(service.Id);

            Assert.NotNull(updatedService);
            Assert.Equal("UpdatedService", updatedService.Name);
            Assert.Equal("{\"key\": \"newValue\"}", updatedService.Auth);
        }
    }

    [Fact]
    public async Task DeleteServiceAsync_WithExistingService_DeletesServiceFromDatabase()
    {
        var service = new Service
        {
            Name = "DeleteService",
            Auth = "{\"key\": \"value\"}"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Services.Add(service);
            await context.SaveChangesAsync();
        }

        await _databaseHandler.DeleteAsync(service);

        using (var context = _contextFactory.CreateDbContext())
        {
            var deletedService = await context.Services.FindAsync(service.Id);
            Assert.Null(deletedService);
        }
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }
}
