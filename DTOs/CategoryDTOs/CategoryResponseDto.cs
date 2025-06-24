namespace WebAPI.DTOs;

public class CategoryResponseDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;

}
