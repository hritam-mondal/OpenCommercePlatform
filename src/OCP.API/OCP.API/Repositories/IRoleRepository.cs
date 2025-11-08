using OCP.API.DTOs;

namespace OCP.API.Repositories;

public interface IRoleRepository
{
    Task<IReadOnlyList<RoleDto>> GetRolesAsync(CancellationToken ct);
    Task<bool> AssignUserRoleAsync(int userId, int roleId, CancellationToken ct);
}
