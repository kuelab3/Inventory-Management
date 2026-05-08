namespace InventoryManagementSystem.EventBus;

public interface IEvent
{
    DateTime OccurredAt { get; }
}
