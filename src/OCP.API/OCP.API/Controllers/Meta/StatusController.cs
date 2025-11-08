using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCP.API.Services.Meta;

namespace OCP.API.Controllers.Meta;

[Route("api/v1/meta/[controller]")]
[ApiController]
[Authorize]
public class StatusController(IStatusService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var statuses = await service.GetAllAsync(ct);
        return Ok(statuses);
    }
}
