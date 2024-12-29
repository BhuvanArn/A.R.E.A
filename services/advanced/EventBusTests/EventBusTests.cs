using EventBus;
using EventBus.Event;

namespace EventBusTests
{
    public class EventBusTests
    {
        private readonly IEventBus _eventBus;

        public EventBusTests()
        {
            _eventBus = new EventBus.EventBus();
        }

        [Fact]
        public async Task PublishAsync_WithNoSubscribers_ReturnsEmptyResponseList()
        {
            var userCreatedEvent = new UserCreatedEvent
            {
                Email = "test@example.com",
                Password = "password123"
            };

            (string, ResultType)? response = await _eventBus.PublishAsync<UserCreatedEvent, (string, ResultType)>(userCreatedEvent);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task PublishAsync_WithSingleSubscriber_ReturnsSingleResponse()
        {
            var userCreatedEvent = new UserCreatedEvent
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var handler = new TestUserCreatedEventHandler();
            _eventBus.Subscribe(handler);

            (string, ResultType)? response = await _eventBus.PublishAsync<UserCreatedEvent, (string, ResultType)>(userCreatedEvent);

            Assert.NotNull(response);
            Assert.Equal(ResultType.Fail, response.Value.Item2);

            _eventBus.Unsubscribe(handler);
        }

        [Fact]
        public async Task Unsubscribe_RemovesHandler_PreventsHandlerFromBeingInvoked()
        {
            var userCreatedEvent = new UserCreatedEvent
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var handler = new TestUserCreatedEventHandler();
            _eventBus.Subscribe(handler);
            _eventBus.Unsubscribe(handler);

            (string, ResultType)? response = await _eventBus.PublishAsync<UserCreatedEvent, (string, ResultType)>(userCreatedEvent);

            Assert.NotNull(response);
        }
    }

    #region Test Handlers

    public class TestUserCreatedEventHandler : IIntegrationEventHandler<UserCreatedEvent, ResultType>
    {
        public Task<ResultType> HandleAsync(UserCreatedEvent @event)
        {
            return Task.FromResult(ResultType.Success);
        }
    }

    public class AnotherUserCreatedEventHandler : IIntegrationEventHandler<UserCreatedEvent, ResultType>
    {
        public Task<ResultType> HandleAsync(UserCreatedEvent @event)
        {
            return Task.FromResult(ResultType.Fail);
        }
    }

    #endregion
}
