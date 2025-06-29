using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;

public class SalesService : ISalesService
{
    private readonly AppDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public SalesService(AppDbContext context, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
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
    public async Task<IEnumerable<SalesResponseDto>> GetAllAsync()
    {
        return await _context.Sales
            .Include(s => s.User)
            .Select(s => new SalesResponseDto
            {
                Id = s.Id,
                SoldBy = s.SoldBy,
                Username = s.User != null ? s.User.Username : string.Empty,
                TotalAmount = s.TotalAmount,
                SaleDate = s.SaleDate,
                Email = s.User != null ? s.User.Email : string.Empty
            })
            .ToListAsync();
    }
    public async Task<SalesResponseDto?> GetByIdAsync(Guid id)
    {
        var sale = await _context.Sales
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sale == null) return null;

        return new SalesResponseDto
        {
            Id = sale.Id,
            SoldBy = sale.SoldBy,
            Username = sale.User != null ? sale.User.Username : string.Empty,
            TotalAmount = sale.TotalAmount,
            SaleDate = sale.SaleDate,
            Email = sale.User != null ? sale.User.Email : string.Empty
        };
    }

    public async Task<SalesResponseDto> CreateAsync(SalesCreateDto dto)
    {
        var sale = new Sale
        {
            Id = dto.Id,
            SoldBy = dto.SoldBy,
            TotalAmount = dto.TotalAmount,
            SaleDate = dto.SaleDate
        };

        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();


        // Reload the sale with User included
        var saleWithUser = await _context.Sales
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == sale.Id);

        if (saleWithUser == null)
        {
            throw new InvalidOperationException("Sale could not be loaded after creation.");
        }
        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Create",
            tableAffected: "Sale",
            recordId: saleWithUser.Id,
            ipAddress: GetIpAddress()
        );

        return new SalesResponseDto
        {
            Id = saleWithUser.Id,
            SoldBy = saleWithUser.SoldBy,
            TotalAmount = saleWithUser.TotalAmount,
            SaleDate = saleWithUser.SaleDate,
            Username = saleWithUser.User?.Username ?? string.Empty,
            Email = saleWithUser.User?.Email ?? string.Empty
        };
    }

    public async Task<SalesResponseDto?> UpdateAsync(Guid id, SalesUpdateDto dto)
    {
        var sale = await _context.Sales.FindAsync(id);
        if (sale == null) return null;

        sale.SoldBy = dto.SoldBy;
        sale.TotalAmount = dto.TotalAmount;
        sale.SaleDate = dto.SaleDate;

        await _context.SaveChangesAsync();

        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Update",
            tableAffected: "Sale",
            recordId: sale.Id,
            ipAddress: GetIpAddress()
        );

        return new SalesResponseDto
        {
            Id = sale.Id,
            SoldBy = sale.SoldBy,
            TotalAmount = sale.TotalAmount,
            SaleDate = sale.SaleDate
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sale = await _context.Sales.FindAsync(id);
        if (sale == null) return false;

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync();

        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Delete",
            tableAffected: "Sale",
            recordId: sale.Id,
            ipAddress: GetIpAddress()
        );

        return true;
    }
}
