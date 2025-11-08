using OCP.API.Data;
using OCP.API.Models;

namespace OCP.API.Repositories;

public class AuditRepository(ApplicationDbContext context) : IAuditRepository
{
    public async Task<List<AuditLog>> GetLogsAsync(int limit, CancellationToken ct)
    {
        var logs = await context.AuditLogs
            .Take(limit)
            .ToListAsync(ct);

        return logs;
    }
}
