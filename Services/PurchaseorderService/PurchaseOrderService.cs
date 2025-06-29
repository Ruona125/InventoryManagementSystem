using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly AppDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public PurchaseOrderService(AppDbContext context, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
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
    public async Task<PurchaseOrderResponseDto> CreateAsync(PurchaseOrderCreateDto dto)
    {
        var purchaseOrder = new PurchaseOrder
        {
            SupplierId = dto.SupplierId,
            OrderDate = dto.OrderDate,
            OrderedBy = dto.OrderedBy
        };
        _context.PurchaseOrders.Add(purchaseOrder);
        await _context.SaveChangesAsync();
        // Reload the purchase order with Supplier and User included
        var purchaseOrderWithDetails = await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.User)
            .FirstOrDefaultAsync(po => po.Id == purchaseOrder.Id);

        if (purchaseOrderWithDetails == null)
        {
            throw new Exception("Purchase order not found after creation.");
        }

        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Create",
            tableAffected: "PurchaseOrder",
            recordId: purchaseOrderWithDetails.Id,
            ipAddress: GetIpAddress()
        );

        return new PurchaseOrderResponseDto
        {
            Id = purchaseOrderWithDetails.Id,
            SupplierId = purchaseOrderWithDetails.SupplierId,
            OrderedBy = purchaseOrderWithDetails.OrderedBy,
            OrderDate = purchaseOrderWithDetails.OrderDate,
            Status = purchaseOrderWithDetails.Status,
            Username = purchaseOrderWithDetails.User?.Username,
            Email = purchaseOrderWithDetails.User?.Email,
            SupplierName = purchaseOrderWithDetails.Supplier?.Name
        };
    }
    public async Task<IEnumerable<PurchaseOrderResponseDto>> GetAllAsync()
    {
        return await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.User)
            .Select(po => new PurchaseOrderResponseDto
            {
                Id = po.Id,
                SupplierId = po.SupplierId,
                OrderedBy = po.OrderedBy,
                OrderDate = po.OrderDate,
                Status = po.Status,
                // Username = po.User?.UserName ?? string.Empty,
                Username = po.User != null ? po.User.Username : string.Empty,
                Email = po.User != null ? po.User.Email : string.Empty,
                SupplierName = po.Supplier != null ? po.Supplier.Name : string.Empty
            })
            .ToListAsync();
    }
    public async Task<PurchaseOrderResponseDto?> GetByIdAsync(Guid id)
    {
        var purchaseOrder = await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.User)
            .FirstOrDefaultAsync(po => po.Id == id);

        if (purchaseOrder == null) return null;

        return new PurchaseOrderResponseDto
        {
            Id = purchaseOrder.Id,
            SupplierId = purchaseOrder.SupplierId,
            OrderedBy = purchaseOrder.OrderedBy,
            OrderDate = purchaseOrder.OrderDate,
            Status = purchaseOrder.Status,
            Username = purchaseOrder.User?.Username,
            Email = purchaseOrder.User?.Email,
            SupplierName = purchaseOrder.Supplier?.Name
        };
    }
    public async Task<PurchaseOrderResponseDto?> UpdateAsync(Guid id, PurchaseOrderUpdateDto dto)
    {
        var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
        if (purchaseOrder == null) return null;

        purchaseOrder.SupplierId = dto.SupplierId;
        purchaseOrder.OrderedBy = dto.OrderedBy;
        purchaseOrder.OrderDate = dto.OrderDate;
        purchaseOrder.Status = dto.Status;

        _context.PurchaseOrders.Update(purchaseOrder);
        await _context.SaveChangesAsync();

        // Reload the purchase order with Supplier and User included
        var updatedPurchaseOrder = await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.User)
            .FirstOrDefaultAsync(po => po.Id == purchaseOrder.Id);

        if (updatedPurchaseOrder == null) return null;

        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Update",
            tableAffected: "PurchaseOrder",
            recordId: updatedPurchaseOrder.Id,
            ipAddress: GetIpAddress()
        );

        return new PurchaseOrderResponseDto
        {
            Id = updatedPurchaseOrder.Id,
            SupplierId = updatedPurchaseOrder.SupplierId,
            OrderedBy = updatedPurchaseOrder.OrderedBy,
            OrderDate = updatedPurchaseOrder.OrderDate,
            Status = updatedPurchaseOrder.Status,
            Username = updatedPurchaseOrder.User?.Username,
            Email = updatedPurchaseOrder.User?.Email,
            SupplierName = updatedPurchaseOrder.Supplier?.Name
        };
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
        if (purchaseOrder == null) return false;

        _context.PurchaseOrders.Remove(purchaseOrder);
        await _context.SaveChangesAsync();
        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Delete",
            tableAffected: "PurchaseOrder",
            recordId: purchaseOrder.Id,
            ipAddress: GetIpAddress()
        );
        return true;
    }
}
