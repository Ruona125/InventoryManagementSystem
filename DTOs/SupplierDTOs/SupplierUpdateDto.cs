namespace WebAPI.DTOs;

public class SupplierUpdateDto
{
    public string Name { get; set; } = null!;
    public string ContactEmail { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Address { get; set; } = null!;
}