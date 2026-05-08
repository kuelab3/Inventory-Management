using InventoryManagementSystem.EventBus;
using InventoryManagementSystem.Events;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.Handlers.SupplierOrderPlaced;

public sealed class SupplierEmailHandler : IEventHandler<SupplierOrderPlacedEvent>
{
    private readonly INotificationService _notificationService;

    public SupplierEmailHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task HandleAsync(SupplierOrderPlacedEvent @event, CancellationToken cancellationToken = default)
    {
        var message = $"[SUPPLIER EMAIL] Sent order notice to {@event.SupplierName} for {@event.QuantityOrdered} units of {@event.ProductName}.";
        await _notificationService.AddAsync(message);
    }
}
