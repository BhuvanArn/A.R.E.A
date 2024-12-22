namespace EventBus;

public interface IEventBus
{
    void Subscribe<TEvent, TResponse>(IIntegrationEventHandler<TEvent, TResponse> handler);
    Task<TResponse?> PublishAsync<TEvent, TResponse>(TEvent @event);
    void Unsubscribe<TEvent, TResponse>(IIntegrationEventHandler<TEvent, TResponse> handler);
}