using WebAPI.DTOs;
public interface IPurchaseOrderService
{
    Task<PurchaseOrderResponseDto> CreateAsync(PurchaseOrderCreateDto dto);
    Task<IEnumerable<PurchaseOrderResponseDto>> GetAllAsync();
    Task<PurchaseOrderResponseDto?> GetByIdAsync(Guid id);
    Task<PurchaseOrderResponseDto?> UpdateAsync(Guid id, PurchaseOrderUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}