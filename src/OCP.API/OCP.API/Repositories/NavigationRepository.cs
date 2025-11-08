using Npgsql;
using OCP.API.Data;
using OCP.API.DTOs;

namespace OCP.API.Repositories;

public class NavigationRepository(ApplicationDbContext context) : INavigationRepository
{
    public async Task<IReadOnlyList<MenuDto>> GetMenusAsync(CancellationToken ct)
    {
        return await context.Menus
            .OrderBy(m => m.Id)
            .Select(m => new MenuDto
            {
                Id = m.Id,
                DisplayName = m.DisplayName
            })
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<PageDto>> GetPagesAsync(CancellationToken ct)
    {
        return await context.Pages
            .OrderBy(p => p.Id)
            .Select(p => new PageDto
            {
                Id = p.Id,
                MenuId = p.MenuId,
                DisplayName = p.DisplayName,
                IsDeleted = p.IsDeleted
            })
            .ToListAsync(ct);
    }
}
