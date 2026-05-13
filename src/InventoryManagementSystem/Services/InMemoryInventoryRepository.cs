using System.Collections.Concurrent;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services;

public sealed class InMemoryInventoryRepository : IInventoryRepository
{
    private readonly List<Product> _products =
    [
        new Product { Id = 1, Name = "Office Chair", Sku = "CHA-1001", SupplierId = 1, StockLevel = 12, ReorderThreshold = 8, UnitPrice = 1450 },
        new Product { Id = 2, Name = "Printer Paper (A4)", Sku = "PPR-2001", SupplierId = 2, StockLevel = 5, ReorderThreshold = 15, UnitPrice = 320 },
        new Product { Id = 3, Name = "Laptop Stand", Sku = "LST-3001", SupplierId = 1, StockLevel = 3, ReorderThreshold = 6, UnitPrice = 890 },
        new Product { Id = 4, Name = "Wireless Mouse", Sku = "MSE-4001", SupplierId = 3, StockLevel = 20, ReorderThreshold = 10, UnitPrice = 650 }
    ];

    private readonly List<Supplier> _suppliers =
    [
        new Supplier { Id = 1, Name = "Addis Office Supply", Email = "orders@addisoffice.example", Phone = "+251-111-111-111" },
        new Supplier { Id = 2, Name = "Blue Nile Paper", Email = "sales@bluenilepaper.example", Phone = "+251-222-222-222" },
        new Supplier { Id = 3, Name = "TekTech Importers", Email = "hello@tektech.example", Phone = "+251-333-333-333" }
    ];

    private readonly List<StockTransaction> _transactions = [];
    private readonly List<SupplierOrder> _orders = [];
    private int _transactionId = 1;
    private int _orderId = 1;

    public Task<IReadOnlyList<Product>> GetProductsAsync() =>
        Task.FromResult((IReadOnlyList<Product>)_products.OrderBy(p => p.Id).ToList());

    public Task<IReadOnlyList<Supplier>> GetSuppliersAsync() =>
        Task.FromResult((IReadOnlyList<Supplier>)_suppliers.OrderBy(s => s.Id).ToList());

    public Task<IReadOnlyList<StockTransaction>> GetTransactionsAsync() =>
        Task.FromResult((IReadOnlyList<StockTransaction>)_transactions.OrderByDescending(t => t.CreatedAt).ToList());

    public Task<IReadOnlyList<SupplierOrder>> GetOrdersAsync() =>
        Task.FromResult((IReadOnlyList<SupplierOrder>)_orders.OrderByDescending(o => o.CreatedAt).ToList());

    public Task<Product?> GetProductByIdAsync(int productId) =>
        Task.FromResult(_products.FirstOrDefault(p => p.Id == productId));

    public Task<Supplier?> GetSupplierByIdAsync(int supplierId) =>
        Task.FromResult(_suppliers.FirstOrDefault(s => s.Id == supplierId));

    public Task AddTransactionAsync(StockTransaction transaction)
    {
        _transactions.Add(transaction with { Id = _transactionId++ });
        return Task.CompletedTask;
    }

    public Task AddOrderAsync(SupplierOrder order)
    {
        order.Id = _orderId++;
        _orders.Add(order);
        return Task.CompletedTask;
    }

    public Task UpdateProductAsync(Product product)
    {
        var current = _products.First(p => p.Id == product.Id);
        current.StockLevel = product.StockLevel;
        return Task.CompletedTask;
    }

    public Task<SupplierOrder?> GetPendingOrderByProductIdAsync(int productId)
    {
        var pendingOrder = _orders.FirstOrDefault(o => o.ProductId == productId && o.Status == "Placed");
        return Task.FromResult<SupplierOrder?>(pendingOrder);
    }
}
