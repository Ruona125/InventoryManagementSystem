using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;

public class WareHouseService : IWareHouseService
{
    private readonly AppDbContext _db;

    public WareHouseService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<WareHouseResponseDto>> GetAllAsync()
    {
        return await _db.Warehouses
            .Select(wh => new WareHouseResponseDto
            {
                Id = wh.Id,
                Name = wh.Name,
                Location = wh.Location ?? string.Empty,

            })
            .ToListAsync();
    }

    public async Task<WareHouseResponseDto?> GetByIdAsync(Guid id)
    {
        var wareHouse = await _db.Warehouses.FindAsync(id);
        if (wareHouse is null) return null;

        return new WareHouseResponseDto
        {
            Id = wareHouse.Id,
            Name = wareHouse.Name,
            Location = wareHouse.Location ?? string.Empty,
        };
    }

    public async Task<WareHouseResponseDto> CreateAsync(WareHouseCreateDto dto)
    {
        var wareHouse = new Warehouse
        {
            Id = dto.Id,
            Name = dto.Name,
            Location = dto.Location
        };

        _db.Warehouses.Add(wareHouse);
        await _db.SaveChangesAsync();

        return new WareHouseResponseDto
        {
            Id = wareHouse.Id,
            Name = wareHouse.Name,
            Location = wareHouse.Location
        };
    }

    public async Task<WareHouseResponseDto?> UpdateAsync(Guid id, WareHouseUpdateDto dto)
    {
        var wareHouse = await _db.Warehouses.FindAsync(id);
        if (wareHouse is null) return null;

        wareHouse.Name = dto.Name;
        wareHouse.Location = dto.Location;

        _db.Warehouses.Update(wareHouse);
        await _db.SaveChangesAsync();

        return new WareHouseResponseDto
        {
            Id = wareHouse.Id,
            Name = wareHouse.Name,
            Location = wareHouse.Location ?? string.Empty,
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var wareHouse = await _db.Warehouses.FindAsync(id);
        if (wareHouse is null) return false;

        _db.Warehouses.Remove(wareHouse);
        await _db.SaveChangesAsync();
        return true;
    }
}
