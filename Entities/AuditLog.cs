public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Action { get; set; } = null!;
    public string TableAffected { get; set; } = null!;
    public Guid RecordId { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? IPAddress { get; set; }

    public User? User { get; set; }
}
