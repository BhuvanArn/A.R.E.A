namespace EventBus;

public interface IIntegrationEventHandler<TEvent>
{
    Task HandleAsync(TEvent @event);
}