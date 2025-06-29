using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;

public class WareHouseService : IWareHouseService
{
    private readonly AppDbContext _db;
    private readonly IAuditLogService _auditLogService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WareHouseService(AppDbContext db, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _auditLogService = auditLogService;
        _httpContextAccessor = httpContextAccessor;
    }
    private Guid GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var guid) ? guid : Guid.Empty;
    }
    private string? GetIpAddress()
    {
        return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
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
            Name = dto.Name,
            Location = dto.Location
        };

        _db.Warehouses.Add(wareHouse);
        await _db.SaveChangesAsync();
        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Create",
            tableAffected: "Warehouse",
            recordId: wareHouse.Id,
            ipAddress: GetIpAddress()
        );

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
        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Update",
            tableAffected: "Warehouse",
            recordId: wareHouse.Id,
            ipAddress: GetIpAddress()
        );

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
        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Delete",
            tableAffected: "Warehouse",
            recordId: wareHouse.Id,
            ipAddress: GetIpAddress()
        );
        return true;
    }
}
