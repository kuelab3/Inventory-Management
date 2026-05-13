using System.Text.Json;
using System.Text.Json.Serialization;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services;

public sealed class FileInventoryRepository : IInventoryRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private RepositoryState _state;
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public FileInventoryRepository(string filePath)
    {
        _filePath = filePath;
        _state = LoadState();
    }

    public Task<IReadOnlyList<Product>> GetProductsAsync() => Task.FromResult((IReadOnlyList<Product>)_state.Products.OrderBy(p => p.Id).Select(CloneProduct).ToList());

    public Task<IReadOnlyList<Supplier>> GetSuppliersAsync() => Task.FromResult((IReadOnlyList<Supplier>)_state.Suppliers.OrderBy(s => s.Id).Select(CloneSupplier).ToList());

    public Task<IReadOnlyList<StockTransaction>> GetTransactionsAsync() => Task.FromResult((IReadOnlyList<StockTransaction>)_state.Transactions.OrderByDescending(t => t.CreatedAt).Select(CloneTransaction).ToList());

    public Task<IReadOnlyList<SupplierOrder>> GetOrdersAsync() => Task.FromResult((IReadOnlyList<SupplierOrder>)_state.Orders.OrderByDescending(o => o.CreatedAt).Select(CloneOrder).ToList());

    public Task<Product?> GetProductByIdAsync(int productId) => Task.FromResult(_state.Products.FirstOrDefault(p => p.Id == productId) is { } product ? CloneProduct(product) : null);

    public Task<Supplier?> GetSupplierByIdAsync(int supplierId) => Task.FromResult(_state.Suppliers.FirstOrDefault(s => s.Id == supplierId) is { } supplier ? CloneSupplier(supplier) : null);

    public async Task AddTransactionAsync(StockTransaction transaction)
    {
        await _lock.WaitAsync();
        try
        {
            var nextId = _state.TransactionId++;
            _state.Transactions.Add(transaction with { Id = nextId });
            await PersistStateAsync();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task AddOrderAsync(SupplierOrder order)
    {
        await _lock.WaitAsync();
        try
        {
            var nextId = _state.OrderId++;
            order.Id = nextId;
            _state.Orders.Add(CloneOrder(order));
            await PersistStateAsync();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task UpdateProductAsync(Product product)
    {
        await _lock.WaitAsync();
        try
        {
            var current = _state.Products.FirstOrDefault(p => p.Id == product.Id);
            if (current is null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            current.StockLevel = product.StockLevel;
            await PersistStateAsync();
        }
        finally
        {
            _lock.Release();
        }
    }

    public Task<SupplierOrder?> GetPendingOrderByProductIdAsync(int productId)
    {
        var pendingOrder = _state.Orders.FirstOrDefault(o => o.ProductId == productId && o.Status == "Placed");
        return Task.FromResult(pendingOrder is null ? null : CloneOrder(pendingOrder));
    }

    private RepositoryState LoadState()
    {
        var folder = Path.GetDirectoryName(_filePath);
        if (folder is not null)
        {
            Directory.CreateDirectory(folder);
        }

        if (!File.Exists(_filePath))
        {
            var initial = new RepositoryState(
                Products: new List<Product>
                {
                    new Product { Id = 1, Name = "Office Chair", Sku = "CHA-1001", SupplierId = 1, StockLevel = 12, ReorderThreshold = 8, UnitPrice = 1450 },
                    new Product { Id = 2, Name = "Printer Paper (A4)", Sku = "PPR-2001", SupplierId = 2, StockLevel = 5, ReorderThreshold = 15, UnitPrice = 320 },
                    new Product { Id = 3, Name = "Laptop Stand", Sku = "LST-3001", SupplierId = 1, StockLevel = 3, ReorderThreshold = 6, UnitPrice = 890 },
                    new Product { Id = 4, Name = "Wireless Mouse", Sku = "MSE-4001", SupplierId = 3, StockLevel = 20, ReorderThreshold = 10, UnitPrice = 650 }
                },
                Suppliers: new List<Supplier>
                {
                    new Supplier { Id = 1, Name = "Addis Office Supply", Email = "orders@addisoffice.example", Phone = "+251-111-111-111" },
                    new Supplier { Id = 2, Name = "Blue Nile Paper", Email = "sales@bluenilepaper.example", Phone = "+251-222-222-222" },
                    new Supplier { Id = 3, Name = "TekTech Importers", Email = "hello@tektech.example", Phone = "+251-333-333-333" }
                },
                Transactions: new List<StockTransaction>(),
                Orders: new List<SupplierOrder>());

            SaveState(initial);
            return initial;
        }

        var json = File.ReadAllText(_filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new RepositoryState(new List<Product>(), new List<Supplier>(), new List<StockTransaction>(), new List<SupplierOrder>());
        }

        return JsonSerializer.Deserialize<RepositoryState>(json, SerializerOptions) ?? new RepositoryState(new List<Product>(), new List<Supplier>(), new List<StockTransaction>(), new List<SupplierOrder>());
    }

    private void SaveState(RepositoryState state)
    {
        var json = JsonSerializer.Serialize(state, SerializerOptions);
        File.WriteAllText(_filePath, json);
    }

    private async Task PersistStateAsync()
    {
        var json = JsonSerializer.Serialize(_state, SerializerOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }

    private static Product CloneProduct(Product source) => new()
    {
        Id = source.Id,
        Name = source.Name,
        Sku = source.Sku,
        SupplierId = source.SupplierId,
        StockLevel = source.StockLevel,
        ReorderThreshold = source.ReorderThreshold,
        UnitPrice = source.UnitPrice
    };

    private static Supplier CloneSupplier(Supplier source) => new()
    {
        Id = source.Id,
        Name = source.Name,
        Email = source.Email,
        Phone = source.Phone
    };

    private static StockTransaction CloneTransaction(StockTransaction source) => new()
    {
        Id = source.Id,
        ProductId = source.ProductId,
        ProductName = source.ProductName,
        Quantity = source.Quantity,
        Type = source.Type,
        PerformedBy = source.PerformedBy,
        CreatedAt = source.CreatedAt
    };

    private static SupplierOrder CloneOrder(SupplierOrder source) => new()
    {
        Id = source.Id,
        SupplierId = source.SupplierId,
        SupplierName = source.SupplierName,
        ProductId = source.ProductId,
        ProductName = source.ProductName,
        QuantityOrdered = source.QuantityOrdered,
        Reason = source.Reason,
        Status = source.Status,
        CreatedAt = source.CreatedAt
    };

    private sealed record RepositoryState(
        List<Product> Products,
        List<Supplier> Suppliers,
        List<StockTransaction> Transactions,
        List<SupplierOrder> Orders)
    {
        public int TransactionId { get; set; } = 1;
        public int OrderId { get; set; } = 1;
    }
}
