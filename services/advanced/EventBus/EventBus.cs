using System.Collections.Concurrent;

namespace EventBus;

public class EventBus : IEventBus
{
    private readonly ConcurrentDictionary<Type, List<object>> _handlers = new();

    public async Task<TResponse?> PublishAsync<TEvent, TResponse>(TEvent @event)
    {
        var responses = new List<TResponse>();

        if (!_handlers.TryGetValue(typeof(TEvent), out var handlers))
        {
            return responses.FirstOrDefault();
        }
        
        var tasks = new List<Task<TResponse>>();

        var handlersCopy = handlers.ToArray();

        foreach (var handlerObj in handlersCopy)
        {
            if (handlerObj is IIntegrationEventHandler<TEvent, TResponse> handler)
            {
                tasks.Add(handler.HandleAsync(@event));
            }
        }

        if (tasks.Count > 0)
        {
            responses = (await Task.WhenAll(tasks)).ToList();
        }

        return responses.FirstOrDefault();
    }

    public void Subscribe<TEvent, TResponse>(IIntegrationEventHandler<TEvent, TResponse> handler)
    {
        var handlers = _handlers.GetOrAdd(typeof(TEvent), _ => new());

        lock (handlers)
        {
            handlers.Add(handler);
        }
    }

    public void Unsubscribe<TEvent, TResponse>(IIntegrationEventHandler<TEvent, TResponse> handler)
    {
        if (!_handlers.TryGetValue(typeof(TEvent), out var handlers))
        {
            return;
        }

        lock (handlers)
        {
            handlers.Remove(handler);
        }
    }
}