using OCP.API.Data;
using OCP.API.DTOs;
using OCP.API.Models;

namespace OCP.API.Repositories;

public class RoleRepository(ApplicationDbContext context, ILogger<RoleRepository> logger) : IRoleRepository
{
    public async Task<IReadOnlyList<RoleDto>> GetRolesAsync(CancellationToken ct)
    {
        var q = context.Roles
            .AsNoTracking()
            .OrderBy(r => r.Id)
            .Select(r => new RoleDto { Id = r.Id, Name = r.Name });

        return await q.ToListAsync(ct);
    }

    public async Task<bool> AssignUserRoleAsync(int userId, int roleId, CancellationToken ct)
    {
        // check for existing assignment
        var exists = await context.UserRoles
            .AsNoTracking()
            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId, ct);

        if (exists) return false; // already assigned

        var userRole = new UserRole
        {
            UserId = userId,
            RoleId = roleId
        };

        try
        {
            context.UserRoles.Add(userRole);
            var affected = await context.SaveChangesAsync(ct);
            return affected > 0;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to assign role {RoleId} to user {UserId}", roleId, userId);
            throw;
        }
    }
}
