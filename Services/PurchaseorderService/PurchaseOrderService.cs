using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly AppDbContext _context;
    public PurchaseOrderService(AppDbContext context)
    {
        _context = context;
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
        return true;
    }
}
