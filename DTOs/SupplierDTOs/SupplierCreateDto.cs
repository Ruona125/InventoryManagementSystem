namespace WebAPI.DTOs;

public class SupplierCreateDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string ContactEmail { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Address { get; set; } = null!;
}