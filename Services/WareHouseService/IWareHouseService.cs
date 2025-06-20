using WebAPI.DTOs;
public interface IWareHouseService
{
    Task<IEnumerable<WareHouseResponseDto>> GetAllAsync();
    Task<WareHouseResponseDto?> GetByIdAsync(Guid id);
    Task<WareHouseResponseDto> CreateAsync(WareHouseCreateDto dto);
    Task<WareHouseResponseDto?> UpdateAsync(Guid id, WareHouseUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}