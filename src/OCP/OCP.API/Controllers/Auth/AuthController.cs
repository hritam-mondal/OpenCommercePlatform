using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCP.API.DTOs;
using OCP.API.Services.Auth;

namespace OCP.API.Controllers.Auth;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController(
    ILogger<AuthController> logger,
    IAuthService authService) : ControllerBase
{
    /// <summary>
    ///     Registers a new user.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var result = await authService.RegisterAsync(dto, ct);

            return result switch
            {
                RegisterResult.Success => Created(string.Empty, new { message = "Registration successful." }),
                RegisterResult.EmailAlreadyExists => BadRequest(new { error = "Email already exists." }),
                RegisterResult.UsernameAlreadyExists => BadRequest(new { error = "UserName already exists." }),
                RegisterResult.Failure => Problem("Registration failed due to server error.", statusCode: 500),
                _ => Problem("Unknown registration result.", statusCode: 500)
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during registration for {Email}", dto.Email);
            return Problem("An unexpected error occurred.", statusCode: 500);
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var token = await authService.LoginAsync(dto, ct);
            if (token == null)
            {
                return Unauthorized(new { error = "Invalid credentials or inactive account" });
            }

            return Ok(new { token = token.Token, expiration = token.Expiration });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during login for {Login}", dto.EmailOrUsername);
            return Problem("An unexpected error occurred.", statusCode: 500);
        }
    }
}
