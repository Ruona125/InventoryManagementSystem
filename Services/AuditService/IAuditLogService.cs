using WebAPI.DTOs;

public interface IAuditLogService
{
    Task<IEnumerable<AuditLogResponseDto>> GetAllAsync();
    Task<AuditLogResponseDto?> GetByIdAsync(Guid id);
    Task LogAsync(Guid userId, string action, string tableAffected, Guid recordId, string? ipAddress);
}
