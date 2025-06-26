namespace WebAPI.DTOs;

public class PurchaseOrderCreateDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SupplierId { get; set; } = Guid.NewGuid();
    public Guid OrderedBy { get; set; } = Guid.NewGuid();
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending";
}
