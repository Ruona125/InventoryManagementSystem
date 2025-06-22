using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;
public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        return await _db.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                Barcode = p.Barcode,
                Quantity = p.Quantity,
                ReorderThreshold = p.ReorderThreshold,
                CategoryName = p.Category != null ? p.Category.Name : null,
                SupplierName = p.Supplier != null ? p.Supplier.Name : null,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ProductResponseDto?> GetByIdAsync(Guid id)
    {
        var p = await _db.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);

        return p is null ? null : new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            SKU = p.SKU,
            Barcode = p.Barcode,
            Quantity = p.Quantity,
            ReorderThreshold = p.ReorderThreshold,
            CategoryName = p.Category?.Name,
            SupplierName = p.Supplier?.Name
        };
    }

    public async Task<ProductResponseDto> CreateAsync(ProductCreateDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            SKU = dto.SKU,
            Barcode = dto.Barcode,
            Quantity = dto.Quantity,
            ReorderThreshold = dto.ReorderThreshold,
            CategoryId = dto.CategoryId,
            SupplierId = dto.SupplierId
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return await GetByIdAsync(product.Id) ?? throw new Exception("Product creation failed.");
    }

    public async Task<ProductResponseDto?> UpdateAsync(Guid id, ProductUpdateDto dto)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return null;

        product.Name = dto.Name ?? product.Name;
        product.SKU = dto.SKU ?? product.SKU;
        product.Barcode = dto.Barcode ?? product.Barcode;
        product.Quantity = dto.Quantity ?? product.Quantity;
        product.ReorderThreshold = dto.ReorderThreshold ?? product.ReorderThreshold;
        product.CategoryId = dto.CategoryId.HasValue ? new Guid(dto.CategoryId.Value.ToString()) : product.CategoryId;
        product.SupplierId = dto.SupplierId.HasValue ? new Guid(dto.SupplierId.Value.ToString()) : product.SupplierId;

        await _db.SaveChangesAsync();

        return await GetByIdAsync(product.Id);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return false;

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }
}
