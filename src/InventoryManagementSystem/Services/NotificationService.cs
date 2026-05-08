using System.Collections.Concurrent;

namespace InventoryManagementSystem.Services;

public sealed class NotificationService : INotificationService
{
    private readonly ConcurrentQueue<string> _entries = new();

    public Task AddAsync(string message)
    {
        _entries.Enqueue(message);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<string>> GetEntriesAsync()
    {
        IReadOnlyList<string> snapshot = _entries.Reverse().ToList();
        return Task.FromResult(snapshot);
    }
}
