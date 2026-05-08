using InventoryManagementSystem.EventBus;
using InventoryManagementSystem.Events;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.Handlers.ProductReceived;

public sealed class ReceptionNotificationHandler : IEventHandler<ProductReceivedEvent>
{
    private readonly INotificationService _notificationService;

    public ReceptionNotificationHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task HandleAsync(ProductReceivedEvent @event, CancellationToken cancellationToken = default)
    {
        var message = $"[NOTIFICATION] Stock received for {@event.ProductName}: +{@event.QuantityReceived}. Current stock: {@event.NewStockLevel}. Logged by {@event.ReceivedBy}.";
        await _notificationService.AddAsync(message);
    }
}
