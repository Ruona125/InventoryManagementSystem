namespace WebAPI.DTOs;

public class AuditLogResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string TableAffected { get; set; } = null!;
    public int RecordId { get; set; }
    public DateTime Timestamp { get; set; }
    public string? IPAddress { get; set; }
}
