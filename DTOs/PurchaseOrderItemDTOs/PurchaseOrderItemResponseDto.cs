namespace WebAPI.DTOs;

public class PurchaseOrderItemResponseDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PurchaseOrderId { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; } = Guid.NewGuid();
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    //     public string? Username { get; set; } = null!;
    public string? ProductName { get; set; } = null!;
}
