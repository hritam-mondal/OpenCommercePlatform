using OCP.API.DTOs;

namespace OCP.API.Services.Auth;

public enum RegisterResult
{
    Success,
    EmailAlreadyExists,
    UsernameAlreadyExists,
    Failure
}

public interface IAuthService
{
    Task<RegisterResult> RegisterAsync(RegisterDto dto, CancellationToken ct);
    Task<LoginResponseDto?> LoginAsync(LoginDto dto, CancellationToken ct);
}
