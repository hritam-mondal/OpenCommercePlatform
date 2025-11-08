using OCP.API.DTOs;
using OCP.API.Repositories;

namespace OCP.API.Services.Security;

public class RolesService(IRoleRepository repo) : IRolesService
{
    public Task<IReadOnlyList<RoleDto>> GetRolesAsync(CancellationToken ct)
    {
        return repo.GetRolesAsync(ct);
    }

    public Task<bool> AssignUserRoleAsync(int userId, int roleId, CancellationToken ct)
    {
        return repo.AssignUserRoleAsync(userId, roleId, ct);
    }
}
