using InventoryManagementSystem.EventBus;
using InventoryManagementSystem.Events;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services;

public sealed class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _repository;
    private readonly IEventBus _eventBus;
    private readonly IAuditLogService _auditLogService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<InventoryService> _logger;

    public InventoryService(
        IInventoryRepository repository,
        IEventBus eventBus,
        IAuditLogService auditLogService,
        INotificationService notificationService,
        ILogger<InventoryService> logger)
    {
        _repository = repository;
        _eventBus = eventBus;
        _auditLogService = auditLogService;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var products = await _repository.GetProductsAsync();
        var suppliers = await _repository.GetSuppliersAsync();
        var transactions = await _repository.GetTransactionsAsync();
        var orders = await _repository.GetOrdersAsync();

        return new DashboardViewModel
        {
            Products = products,
            Suppliers = suppliers,
            RecentTransactions = transactions.Take(8).ToList(),
            RecentOrders = orders.Take(8).ToList(),
            AuditEntries = await _auditLogService.GetEntriesAsync(),
            NotificationEntries = await _notificationService.GetEntriesAsync(),
            LowStockProducts = products.Where(p => p.StockLevel <= p.ReorderThreshold).OrderBy(p => p.StockLevel).ToList()
        };
    }

    public async Task ReceiveProductAsync(int productId, int quantity, string receivedBy, CancellationToken cancellationToken = default)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        var product = await _repository.GetProductByIdAsync(productId);
        if (product is null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        product.StockLevel += quantity;
        await _repository.UpdateProductAsync(product);

        await _repository.AddTransactionAsync(new StockTransaction
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Quantity = quantity,
            Type = "Received",
            PerformedBy = receivedBy,
            CreatedAt = DateTime.UtcNow
        });

        var receivedEvent = new ProductReceivedEvent(
            product.Id,
            product.Name,
            quantity,
            product.StockLevel,
            receivedBy,
            DateTime.UtcNow);

        _logger.LogInformation("Publishing ProductReceivedEvent for product {ProductId} ({ProductName})", product.Id, product.Name);
        await _eventBus.PublishAsync(receivedEvent, cancellationToken);
    }

    public async Task RemoveProductAsync(int productId, int quantity, string removedBy, CancellationToken cancellationToken = default)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        var product = await _repository.GetProductByIdAsync(productId);
        if (product is null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        if (product.StockLevel < quantity)
        {
            throw new InvalidOperationException($"Insufficient stock. Available: {product.StockLevel}, Requested: {quantity}");
        }

        product.StockLevel -= quantity;
        await _repository.UpdateProductAsync(product);

        await _repository.AddTransactionAsync(new StockTransaction
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Quantity = quantity,
            Type = "Removed",
            PerformedBy = removedBy,
            CreatedAt = DateTime.UtcNow
        });

        _logger.LogInformation("Product removed: {ProductId} ({ProductName}) - Quantity: {Quantity}, New stock level: {NewStockLevel}", product.Id, product.Name, quantity, product.StockLevel);
    }

    public async Task ScanLowStockAsync(CancellationToken cancellationToken = default)
    {
        var products = await _repository.GetProductsAsync();
        var lowStockProducts = products.Where(p => p.StockLevel <= p.ReorderThreshold).ToList();

        _logger.LogInformation("Scanning for low stock products. Found {LowStockCount} low-stock items.", lowStockProducts.Count);

        foreach (var product in lowStockProducts)
        {
            var lowStockEvent = new StockLevelLowEvent(
                product.Id,
                product.Name,
                product.StockLevel,
                product.ReorderThreshold,
                DateTime.UtcNow);

            await _eventBus.PublishAsync(lowStockEvent, cancellationToken);
        }
    }

    public Task<IReadOnlyList<Product>> GetProductsAsync() => _repository.GetProductsAsync();
    public Task<IReadOnlyList<Supplier>> GetSuppliersAsync() => _repository.GetSuppliersAsync();
    public Task<IReadOnlyList<StockTransaction>> GetTransactionsAsync() => _repository.GetTransactionsAsync();
    public Task<IReadOnlyList<SupplierOrder>> GetOrdersAsync() => _repository.GetOrdersAsync();
}
