using System.Collections.Concurrent;

namespace InventoryManagementSystem.Services;

public sealed class AuditLogService : IAuditLogService
{
    private readonly ConcurrentQueue<string> _entries = new();

    public Task AddAsync(string entry)
    {
        _entries.Enqueue(entry);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<string>> GetEntriesAsync()
    {
        IReadOnlyList<string> snapshot = _entries.Reverse().ToList();
        return Task.FromResult(snapshot);
    }
}
