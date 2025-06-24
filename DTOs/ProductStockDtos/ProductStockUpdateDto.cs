public class ProductStockUpdateDto
{
    public Guid ProductId { get; set; } = Guid.NewGuid();
    public Guid WarehouseId { get; set; } = Guid.NewGuid();
    public int Quantity { get; set; }
}