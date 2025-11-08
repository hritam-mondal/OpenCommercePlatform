using OCP.API.DTOs;

namespace OCP.API.Repositories;

public interface IStatusRepository
{
    Task<IReadOnlyList<StatusDto>> GetAllAsync(CancellationToken ct);
}
