using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagementSystem.EventBus;

public sealed class EventBus : IEventBus
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventBus> _logger;

    public EventBus(IServiceProvider serviceProvider, ILogger<EventBus> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>().ToList();

        if (handlers.Count == 0)
        {
            _logger.LogWarning("No handlers registered for event type {EventType}", typeof(TEvent).Name);
            return;
        }

        var tasks = handlers.Select(handler => handler.HandleAsync(@event, cancellationToken));
        await Task.WhenAll(tasks);
    }
}
