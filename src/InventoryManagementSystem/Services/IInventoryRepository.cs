using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services;

public interface IInventoryRepository
{
    Task<IReadOnlyList<Product>> GetProductsAsync();
    Task<IReadOnlyList<Supplier>> GetSuppliersAsync();
    Task<IReadOnlyList<StockTransaction>> GetTransactionsAsync();
    Task<IReadOnlyList<SupplierOrder>> GetOrdersAsync();
    Task<Product?> GetProductByIdAsync(int productId);
    Task<Supplier?> GetSupplierByIdAsync(int supplierId);
    Task AddTransactionAsync(StockTransaction transaction);
    Task AddOrderAsync(SupplierOrder order);
    Task UpdateProductAsync(Product product);
    Task<SupplierOrder?> GetPendingOrderByProductIdAsync(int productId);
}
