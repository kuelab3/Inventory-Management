namespace InventoryManagementSystem.Models;

public sealed record StockTransaction
{
    public int Id { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public string Type { get; init; } = string.Empty;
    public string PerformedBy { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
