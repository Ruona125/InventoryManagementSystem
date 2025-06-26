using WebAPI.DTOs;
public interface ISalesItemService
{
    Task<SaleItemResponseDto> CreateAsync(SaleItemCreateDto dto);
    Task<IEnumerable<SaleItemResponseDto>> GetAllAsync();
    Task<SaleItemResponseDto?> GetByIdAsync(Guid id);
    Task<SaleItemResponseDto?> UpdateAsync(Guid id, SaleItemUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}