using InventoryManagementSystem.EventBus;
using InventoryManagementSystem.Events;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.Handlers.StockLevelLow;

public sealed class ReorderPlanningHandler : IEventHandler<StockLevelLowEvent>
{
    private readonly IInventoryRepository _repository;
    private readonly IEventBus _eventBus;

    public ReorderPlanningHandler(IInventoryRepository repository, IEventBus eventBus)
    {
        _repository = repository;
        _eventBus = eventBus;
    }

    public async Task HandleAsync(StockLevelLowEvent @event, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetProductByIdAsync(@event.ProductId);
        if (product is null)
        {
            return;
        }

        var supplier = await _repository.GetSupplierByIdAsync(product.SupplierId);
        if (supplier is null)
        {
            return;
        }

        var suggestedQuantity = Math.Max(product.ReorderThreshold * 2, 10);

        var order = new SupplierOrder
        {
            SupplierId = supplier.Id,
            SupplierName = supplier.Name,
            ProductId = product.Id,
            ProductName = product.Name,
            QuantityOrdered = suggestedQuantity,
            Reason = $"Auto reorder triggered because stock reached {@event.CurrentStock}",
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddOrderAsync(order);

        var orderEvent = new SupplierOrderPlacedEvent(
            order.Id == 0 ? 0 : order.Id,
            supplier.Id,
            supplier.Name,
            product.Id,
            product.Name,
            suggestedQuantity,
            order.Reason,
            DateTime.UtcNow);

        await _eventBus.PublishAsync(orderEvent, cancellationToken);
    }
}
