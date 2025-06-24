namespace WebAPI.DTOs;

public class CategoryCreateDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;

}
