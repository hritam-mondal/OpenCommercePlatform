using OCP.API.DTOs;
using OCP.API.Models;

namespace OCP.API.Repositories;

public interface IUserRepository
{
    /// <summary>
    ///     Registers a new user in the system via stored function.
    ///     Returns:
    ///     1 = success, 0 = email exists
    /// </summary>
    Task<int> RegisterUserAsync(User user, string passwordHash, string passwordSalt, Guid token, DateTime expires,
        CancellationToken ct);

    Task<User?> GetByIdAsync(int userId);
    Task<List<User>> GetAllAsync(UserFilterDto filter);
    Task UpdateAsync(User user);
    Task<bool> DeleteAsync(int userId);
    Task<AuthUserDto?> GetAuthUserAsync(string emailOrUsername, CancellationToken ct);
}
