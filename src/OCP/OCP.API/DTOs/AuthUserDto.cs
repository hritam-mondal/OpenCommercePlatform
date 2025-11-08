namespace OCP.API.DTOs;

public record AuthUserDto(
    long UserId,
    string Username,
    string Email,
    string PasswordHash,
    string? PasswordSalt,
    bool IsActive,
    bool IsConfirmed,
    string? FirstName,
    string? LastName,
    bool IsAdmin
)
{
    public string FullName => string.Join(" ", new[] { FirstName, LastName }
        .Where(s => !string.IsNullOrWhiteSpace(s)));
}
