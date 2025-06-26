namespace WebAPI.DTOs;

public class SaleItemResponseDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SaleId { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; } = Guid.NewGuid();
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? ProductName { get; set; } = null!;
}
