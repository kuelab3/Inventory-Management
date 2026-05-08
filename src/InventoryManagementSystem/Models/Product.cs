namespace InventoryManagementSystem.Models;

public sealed class Product
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Sku { get; init; } = string.Empty;
    public int SupplierId { get; init; }
    public int StockLevel { get; set; }
    public int ReorderThreshold { get; init; }
    public decimal UnitPrice { get; init; }
}
