using WebAPI.DTOs;
public class ProductStockResponseDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; } = Guid.NewGuid();
    public Guid WarehouseId { get; set; } = Guid.NewGuid();
    public int Quantity { get; set; }
    public string? WarehouseName { get; set; } = null!;
    public string? ProductName { get; set; } = null!;
}
