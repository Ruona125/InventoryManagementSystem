using WebAPI.DTOs;
public interface IProductStockService
{
    Task<ProductStockResponseDto> CreateAsync(ProductStockCreateDto dto);
    Task<IEnumerable<ProductStockResponseDto>> GetAllAsync();
    Task<ProductStockResponseDto?> GetByIdAsync(Guid id);
    Task<ProductStockResponseDto?> UpdateAsync(Guid id, ProductStockUpdateDto dto);
}
