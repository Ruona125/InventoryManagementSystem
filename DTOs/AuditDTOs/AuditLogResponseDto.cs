namespace WebAPI.DTOs;

public class AuditLogResponseDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string UserName { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string TableAffected { get; set; } = null!;
    public Guid RecordId { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; }
    public string? IPAddress { get; set; }
}
