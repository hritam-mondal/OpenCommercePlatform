using OCP.API.Models;

namespace OCP.API.Repositories;

public interface IAuditRepository
{
    Task<List<AuditLog>> GetLogsAsync(int limit, CancellationToken cancellationToken);
}
