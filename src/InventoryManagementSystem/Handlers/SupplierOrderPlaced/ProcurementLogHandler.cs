using InventoryManagementSystem.EventBus;
using InventoryManagementSystem.Events;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.Handlers.SupplierOrderPlaced;

public sealed class ProcurementLogHandler : IEventHandler<SupplierOrderPlacedEvent>
{
    private readonly IAuditLogService _auditLogService;

    public ProcurementLogHandler(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    public async Task HandleAsync(SupplierOrderPlacedEvent @event, CancellationToken cancellationToken = default)
    {
        var entry = $"[ORDER] {@event.QuantityOrdered} x {@event.ProductName} ordered from {@event.SupplierName}. Reason: {@event.Reason}.";
        await _auditLogService.AddAsync(entry);
    }
}
