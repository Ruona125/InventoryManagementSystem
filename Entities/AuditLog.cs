public class AuditLog
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Action { get; set; } = null!;
    public string TableAffected { get; set; } = null!;
    public int RecordId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? IPAddress { get; set; }

    public User? User { get; set; }
}
