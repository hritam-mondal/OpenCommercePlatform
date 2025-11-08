using OCP.API.DTOs;

namespace OCP.API.Services.Meta;

public interface IStatusService
{
    Task<IReadOnlyList<StatusDto>> GetAllAsync(CancellationToken ct);
}
