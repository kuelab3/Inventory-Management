namespace InventoryManagementSystem.Services;

public interface INotificationService
{
    Task AddAsync(string message);
    Task<IReadOnlyList<string>> GetEntriesAsync();
}
