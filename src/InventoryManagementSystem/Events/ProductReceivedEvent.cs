using InventoryManagementSystem.EventBus;

namespace InventoryManagementSystem.Events;

public sealed record ProductReceivedEvent(
    int ProductId,
    string ProductName,
    int QuantityReceived,
    int NewStockLevel,
    string ReceivedBy,
    DateTime OccurredAt) : IEvent;
