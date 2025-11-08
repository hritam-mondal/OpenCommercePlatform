using OCP.API.Models;
using OCP.API.Repositories;

namespace OCP.API.Services.Audit;

public interface IAuditService
{
    Task<List<AuditLog>> GetLogsAsync(int limit, CancellationToken ct);
}

public class AuditService(IAuditRepository repo) : IAuditService
{
    public Task<List<AuditLog>> GetLogsAsync(int limit, CancellationToken ct)
    {
        return repo.GetLogsAsync(limit, ct);
    }
}
