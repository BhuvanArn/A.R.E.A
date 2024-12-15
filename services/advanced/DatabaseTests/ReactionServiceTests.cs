using System;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class ReactionDatabaseHandlerTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDatabaseHandler _databaseHandler;
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;

    public ReactionDatabaseHandlerTests()
    {
        var services = new ServiceCollection();

        // Use an InMemory database for testing
        services.AddDbContextFactory<DatabaseContext>(options =>
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        );

        // Register your DatabaseHandler as IDatabaseHandler
        services.AddScoped<IDatabaseHandler, DatabaseHandler>();

        _serviceProvider = services.BuildServiceProvider();

        // Resolve the services we need
        _contextFactory = _serviceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        _databaseHandler = _serviceProvider.GetRequiredService<IDatabaseHandler>();
    }

    [Fact]
    public async Task CreateReactionAsync_WithValidReaction_AddsReactionToDatabase()
    {
        var reaction = new Reaction
        {
            Name = "TestReaction",
            ServiceId = Guid.NewGuid(),
            ExecutionConfig = "{\"key\": \"value\"}"
        };

        await _databaseHandler.AddAsync(reaction);

        using var context = _contextFactory.CreateDbContext();
        var createdReaction = await context.Reactions.FirstOrDefaultAsync(r => r.Name == "TestReaction");

        Assert.NotNull(createdReaction);
        Assert.Equal("TestReaction", createdReaction.Name);
        Assert.Equal("{\"key\": \"value\"}", createdReaction.ExecutionConfig);
    }

    [Fact]
    public async Task GetReactionByIdAsync_WithExistingReaction_ReturnsReaction()
    {
        var reaction = new Reaction
        {
            Name = "TestReactionById",
            ServiceId = Guid.NewGuid(),
            ExecutionConfig = "{\"key\": \"value\"}"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Reactions.Add(reaction);
            await context.SaveChangesAsync();
        }

        var retrievedReaction = (await _databaseHandler.GetAsync<Reaction>(s => s.Id == reaction.Id)).FirstOrDefault();

        Assert.NotNull(retrievedReaction);
        Assert.Equal(reaction.Id, retrievedReaction.Id);
        Assert.Equal("TestReactionById", retrievedReaction.Name);
        Assert.Equal("{\"key\": \"value\"}", retrievedReaction.ExecutionConfig);
    }

    [Fact]
    public async Task GetAllReactionsAsync_ReturnsAllReactions()
    {
        var reaction1 = new Reaction
        {
            Name = "Reaction1",
            ServiceId = Guid.NewGuid(),
            ExecutionConfig = "{\"key1\": \"value1\"}"
        };
        var reaction2 = new Reaction
        {
            Name = "Reaction2",
            ServiceId = Guid.NewGuid(),
            ExecutionConfig = "{\"key2\": \"value2\"}"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Reactions.AddRange(reaction1, reaction2);
            await context.SaveChangesAsync();
        }

        var reactions = await _databaseHandler.GetAllAsync<Reaction>();

        Assert.NotNull(reactions);
        Assert.Equal(2, reactions.Count());
        Assert.Contains(reactions, r => r.Name == "Reaction1" && r.ExecutionConfig == "{\"key1\": \"value1\"}");
        Assert.Contains(reactions, r => r.Name == "Reaction2" && r.ExecutionConfig == "{\"key2\": \"value2\"}");
    }

    [Fact]
    public async Task UpdateReactionAsync_WithExistingReaction_UpdatesReactionInDatabase()
    {
        var reaction = new Reaction
        {
            Name = "UpdateReaction",
            ServiceId = Guid.NewGuid(),
            ExecutionConfig = "{\"key\": \"value\"}"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Reactions.Add(reaction);
            await context.SaveChangesAsync();
        }

        reaction.Name = "UpdatedReaction";
        reaction.ExecutionConfig = "{\"key\": \"newValue\"}";

        await _databaseHandler.UpdateAsync(reaction);

        using (var context = _contextFactory.CreateDbContext())
        {
            var updatedReaction = await context.Reactions.FindAsync(reaction.Id);
            Assert.NotNull(updatedReaction);
            Assert.Equal("UpdatedReaction", updatedReaction.Name);
            Assert.Equal("{\"key\": \"newValue\"}", updatedReaction.ExecutionConfig);
        }
    }

    [Fact]
    public async Task DeleteReactionAsync_WithExistingReaction_DeletesReactionFromDatabase()
    {
        var reaction = new Reaction
        {
            Name = "DeleteReaction",
            ServiceId = Guid.NewGuid(),
            ExecutionConfig = "{\"key\": \"value\"}"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Reactions.Add(reaction);
            await context.SaveChangesAsync();
        }

        await _databaseHandler.DeleteAsync(reaction);

        using (var context = _contextFactory.CreateDbContext())
        {
            var deletedReaction = await context.Reactions.FindAsync(reaction.Id);
            Assert.Null(deletedReaction);
        }
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }
}
