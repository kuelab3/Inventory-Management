using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services;

public interface IInventoryService
{
    Task<DashboardViewModel> GetDashboardAsync();
    Task ReceiveProductAsync(int productId, int quantity, string receivedBy, CancellationToken cancellationToken = default);
    Task RemoveProductAsync(int productId, int quantity, string removedBy, CancellationToken cancellationToken = default);
    Task ScanLowStockAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetProductsAsync();
    Task<IReadOnlyList<Supplier>> GetSuppliersAsync();
    Task<IReadOnlyList<StockTransaction>> GetTransactionsAsync();
    Task<IReadOnlyList<SupplierOrder>> GetOrdersAsync();
}
