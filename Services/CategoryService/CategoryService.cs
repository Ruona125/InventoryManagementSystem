using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CategoryService(AppDbContext context, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
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

    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return null;

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto)
    {
        var category = new Category
        {
            Name = dto.Name
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Create",
            tableAffected: "Category",
            recordId: category.Id,
            ipAddress: GetIpAddress()
        );

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<CategoryResponseDto?> UpdateAsync(Guid id, CategoryUpdateDto dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return null;

        category.Name = dto.Name;
        await _context.SaveChangesAsync();

        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Update",
            tableAffected: "Category",
            recordId: category.Id,
            ipAddress: GetIpAddress()
        );

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        // Audit log
        await _auditLogService.LogAsync(
            userId: GetCurrentUserId(),
            action: "Delete",
            tableAffected: "Category",
            recordId: category.Id,
            ipAddress: GetIpAddress()
        );

        return true;
    }
}
