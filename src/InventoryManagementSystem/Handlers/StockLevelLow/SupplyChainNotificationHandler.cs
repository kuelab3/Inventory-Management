using InventoryManagementSystem.EventBus;
using InventoryManagementSystem.Events;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.Handlers.StockLevelLow;

public sealed class SupplyChainNotificationHandler : IEventHandler<StockLevelLowEvent>
{
    private readonly INotificationService _notificationService;
    private readonly IAuditLogService _auditLogService;

    public SupplyChainNotificationHandler(INotificationService notificationService, IAuditLogService auditLogService)
    {
        _notificationService = notificationService;
        _auditLogService = auditLogService;
    }

    public async Task HandleAsync(StockLevelLowEvent @event, CancellationToken cancellationToken = default)
    {
        var message = $"[LOW STOCK] {@event.ProductName} has {@event.CurrentStock} units remaining. Threshold: {@event.Threshold}.";
        await _notificationService.AddAsync(message);
        await _auditLogService.AddAsync($"[AUDIT] {DateTime.UtcNow:O} - Low stock detected for {@event.ProductName}.");
    }
}
