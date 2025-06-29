using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;

public class ProductStockService : IProductStockService
{
    private readonly AppDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ProductStockService(AppDbContext context, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
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

    public async Task<IEnumerable<ProductStockResponseDto>> GetAllAsync()
    {
        return await _context.ProductStocks
            .Include(ps => ps.Product)
            .Include(ps => ps.Warehouse)
            .Select(ps => new ProductStockResponseDto
            {
                Id = ps.Id,
                ProductId = ps.ProductId,
                WarehouseId = ps.WarehouseId,
                Quantity = ps.Quantity,
                WarehouseName = ps.Warehouse != null ? ps.Warehouse.Name : string.Empty,
                ProductName = ps.Product != null ? ps.Product.Name : string.Empty
            })
            .ToListAsync();
    }

    public async Task<ProductStockResponseDto?> GetByIdAsync(Guid id)
    {
        var productStock = await _context.ProductStocks
            .Include(ps => ps.Product)
            .Include(ps => ps.Warehouse)
            .FirstOrDefaultAsync(ps => ps.Id == id);

        if (productStock == null) return null;

        return new ProductStockResponseDto
        {
            Id = productStock.Id,
            ProductId = productStock.ProductId,
            WarehouseId = productStock.WarehouseId,
            Quantity = productStock.Quantity,
            WarehouseName = productStock.Warehouse?.Name,
            ProductName = productStock.Product?.Name
        };
    }

    public async Task<ProductStockResponseDto> CreateAsync(ProductStockCreateDto dto)
    {
        var productStock = new ProductStock
        {
            Id = dto.Id,
            ProductId = dto.ProductId,
            WarehouseId = dto.WarehouseId,
            Quantity = dto.Quantity
        };

        _context.ProductStocks.Add(productStock);
        await _context.SaveChangesAsync();

        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Create",
            tableAffected: "ProductStock",
            recordId: productStock.Id,
            ipAddress: GetIpAddress()
        );

        return new ProductStockResponseDto
        {
            Id = productStock.Id,
            ProductId = productStock.ProductId,
            WarehouseId = productStock.WarehouseId,
            Quantity = productStock.Quantity,
            WarehouseName = (await _context.Warehouses.FindAsync(productStock.WarehouseId))?.Name ?? string.Empty,
            ProductName = (await _context.Products.FindAsync(productStock.ProductId))?.Name ?? string.Empty
        };
    }

    public async Task<ProductStockResponseDto?> UpdateAsync(Guid id, ProductStockUpdateDto dto)
    {
        var productStock = await _context.ProductStocks.FindAsync(id);
        if (productStock == null) return null;
        productStock.ProductId = dto.ProductId;
        productStock.WarehouseId = dto.WarehouseId;
        productStock.Quantity = dto.Quantity;
        _context.ProductStocks.Update(productStock);
        await _context.SaveChangesAsync();
        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Update",
            tableAffected: "ProductStock",
            recordId: productStock.Id,
            ipAddress: GetIpAddress()
        );
        return new ProductStockResponseDto
        {
            Id = productStock.Id,
            ProductId = productStock.ProductId,
            WarehouseId = productStock.WarehouseId,
            Quantity = productStock.Quantity,
            WarehouseName = (await _context.Warehouses.FindAsync(productStock.WarehouseId))?.Name ?? string.Empty,
            ProductName = (await _context.Products.FindAsync(productStock.ProductId))?.Name ?? string.Empty
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var productStock = await _context.ProductStocks.FindAsync(id);
        if (productStock == null) return false;

        _context.ProductStocks.Remove(productStock);
        await _context.SaveChangesAsync();
        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Delete",
            tableAffected: "ProductStock",
            recordId: productStock.Id,
            ipAddress: GetIpAddress()
        );
        return true;
    }
}
