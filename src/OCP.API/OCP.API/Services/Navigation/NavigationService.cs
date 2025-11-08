using OCP.API.DTOs;
using OCP.API.Repositories;

namespace OCP.API.Services.Navigation;

public interface INavigationService
{
    Task<IReadOnlyList<MenuDto>> GetMenusAsync(CancellationToken ct);
    Task<IReadOnlyList<PageDto>> GetPagesAsync(CancellationToken ct);
}

public class NavigationService(INavigationRepository repo) : INavigationService
{
    public Task<IReadOnlyList<MenuDto>> GetMenusAsync(CancellationToken ct)
    {
        return repo.GetMenusAsync(ct);
    }

    public Task<IReadOnlyList<PageDto>> GetPagesAsync(CancellationToken ct)
    {
        return repo.GetPagesAsync(ct);
    }
}
