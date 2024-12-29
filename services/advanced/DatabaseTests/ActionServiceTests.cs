using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class ActionDatabaseHandlerTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDatabaseHandler _databaseHandler;
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;

    public ActionDatabaseHandlerTests()
    {
        var services = new ServiceCollection();

        services.AddDbContextFactory<DatabaseContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString())
        );

        services.AddScoped<IDatabaseHandler, DatabaseHandler>();

        _serviceProvider = services.BuildServiceProvider();

        _contextFactory = _serviceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        _databaseHandler = _serviceProvider.GetRequiredService<IDatabaseHandler>();
    }

    [Fact]
    public async Task CreateActionAsync_WithValidAction_AddsActionToDatabase()
    {
        var action = new Database.Entities.Action
        {
            Name = "TestAction",
            ServiceId = Guid.NewGuid(),
            TriggerConfig = "{\"key\": \"value\"}"
        };

        await _databaseHandler.AddAsync(action);

        using var context = _contextFactory.CreateDbContext();
        var createdAction = await context.Actions.FirstOrDefaultAsync(a => a.Name == "TestAction");

        Assert.NotNull(createdAction);
        Assert.Equal("TestAction", createdAction.Name);
        Assert.Equal("{\"key\": \"value\"}", createdAction.TriggerConfig);
    }

    [Fact]
    public async Task GetActionByIdAsync_WithExistingAction_ReturnsAction()
    {
        var action = new Database.Entities.Action
        {
            Name = "TestActionById",
            ServiceId = Guid.NewGuid(),
            TriggerConfig = "{\"key\": \"value\"}"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Actions.Add(action);
            await context.SaveChangesAsync();
        }

        var retrievedAction = (await _databaseHandler.GetAsync<Database.Entities.Action>(s => s.Id == action.Id)).FirstOrDefault();

        Assert.NotNull(retrievedAction);
        Assert.Equal(action.Id, retrievedAction.Id);
        Assert.Equal("TestActionById", retrievedAction.Name);
        Assert.Equal("{\"key\": \"value\"}", retrievedAction.TriggerConfig);
    }

    [Fact]
    public async Task GetAllActionsAsync_ReturnsAllActions()
    {
        var action1 = new Database.Entities.Action
        {
            Name = "Action1",
            ServiceId = Guid.NewGuid(),
            TriggerConfig = "{\"key1\": \"value1\"}"
        };
        var action2 = new Database.Entities.Action
        {
            Name = "Action2",
            ServiceId = Guid.NewGuid(),
            TriggerConfig = "{\"key2\": \"value2\"}"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Actions.AddRange(action1, action2);
            await context.SaveChangesAsync();
        }

        var actions = await _databaseHandler.GetAllAsync<Database.Entities.Action>();

        Assert.NotNull(actions);
        Assert.Equal(2, actions.Count());
        Assert.Contains(actions, a => a.Name == "Action1" && a.TriggerConfig == "{\"key1\": \"value1\"}");
        Assert.Contains(actions, a => a.Name == "Action2" && a.TriggerConfig == "{\"key2\": \"value2\"}");
    }

    [Fact]
    public async Task UpdateActionAsync_WithExistingAction_UpdatesActionInDatabase()
    {
        var action = new Database.Entities.Action
        {
            Name = "UpdateAction",
            ServiceId = Guid.NewGuid(),
            TriggerConfig = "{\"key\": \"value\"}"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Actions.Add(action);
            await context.SaveChangesAsync();
        }

        action.Name = "UpdatedAction";
        action.TriggerConfig = "{\"key\": \"newValue\"}";

        await _databaseHandler.UpdateAsync(action);

        using (var context = _contextFactory.CreateDbContext())
        {
            var updatedAction = await context.Actions.FindAsync(action.Id);
            Assert.NotNull(updatedAction);
            Assert.Equal("UpdatedAction", updatedAction.Name);
            Assert.Equal("{\"key\": \"newValue\"}", updatedAction.TriggerConfig);
        }
    }

    [Fact]
    public async Task DeleteActionAsync_WithExistingAction_DeletesActionFromDatabase()
    {
        var action = new Database.Entities.Action
        {
            Name = "DeleteAction",
            ServiceId = Guid.NewGuid(),
            TriggerConfig = "{\"key\": \"value\"}"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Actions.Add(action);
            await context.SaveChangesAsync();
        }

        await _databaseHandler.DeleteAsync(action);

        using (var context = _contextFactory.CreateDbContext())
        {
            var deletedAction = await context.Actions.FindAsync(action.Id);
            Assert.Null(deletedAction);
        }
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }
}
