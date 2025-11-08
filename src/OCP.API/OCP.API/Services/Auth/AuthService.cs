using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OCP.API.DTOs;
using OCP.API.Models;
using OCP.API.Repositories;

namespace OCP.API.Services.Auth;

public class AuthService(
    IUserRepository userRepository,
    ILogger<AuthService> logger,
    IConfiguration config)
    : IAuthService
{
    public async Task<RegisterResult> RegisterAsync(RegisterDto dto, CancellationToken ct)
    {
        var user = new User
        {
            Username = dto.Username,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };

        // Hash password
        var (hash, salt) = PasswordHasher.HashPasswordToBase64(dto.Password);

        // Generate email confirmation token
        var confirmationToken = Guid.NewGuid();
        var tokenExpiresAt = DateTime.UtcNow.AddHours(24);

        try
        {
            var status =
                await userRepository.RegisterUserAsync(user, hash, salt, confirmationToken, tokenExpiresAt, ct);

            return status switch
            {
                1 => RegisterResult.Success,
                0 => RegisterResult.EmailAlreadyExists,
                -2 => RegisterResult.UsernameAlreadyExists,
                _ => RegisterResult.Failure
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while registering user {Email}", dto.Email);
            return RegisterResult.Failure;
        }
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto dto, CancellationToken ct)
    {
        var record = await userRepository.GetAuthUserAsync(dto.EmailOrUsername, ct);
        if (record == null)
        {
            return null;
        }

        var valid = PasswordHasher.VerifyPasswordFromBase64(dto.Password, record.PasswordHash,
            record.PasswordSalt ?? string.Empty);
        if (!valid || !record.IsActive)
        {
            return null;
        }

        var fullName = string.IsNullOrWhiteSpace(record.FullName) ? string.Empty : record.FullName;

        var issuer = config["Jwt:Issuer"] ?? "OCP.API";
        var audience = config["Jwt:Audience"] ?? "OCP.API.Clients";
        var key = config["Jwt:Key"] ?? string.Empty;
        var expiresMinutes = int.TryParse(config["Jwt:ExpiresMinutes"], out var m) ? m : 60;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, record.UserId.ToString()),
            new(ClaimTypes.NameIdentifier, record.UserId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, record.Username),
            new(ClaimTypes.Name, record.Username),
            new(JwtRegisteredClaimNames.Email, record.Email),
            new(ClaimTypes.Email, record.Email),
            new("FullName", fullName),
            new(ClaimTypes.Role, record.IsAdmin ? "Admin" : "User")
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials
        );

        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwt);

        return new LoginResponseDto { Token = token, Expiration = DateTimeOffset.UtcNow.AddMinutes(expiresMinutes) };
    }
}
