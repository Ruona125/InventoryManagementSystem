using WebAPI.DTOs;
public interface ISupplierService
{
    Task<IEnumerable<SupplierResponseDto>> GetAllAsync();
    Task<SupplierResponseDto?> GetByIdAsync(Guid id);
    Task<SupplierResponseDto> CreateAsync(SupplierCreateDto dto);
    Task<SupplierResponseDto?> UpdateAsync(Guid id, SupplierUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}