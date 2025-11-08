using OCP.API.DTOs;
using OCP.API.Repositories;
using OCP.API.Services.Auth;

namespace OCP.API.Services.Users;

/// <summary>
///     Handles all business logic for user operations.
/// </summary>
public class UsersService(IUserRepository userRepository) : IUsersService
{
    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        return new UserDto
        {
            UserId = (int)user.UserId,
            Username = user.Username,
            Email = user.Email,
            FullName = string.Join(" ",
                new[]
                {
                    user.FirstName,
                    user.LastName
                }.Where(s => !string.IsNullOrWhiteSpace(s))),
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync(UserFilterDto filter)
    {
        var users = await userRepository.GetAllAsync(filter);

        return users.Select(u => new UserDto
            {
                UserId = (int)u.UserId,
                Username = u.Username,
                Email = u.Email,
                FullName = string.Join(" ",
                    new[] { u.FirstName, u.LastName }.Where(s => !string.IsNullOrWhiteSpace(s))),
                PhoneNumber = u.PhoneNumber
            })
            .ToList();
    }

    public async Task<bool> UpdateUserAsync(UpdateUserDto dto)
    {
        var user = await userRepository.GetByIdAsync(dto.UserId);
        if (user == null)
        {
            return false;
        }

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.PhoneNumber = dto.PhoneNumber;

        await userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        return await userRepository.DeleteAsync(userId);
    }

    public async Task<PasswordChangeResult> ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return PasswordChangeResult.UserNotFound;
        }

        var isValid =
            PasswordHasher.VerifyPasswordFromBase64(dto.OldPassword, user.PasswordHash,
                user.PasswordSalt ?? string.Empty);

        if (!isValid)
        {
            return PasswordChangeResult.InvalidOldPassword;
        }

        var (newHash, newSalt) = PasswordHasher.HashPasswordToBase64(dto.NewPassword);
        user.PasswordHash = newHash;
        user.PasswordSalt = newSalt;

        await userRepository.UpdateAsync(user);
        return PasswordChangeResult.Success;
    }
}
