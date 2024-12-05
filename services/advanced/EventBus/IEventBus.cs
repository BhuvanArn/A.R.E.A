namespace EventBus;

public interface IEventBus
{
    void Subscribe<TEvent, TResponse>(IIntegrationEventHandler<TEvent, TResponse> handler);
    Task<IEnumerable<TResponse>> PublishAsync<TEvent, TResponse>(TEvent @event);
    void Unsubscribe<TEvent, TResponse>(IIntegrationEventHandler<TEvent, TResponse> handler);
}