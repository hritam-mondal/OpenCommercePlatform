using OCP.API.DTOs;

namespace OCP.API.Services.Security;

public interface IRolesService
{
    Task<IReadOnlyList<RoleDto>> GetRolesAsync(CancellationToken ct);
    Task<bool> AssignUserRoleAsync(int userId, int roleId, CancellationToken ct);
}
