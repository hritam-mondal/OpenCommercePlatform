using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCP.API.Services.Geo;

namespace OCP.API.Controllers.Geo;

[Route("api/v1/geo/[controller]")]
[ApiController]
[Authorize]
public class PincodesController(IPincodesService service) : ControllerBase
{
    [HttpGet("{pin}")]
    public async Task<IActionResult> GetByPin(string pin, CancellationToken ct)
    {
        var list = await service.GetByPinAsync(pin, ct);
        return Ok(list);
    }

    [HttpGet("stores/{storeId:int}")]
    public async Task<IActionResult> GetByStore(int storeId, CancellationToken ct)
    {
        var list = await service.GetByStoreAsync(storeId, ct);
        return Ok(list);
    }
}
