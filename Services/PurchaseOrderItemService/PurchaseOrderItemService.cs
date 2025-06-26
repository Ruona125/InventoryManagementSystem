using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;

public class PurchaseOrderItemService : IPurchaseOrderItemService
{
    private readonly AppDbContext _context;
    public PurchaseOrderItemService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<PurchaseOrderItemResponseDto> CreateAsync(PurchaseOrderItemCreateDto dto)
    {
        var item = new PurchaseOrderItem
        {
            Id = dto.Id,
            PurchaseOrderId = dto.PurchaseOrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice
        };
        _context.PurchaseOrderItems.Add(item);
        await _context.SaveChangesAsync();
        // Reload the purchase order item with Product and PurchaseOrder included
        var itemWithDetails = await _context.PurchaseOrderItems
            .Include(po => po.Product)
            .Include(po => po.PurchaseOrder)
            .FirstOrDefaultAsync(po => po.Id == item.Id);
        if (itemWithDetails == null)
        {
            throw new Exception("Purchase order item not found after creation.");
        }
        return new PurchaseOrderItemResponseDto
        {
            Id = itemWithDetails.Id,
            PurchaseOrderId = itemWithDetails.PurchaseOrderId,
            ProductId = itemWithDetails.ProductId,
            Quantity = itemWithDetails.Quantity,
            UnitPrice = itemWithDetails.UnitPrice,
            ProductName = itemWithDetails.Product?.Name
        };
    }
    public async Task<IEnumerable<PurchaseOrderItemResponseDto>> GetAllAsync()
    {
        return await _context.PurchaseOrderItems
            .Include(po => po.Product)
            .Include(po => po.PurchaseOrder)
            .Select(po => new PurchaseOrderItemResponseDto
            {
                Id = po.Id,
                PurchaseOrderId = po.PurchaseOrderId,
                ProductId = po.ProductId,
                Quantity = po.Quantity,
                UnitPrice = po.UnitPrice,
                ProductName = po.Product != null ? po.Product.Name : string.Empty
            })
            .ToListAsync();
    }
    public async Task<PurchaseOrderItemResponseDto?> GetByIdAsync(Guid id)
    {
        var item = await _context.PurchaseOrderItems
            .Include(po => po.Product)
            .Include(po => po.PurchaseOrder)
            .FirstOrDefaultAsync(po => po.Id == id);
        if (item == null)
        {
            return null;
        }
        return new PurchaseOrderItemResponseDto
        {
            Id = item.Id,
            PurchaseOrderId = item.PurchaseOrderId,
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            ProductName = item.Product?.Name
        };
    }
    public async Task<PurchaseOrderItemResponseDto?> UpdateAsync(Guid id, PurchaseOrderItemUpdateDto dto)
    {
        var item = await _context.PurchaseOrderItems.FindAsync(id);
        if (item == null)
        {
            return null;
        }
        item.Quantity = dto.Quantity;
        item.UnitPrice = dto.UnitPrice;
        await _context.SaveChangesAsync();
        return new PurchaseOrderItemResponseDto
        {
            Id = item.Id,
            PurchaseOrderId = item.PurchaseOrderId,
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            ProductName = item.Product?.Name
        };
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        var item = await _context.PurchaseOrderItems.FindAsync(id);
        if (item == null)
        {
            return false;
        }
        _context.PurchaseOrderItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
