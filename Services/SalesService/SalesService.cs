using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;

public class SalesService : ISalesService
{
    private readonly AppDbContext _context;
    public SalesService(AppDbContext context)
    {
        _context = context;
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

        return new SalesResponseDto
        {
            Id = sale.Id,
            SoldBy = sale.SoldBy,
            TotalAmount = sale.TotalAmount,
            SaleDate = sale.SaleDate
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

        return true;
    }
}
