namespace InventoryManagementSystem.Models;

public sealed class SupplierOrder
{
    public int Id { get; set; }
    public int SupplierId { get; init; }
    public string SupplierName { get; init; } = string.Empty;
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int QuantityOrdered { get; init; }
    public string Reason { get; init; } = string.Empty;
    public string Status { get; set; } = "Placed";
    public DateTime CreatedAt { get; init; }
}
