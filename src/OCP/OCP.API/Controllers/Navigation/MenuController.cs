using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCP.API.Services.Navigation;

namespace OCP.API.Controllers.Navigation;

[Route("api/v1/navigation/[controller]")]
[ApiController]
[Authorize]
public class MenuController(INavigationService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var menus = await service.GetMenusAsync(ct);
        return Ok(menus);
    }
}
