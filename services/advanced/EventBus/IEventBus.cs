namespace EventBus;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event);
    void Subscribe<TEvent>(IIntegrationEventHandler<TEvent> handler);
    void Unsubscribe<TEvent>(IIntegrationEventHandler<TEvent> handler);
}