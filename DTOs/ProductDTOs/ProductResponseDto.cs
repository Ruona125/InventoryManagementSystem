namespace WebAPI.DTOs;

public class ProductResponseDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string SKU { get; set; } = null!;
    public string? Barcode { get; set; }
    public int Quantity { get; set; }
    public int ReorderThreshold { get; set; }
    public string? CategoryName { get; set; }
    public string? SupplierName { get; set; }
    public DateTime CreatedAt { get; set; }
}
