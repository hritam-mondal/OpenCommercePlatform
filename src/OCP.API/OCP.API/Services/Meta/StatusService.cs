using OCP.API.DTOs;
using OCP.API.Repositories;

namespace OCP.API.Services.Meta;

public class StatusService(IStatusRepository repo) : IStatusService
{
    public Task<IReadOnlyList<StatusDto>> GetAllAsync(CancellationToken ct)
    {
        return repo.GetAllAsync(ct);
    }
}
