using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;

public class AuditLogService : IAuditLogService
{
    private readonly AppDbContext _db;

    public AuditLogService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<AuditLogResponseDto>> GetAllAsync()
    {
        return await _db.AuditLogs
            .Include(a => a.User)
            .OrderByDescending(a => a.Timestamp)
            .Select(a => new AuditLogResponseDto
            {
                Id = a.Id,
                UserId = a.UserId,
                UserName = a.User!.Username,
                Action = a.Action,
                TableAffected = a.TableAffected,
                RecordId = a.RecordId,
                Timestamp = a.Timestamp,
                IPAddress = a.IPAddress
            })
            .ToListAsync();
    }

    public async Task<AuditLogResponseDto?> GetByIdAsync(int id)
    {
        var a = await _db.AuditLogs
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (a == null) return null;

        return new AuditLogResponseDto
        {
            Id = a.Id,
            UserId = a.UserId,
            UserName = a.User!.Username,
            Action = a.Action,
            TableAffected = a.TableAffected,
            RecordId = a.RecordId,
            Timestamp = a.Timestamp,
            IPAddress = a.IPAddress
        };
    }

    public async Task LogAsync(int userId, string action, string tableAffected, int recordId, string? ipAddress)
    {
        var log = new AuditLog
        {
            UserId = userId,
            Action = action,
            TableAffected = tableAffected,
            RecordId = recordId,
            IPAddress = ipAddress,
            Timestamp = DateTime.UtcNow
        };

        _db.AuditLogs.Add(log);
        await _db.SaveChangesAsync();
    }
}
