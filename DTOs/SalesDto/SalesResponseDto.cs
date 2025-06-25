namespace WebAPI.DTOs;

public class SalesResponseDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SoldBy { get; set; } = Guid.NewGuid();
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public string? Username { get; set; } = null!;
    public string? Email { get; set; } = null!;
}
