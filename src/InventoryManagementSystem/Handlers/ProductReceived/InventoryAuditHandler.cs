using InventoryManagementSystem.EventBus;
using InventoryManagementSystem.Events;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.Handlers.ProductReceived;

public sealed class InventoryAuditHandler : IEventHandler<ProductReceivedEvent>
{
    private readonly IAuditLogService _auditLogService;

    public InventoryAuditHandler(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    public async Task HandleAsync(ProductReceivedEvent @event, CancellationToken cancellationToken = default)
    {
        var entry = $"[AUDIT] {DateTime.UtcNow:O} - Received {@event.QuantityReceived} of {@event.ProductName} (Product #{@event.ProductId}). New stock: {@event.NewStockLevel}.";
        await _auditLogService.AddAsync(entry);
    }
}
