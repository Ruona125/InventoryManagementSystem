namespace WebAPI.DTOs;

public class WareHouseCreateDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;

}