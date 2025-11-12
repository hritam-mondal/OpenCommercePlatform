using OCP.API.DTOs;

namespace OCP.API.Services.Navigation;

public interface INavigationService
{
    Task<IReadOnlyList<MenuDto>> GetMenusAsync(CancellationToken ct);
    Task<IReadOnlyList<PageDto>> GetPagesAsync(CancellationToken ct);
}
