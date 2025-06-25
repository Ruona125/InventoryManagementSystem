namespace WebAPI.DTOs;

public class SalesCreateDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SoldBy { get; set; } = Guid.NewGuid();
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
}
