using WebAPI.DTOs;
public interface IPurchaseOrderItemService
{
    Task<PurchaseOrderItemResponseDto> CreateAsync(PurchaseOrderItemCreateDto dto);
    Task<IEnumerable<PurchaseOrderItemResponseDto>> GetAllAsync();
    Task<PurchaseOrderItemResponseDto?> GetByIdAsync(Guid id);
    Task<PurchaseOrderItemResponseDto?> UpdateAsync(Guid id, PurchaseOrderItemUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}