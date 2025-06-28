using WebAPI.DTOs;

public interface IAuditLogService
{
    Task<IEnumerable<AuditLogResponseDto>> GetAllAsync();
    Task<AuditLogResponseDto?> GetByIdAsync(int id);
    Task LogAsync(int userId, string action, string tableAffected, int recordId, string? ipAddress);
}
