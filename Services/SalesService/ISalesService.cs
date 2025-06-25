using WebAPI.DTOs;
public interface ISalesService
{
    Task<IEnumerable<SalesResponseDto>> GetAllAsync();
    Task<SalesResponseDto?> GetByIdAsync(Guid id);
    Task<SalesResponseDto> CreateAsync(SalesCreateDto dto);
    Task<SalesResponseDto?> UpdateAsync(Guid id, SalesUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}