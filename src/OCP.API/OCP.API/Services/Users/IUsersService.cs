using OCP.API.DTOs;

namespace OCP.API.Services.Users;

public enum PasswordChangeResult
{
    Success,
    UserNotFound,
    InvalidOldPassword
}

public interface IUsersService
{
    Task<UserDto?> GetUserByIdAsync(int userId);
    Task<IEnumerable<UserDto>> GetUsersAsync(UserFilterDto filter);
    Task<bool> UpdateUserAsync(UpdateUserDto dto);
    Task<bool> DeleteUserAsync(int userId);
    Task<PasswordChangeResult> ChangePasswordAsync(int userId, ChangePasswordDto dto);
}
