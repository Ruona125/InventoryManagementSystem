namespace WebApi.DTOs;

public class SaleCreateDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SoldBy { get; set; } = Guid.NewGuid();
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
}
