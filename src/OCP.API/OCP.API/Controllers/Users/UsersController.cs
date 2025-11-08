using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCP.API.Common.Extensions;
using OCP.API.DTOs;
using OCP.API.Services.Users;

namespace OCP.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUsersService _usersService;

    /// <summary>
    ///     Constructor injects the user service which handles business logic.
    /// </summary>
    /// <param name="usersService">The service responsible for user operations</param>
    /// <param name="logger"></param>
    public UsersController(IUsersService usersService, ILogger<UsersController> logger)
    {
        _usersService = usersService;
        _logger = logger;
    }

    /// <summary>
    ///     Returns details of the currently authenticated user.
    /// </summary>
    /// <returns>
    ///     200 - User details object
    ///     401 - Unauthorized (if token missing or invalid)
    ///     404 - User not found (if the user id claim is valid but record no longer exists)
    /// </returns>
    [HttpGet("self")]
    public async Task<IActionResult> GetCurrentUser()
    {
        // Extract user ID from JWT claims via extension method
        var userId = User.GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("Unauthorized access attempt: missing or invalid user id claim. Path: {Path}",
                HttpContext.Request.Path);
            return Unauthorized(new { error = "Unauthorized access. Invalid or missing user token." });
        }

        try
        {
            _logger.LogInformation("Fetching user profile for UserId: {UserId}", userId);

            // Fetch the user details
            var user = await _usersService.GetUserByIdAsync(userId.Value);
            if (user == null)
            {
                _logger.LogWarning("User profile not found. UserId: {UserId}", userId);
                return NotFound(new { error = "User not found." });
            }

            // Log successful operation
            _logger.LogInformation("Successfully returned profile for UserId: {UserId}", userId);

            // Return result
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user profile. UserId: {UserId}", userId);
            return StatusCode(500, new { error = "An unexpected error occurred while fetching the user profile." });
        }
    }

    /// <summary>
    ///     Returns a list of users based on search filters.
    /// </summary>
    /// <param name="filter">Search parameters</param>
    /// <returns>Filtered list of users</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] UserFilterDto filter)
    {
        var users = await _usersService.GetUsersAsync(filter);
        return Ok(users);
    }

    /// <summary>
    ///     Updates an existing user's information.
    /// </summary>
    /// <param name="dto">User update data</param>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var success = await _usersService.UpdateUserAsync(dto);

        return success ? Ok(new { message = "User updated successfully" }) : NotFound(new { error = "User not found" });
    }

    /// <summary>
    ///     Deletes a user by id.
    /// </summary>
    /// <param name="userId">ID of the user to delete</param>
    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> Delete(int userId)
    {
        var success = await _usersService.DeleteUserAsync(userId);
        return success ? Ok(new { message = "User deleted successfully" }) : NotFound(new { error = "User not found" });
    }
}
