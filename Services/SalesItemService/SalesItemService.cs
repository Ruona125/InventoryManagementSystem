using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;

public class SalesItemService : ISalesItemService
{
    private readonly AppDbContext _context;
    public SalesItemService(AppDbContext context)
    {
        _context = context;
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
