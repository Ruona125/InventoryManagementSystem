public class ProductStock
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; } = Guid.NewGuid();
    public Guid WarehouseId { get; set; } = Guid.NewGuid();
    public int Quantity { get; set; }
    public Product? Product { get; set; }
    public Warehouse? Warehouse { get; set; }
}
