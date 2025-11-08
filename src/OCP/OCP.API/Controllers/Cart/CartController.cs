using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCP.API.DTOs;
using OCP.API.Services.Cart;

namespace OCP.API.Controllers.Cart;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CartController(ICartService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int userId, CancellationToken ct)
    {
        var cart = await service.GetCartAsync(userId, ct);
        return cart is null ? NotFound() : Ok(cart);
    }

    [HttpPost("ensure")]
    public async Task<IActionResult> Ensure([FromBody] int userId, CancellationToken ct)
    {
        var id = await service.EnsureCartAsync(userId, ct);
        return Ok(new { id });
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddCartItemDto dto, CancellationToken ct)
    {
        var id = await service.AddItemAsync(dto, ct);
        return Created(string.Empty, new { id });
    }

    [HttpPut("items/{id:int}")]
    public async Task<IActionResult> UpdateItem(int id, [FromBody] UpdateCartItemDto dto, CancellationToken ct)
    {
        var ok = await service.UpdateItemAsync(id, dto, ct);
        return ok ? Ok(new { message = "Updated" }) : NotFound();
    }

    [HttpDelete("items/{id:int}")]
    public async Task<IActionResult> RemoveItem(int id, CancellationToken ct)
    {
        var ok = await service.RemoveItemAsync(id, ct);
        return ok ? Ok(new { message = "Deleted" }) : NotFound();
    }
}
