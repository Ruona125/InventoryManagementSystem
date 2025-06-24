using WebAPI.DTOs;
public class ProductStockCreateDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; } = Guid.NewGuid();
    public Guid WarehouseId { get; set; } = Guid.NewGuid();
    public int Quantity { get; set; }
}
