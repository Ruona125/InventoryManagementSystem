public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string SKU { get; set; } = null!;
    public string? Barcode { get; set; }
    public int Quantity { get; set; } = 0;
    public int ReorderThreshold { get; set; } = 10;
    public Guid? CategoryId { get; set; } = Guid.NewGuid();
    public Guid? SupplierId { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Category? Category { get; set; }
    public Supplier? Supplier { get; set; }

    public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    public ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
}
