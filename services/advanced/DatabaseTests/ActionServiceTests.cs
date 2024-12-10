using Database;
using Database.Dao;
using Database.Service;
using Microsoft.EntityFrameworkCore;
using Action = Database.Entities.Action;

namespace DatabaseTests
{
    public class ActionServiceTests : IDisposable
    {
        private readonly DatabaseContext _dbContext;
        private readonly DaoFactory _daoFactory;
        private readonly ActionService _actionService;

        public ActionServiceTests()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new DatabaseContext(options);
            _daoFactory = new DaoFactory(_dbContext);
            _actionService = new ActionService(_daoFactory);
        }

        [Fact]
        public async Task CreateActionAsync_WithValidAction_AddsActionToDatabase()
        {
            var action = new Action
            {
                Name = "TestAction",
                ServiceId = Guid.NewGuid(),
                TriggerConfig = "{\"key\": \"value\"}"
            };

            await _actionService.CreateActionAsync(action);
            var createdAction = await _dbContext.Actions.FirstOrDefaultAsync(a => a.Name == "TestAction");

            Assert.NotNull(createdAction);
            Assert.Equal("TestAction", createdAction.Name);
            Assert.Equal("{\"key\": \"value\"}", createdAction.TriggerConfig);
        }

        [Fact]
        public async Task GetActionByIdAsync_WithExistingAction_ReturnsAction()
        {
            var action = new Action
            {
                Name = "TestActionById",
                ServiceId = Guid.NewGuid(),
                TriggerConfig = "{\"key\": \"value\"}"
            };
            _dbContext.Actions.Add(action);
            await _dbContext.SaveChangesAsync();

            var retrievedAction = await _actionService.GetActionByIdAsync(action.Id);

            Assert.NotNull(retrievedAction);
            Assert.Equal(action.Id, retrievedAction.Id);
            Assert.Equal("TestActionById", retrievedAction.Name);
            Assert.Equal("{\"key\": \"value\"}", retrievedAction.TriggerConfig);
        }

        [Fact]
        public async Task GetAllActionsAsync_ReturnsAllActions()
        {
            var action1 = new Action
            {
                Name = "Action1",
                ServiceId = Guid.NewGuid(),
                TriggerConfig = "{\"key1\": \"value1\"}"
            };
            var action2 = new Action
            {
                Name = "Action2",
                ServiceId = Guid.NewGuid(),
                TriggerConfig = "{\"key2\": \"value2\"}"
            };
            _dbContext.Actions.AddRange(action1, action2);
            await _dbContext.SaveChangesAsync();

            var actions = await _actionService.GetAllActionsAsync();

            Assert.NotNull(actions);
            Assert.Equal(2, actions.Count());
            Assert.Contains(actions, a => a.Name == "Action1" && a.TriggerConfig == "{\"key1\": \"value1\"}");
            Assert.Contains(actions, a => a.Name == "Action2" && a.TriggerConfig == "{\"key2\": \"value2\"}");
        }

        [Fact]
        public async Task UpdateActionAsync_WithExistingAction_UpdatesActionInDatabase()
        {
            var action = new Action
            {
                Name = "UpdateAction",
                ServiceId = Guid.NewGuid(),
                TriggerConfig = "{\"key\": \"value\"}"
            };
            _dbContext.Actions.Add(action);
            await _dbContext.SaveChangesAsync();

            action.Name = "UpdatedAction";
            action.TriggerConfig = "{\"key\": \"newValue\"}";

            await _actionService.UpdateActionAsync(action);

            var updatedAction = await _dbContext.Actions.FindAsync(action.Id);

            Assert.NotNull(updatedAction);
            Assert.Equal("UpdatedAction", updatedAction.Name);
            Assert.Equal("{\"key\": \"newValue\"}", updatedAction.TriggerConfig);
        }

        [Fact]
        public async Task DeleteActionAsync_WithExistingAction_DeletesActionFromDatabase()
        {
            var action = new Action
            {
                Name = "DeleteAction",
                ServiceId = Guid.NewGuid(),
                TriggerConfig = "{\"key\": \"value\"}"
            };
            _dbContext.Actions.Add(action);
            await _dbContext.SaveChangesAsync();

            await _actionService.DeleteActionAsync(action.Id);

            var deletedAction = await _dbContext.Actions.FindAsync(action.Id);
            Assert.Null(deletedAction);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
