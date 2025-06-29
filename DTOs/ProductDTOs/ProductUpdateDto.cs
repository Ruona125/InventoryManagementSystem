namespace WebAPI.DTOs;

public class ProductUpdateDto
{
    public string? Name { get; set; }
    public string? SKU { get; set; }
    public string? Barcode { get; set; }
    public int? Quantity { get; set; }
    public int? ReorderThreshold { get; set; }
    public Guid CategoryId { get; set; } = Guid.NewGuid();
    public Guid SupplierId { get; set; } = Guid.NewGuid();
}

