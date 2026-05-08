namespace InventoryManagementSystem.Models;

public sealed class DashboardViewModel
{
    public IReadOnlyList<Product> Products { get; init; } = Array.Empty<Product>();
    public IReadOnlyList<Supplier> Suppliers { get; init; } = Array.Empty<Supplier>();
    public IReadOnlyList<StockTransaction> RecentTransactions { get; init; } = Array.Empty<StockTransaction>();
    public IReadOnlyList<SupplierOrder> RecentOrders { get; init; } = Array.Empty<SupplierOrder>();
    public IReadOnlyList<string> AuditEntries { get; init; } = Array.Empty<string>();
    public IReadOnlyList<string> NotificationEntries { get; init; } = Array.Empty<string>();
    public IReadOnlyList<Product> LowStockProducts { get; init; } = Array.Empty<Product>();
}
