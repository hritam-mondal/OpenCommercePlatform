using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCP.API.Services.Audit;

namespace OCP.API.Controllers.Audit;

[Route("api/v1/audit/[controller]")]
[ApiController]
[Authorize]
public class AuditLogsController(IAuditService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int limit = 100, CancellationToken ct = default)
    {
        var logs = await service.GetLogsAsync(Math.Clamp(limit, 1, 1000), ct);
        return Ok(logs);
    }
}
