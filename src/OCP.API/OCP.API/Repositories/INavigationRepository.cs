using OCP.API.DTOs;

namespace OCP.API.Repositories;

public interface INavigationRepository
{
    Task<IReadOnlyList<MenuDto>> GetMenusAsync(CancellationToken ct);
    Task<IReadOnlyList<PageDto>> GetPagesAsync(CancellationToken ct);
}
