namespace InventoryManagementSystem.Services;

public interface IAuditLogService
{
    Task AddAsync(string entry);
    Task<IReadOnlyList<string>> GetEntriesAsync();
}
