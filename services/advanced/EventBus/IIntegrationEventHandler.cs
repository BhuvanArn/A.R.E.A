namespace EventBus;

public interface IIntegrationEventHandler<TEvent, TResponse>
{
    Task<TResponse> HandleAsync(TEvent @event);
}