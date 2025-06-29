using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;

public class SalesItemService : ISalesItemService
{
    private readonly AppDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public SalesItemService(AppDbContext context, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
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
    public async Task<SaleItemResponseDto> CreateAsync(SaleItemCreateDto dto)
    {
        var salesItem = new SaleItem
        {
            Id = dto.Id,
            SaleId = dto.SaleId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
        };
        _context.SaleItems.Add(salesItem);
        await _context.SaveChangesAsync();

        // Reload the sales item with Product included
        var salesItemWithProduct = await _context.SaleItems
            .Include(si => si.Product)
            .FirstOrDefaultAsync(si => si.Id == salesItem.Id);
        if (salesItemWithProduct == null)
        {
            throw new Exception("Sales item not found after creation.");
        }
        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Create",
            tableAffected: "SaleItem",
            recordId: salesItemWithProduct.Id,
            ipAddress: GetIpAddress()
        );
        return new SaleItemResponseDto
        {
            Id = salesItemWithProduct.Id,
            SaleId = salesItemWithProduct.SaleId,
            ProductId = salesItemWithProduct.ProductId,
            Quantity = salesItemWithProduct.Quantity,
            UnitPrice = salesItemWithProduct.UnitPrice,
            ProductName = salesItemWithProduct.Product?.Name ?? string.Empty
        };
    }
    public async Task<IEnumerable<SaleItemResponseDto>> GetAllAsync()
    {
        return await _context.SaleItems
            .Include(si => si.Product)
            .Select(si => new SaleItemResponseDto
            {
                Id = si.Id,
                SaleId = si.SaleId,
                ProductId = si.ProductId,
                Quantity = si.Quantity,
                UnitPrice = si.UnitPrice,
                ProductName = si.Product != null ? si.Product.Name : string.Empty
            })
            .ToListAsync();
    }
    public async Task<SaleItemResponseDto?> GetByIdAsync(Guid id)
    {
        var salesItem = await _context.SaleItems
            .Include(si => si.Product)
            .FirstOrDefaultAsync(si => si.Id == id);

        if (salesItem == null) return null;

        return new SaleItemResponseDto
        {
            Id = salesItem.Id,
            SaleId = salesItem.SaleId,
            ProductId = salesItem.ProductId,
            Quantity = salesItem.Quantity,
            UnitPrice = salesItem.UnitPrice,
            ProductName = salesItem.Product?.Name ?? string.Empty
        };
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        var salesItem = await _context.SaleItems.FindAsync(id);
        if (salesItem == null) return false;

        _context.SaleItems.Remove(salesItem);
        await _context.SaveChangesAsync();
        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Delete",
            tableAffected: "SaleItem",
            recordId: salesItem.Id,
            ipAddress: GetIpAddress()
        );
        return true;
    }
    public async Task<SaleItemResponseDto?> UpdateAsync(Guid id, SaleItemUpdateDto dto)
    {
        var salesItem = await _context.SaleItems.FindAsync(id);
        if (salesItem == null) return null;

        salesItem.SaleId = dto.SaleId;
        salesItem.ProductId = dto.ProductId;
        salesItem.Quantity = dto.Quantity;
        salesItem.UnitPrice = dto.UnitPrice;

        await _context.SaveChangesAsync();

        // Reload the sales item with Product included
        var updatedSalesItem = await _context.SaleItems
            .Include(si => si.Product)
            .FirstOrDefaultAsync(si => si.Id == salesItem.Id);

        if (updatedSalesItem == null)
        {
            throw new Exception("Sales item not found after update.");
        }
        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Update",
            tableAffected: "SaleItem",
            recordId: updatedSalesItem.Id,
            ipAddress: GetIpAddress()
        );

        return new SaleItemResponseDto
        {
            Id = updatedSalesItem.Id,
            SaleId = updatedSalesItem.SaleId,
            ProductId = updatedSalesItem.ProductId,
            Quantity = updatedSalesItem.Quantity,
            UnitPrice = updatedSalesItem.UnitPrice,
            ProductName = updatedSalesItem.Product?.Name ?? string.Empty
        };
    }
}
