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

            var responses = await _eventBus.PublishAsync<UserCreatedEvent, ResultType>(userCreatedEvent);

            Assert.NotNull(responses);
            Assert.Empty(responses);
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

            var responses = await _eventBus.PublishAsync<UserCreatedEvent, ResultType>(userCreatedEvent);

            Assert.NotNull(responses);
            Assert.Single(responses);
            Assert.Contains(ResultType.Success, responses);

            _eventBus.Unsubscribe(handler);
        }

        [Fact]
        public async Task PublishAsync_WithMultipleSubscribers_ReturnsResponsesFromAllHandlers()
        {
            var userCreatedEvent = new UserCreatedEvent
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var handler1 = new TestUserCreatedEventHandler();
            var handler2 = new AnotherUserCreatedEventHandler();
            _eventBus.Subscribe(handler1);
            _eventBus.Subscribe(handler2);

            var responses = await _eventBus.PublishAsync<UserCreatedEvent, ResultType>(userCreatedEvent);

            Assert.NotNull(responses);
            Assert.Equal(2, responses.Count());
            Assert.Contains(ResultType.Success, responses);
            Assert.Contains(ResultType.Fail, responses);

            _eventBus.Unsubscribe(handler1);
            _eventBus.Unsubscribe(handler2);
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

            var responses = await _eventBus.PublishAsync<UserCreatedEvent, ResultType>(userCreatedEvent);

            Assert.NotNull(responses);
            Assert.Empty(responses);
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
