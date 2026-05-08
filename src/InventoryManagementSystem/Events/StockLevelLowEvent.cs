using InventoryManagementSystem.EventBus;

namespace InventoryManagementSystem.Events;

public sealed record StockLevelLowEvent(
    int ProductId,
    string ProductName,
    int CurrentStock,
    int Threshold,
    DateTime OccurredAt) : IEvent;
