using System.Collections.Concurrent;

namespace EventBus;

public class EventBus : IEventBus
{
    private readonly ConcurrentDictionary<Type, List<object>> _handlers = new();

    public async Task PublishAsync<TEvent>(TEvent @event)
    {
        if (_handlers.TryGetValue(typeof(TEvent), out var handlers))
        {
            var tasks = new List<Task>();

            var handlersCopy = handlers.ToArray();

            foreach (var handlerObj in handlersCopy)
            {
                if (handlerObj is IIntegrationEventHandler<TEvent> handler)
                {
                    tasks.Add(handler.HandleAsync(@event));
                }
            }

            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
            }
        }
    }
    
    public void Subscribe<TEvent>(IIntegrationEventHandler<TEvent> handler)
    {
        var handlers = _handlers.GetOrAdd(typeof(TEvent), _ => new());
        
        lock (handlers)
        {
            handlers.Add(handler);
        }
    }
    
    public void Unsubscribe<TEvent>(IIntegrationEventHandler<TEvent> handler)
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