namespace OCP.API.DTOs;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTimeOffset Expiration { get; set; }
}
