using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCP.API.Services.Navigation;

namespace OCP.API.Controllers.Navigation;

[Route("api/v1/navigation/[controller]")]
[ApiController]
[Authorize]
public class PagesController(INavigationService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var pages = await service.GetPagesAsync(ct);
        return Ok(pages);
    }
}
