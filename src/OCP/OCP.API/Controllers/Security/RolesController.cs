using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCP.API.Services.Security;

namespace OCP.API.Controllers.Security;

[Route("api/v1/security/[controller]")]
[ApiController]
[Authorize]
public class RolesController(IRolesService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var roles = await service.GetRolesAsync(ct);
        return Ok(roles);
    }

    [HttpPost("assign")]
    public async Task<IActionResult> Assign([FromQuery] int userId, [FromQuery] int roleId, CancellationToken ct)
    {
        var ok = await service.AssignUserRoleAsync(userId, roleId, ct);
        return ok ? Ok(new { message = "Assigned" }) : Problem("Failed to assign role");
    }
}
