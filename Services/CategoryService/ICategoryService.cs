using WebAPI.DTOs;
public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
    Task<CategoryResponseDto?> GetByIdAsync(Guid id);
    Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto);
    Task<CategoryResponseDto?> UpdateAsync(Guid id, CategoryUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}