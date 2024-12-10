using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Dao;
using Database.Entities;
using Database.Service;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DatabaseTests
{
    public class ReactionServiceTests : IDisposable
    {
        private readonly DatabaseContext _dbContext;
        private readonly DaoFactory _daoFactory;
        private readonly ReactionService _reactionService;

        public ReactionServiceTests()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new DatabaseContext(options);
            _daoFactory = new DaoFactory(_dbContext);
            _reactionService = new ReactionService(_daoFactory);
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

            await _reactionService.CreateReactionAsync(reaction);
            var createdReaction = await _dbContext.Reactions.FirstOrDefaultAsync(r => r.Name == "TestReaction");

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
            _dbContext.Reactions.Add(reaction);
            await _dbContext.SaveChangesAsync();

            var retrievedReaction = await _reactionService.GetReactionByIdAsync(reaction.Id);

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
            _dbContext.Reactions.AddRange(reaction1, reaction2);
            await _dbContext.SaveChangesAsync();

            var reactions = await _reactionService.GetAllReactionsAsync();

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
            _dbContext.Reactions.Add(reaction);
            await _dbContext.SaveChangesAsync();

            reaction.Name = "UpdatedReaction";
            reaction.ExecutionConfig = "{\"key\": \"newValue\"}";

            await _reactionService.UpdateReactionAsync(reaction);

            var updatedReaction = await _dbContext.Reactions.FindAsync(reaction.Id);

            Assert.NotNull(updatedReaction);
            Assert.Equal("UpdatedReaction", updatedReaction.Name);
            Assert.Equal("{\"key\": \"newValue\"}", updatedReaction.ExecutionConfig);
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
            _dbContext.Reactions.Add(reaction);
            await _dbContext.SaveChangesAsync();

            await _reactionService.DeleteReactionAsync(reaction.Id);

            var deletedReaction = await _dbContext.Reactions.FindAsync(reaction.Id);
            Assert.Null(deletedReaction);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
