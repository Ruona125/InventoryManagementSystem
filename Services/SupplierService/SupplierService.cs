using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;

public class SupplierService : ISupplierService
{
    private readonly AppDbContext _context;

    public SupplierService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SupplierResponseDto>> GetAllAsync()
    {
        return await _context.Suppliers
            .Select(s => new SupplierResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                ContactEmail = s.ContactEmail ?? string.Empty,
                Phone = s.Phone ?? string.Empty,
                Address = s.Address ?? string.Empty
            })
            .ToListAsync();
    }

    public async Task<SupplierResponseDto?> GetByIdAsync(Guid id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null) return null;

        return new SupplierResponseDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            ContactEmail = supplier.ContactEmail ?? string.Empty,
            Phone = supplier.Phone ?? string.Empty,
            Address = supplier.Address ?? string.Empty
        };
    }

    public async Task<SupplierResponseDto> CreateAsync(SupplierCreateDto supplierDto)
    {
        var supplier = new Supplier
        {
            Id = supplierDto.Id,
            Name = supplierDto.Name,
            ContactEmail = supplierDto.ContactEmail,
            Phone = supplierDto.Phone,
            Address = supplierDto.Address
        };

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        return new SupplierResponseDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            ContactEmail = supplier.ContactEmail ?? string.Empty,
            Phone = supplier.Phone ?? string.Empty,
            Address = supplier.Address ?? string.Empty
        };
    }


    public async Task<SupplierResponseDto?> UpdateAsync(Guid id, SupplierUpdateDto supplierDto)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null) return null;

        supplier.Name = supplierDto.Name;
        supplier.ContactEmail = supplierDto.ContactEmail;
        supplier.Phone = supplierDto.Phone;
        supplier.Address = supplierDto.Address;

        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync();

        return new SupplierResponseDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            ContactEmail = supplier.ContactEmail ?? string.Empty,
            Phone = supplier.Phone ?? string.Empty,
            Address = supplier.Address ?? string.Empty
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null) return false;

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
        return true;
    }
}
