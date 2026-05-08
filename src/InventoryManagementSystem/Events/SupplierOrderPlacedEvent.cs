using InventoryManagementSystem.EventBus;

namespace InventoryManagementSystem.Events;

public sealed record SupplierOrderPlacedEvent(
    int OrderId,
    int SupplierId,
    string SupplierName,
    int ProductId,
    string ProductName,
    int QuantityOrdered,
    string Reason,
    DateTime OccurredAt) : IEvent;
