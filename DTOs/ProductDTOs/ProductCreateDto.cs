namespace WebAPI.DTOs;

public class ProductCreateDto
{
    public string Name { get; set; } = null!;
    public string SKU { get; set; } = null!;
    public string? Barcode { get; set; }
    public int Quantity { get; set; }
    public int ReorderThreshold { get; set; }
    public Guid CategoryId { get; set; } = Guid.NewGuid();
    public Guid SupplierId { get; set; } = Guid.NewGuid();
}
