using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCP.API.DTOs;
using OCP.API.Services.Orders;

namespace OCP.API.Controllers.Orders;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class OrdersController(IOrdersService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto, CancellationToken ct)
    {
        var id = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int userId, [FromQuery] int? storeId, [FromQuery] int limit = 50,
        [FromQuery] int offset = 0, CancellationToken ct = default)
    {
        var list = await service.ListAsync(userId, storeId, Math.Clamp(limit, 1, 1000), Math.Max(0, offset), ct);
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken ct)
    {
        var order = await service.GetByIdAsync(id, ct);
        return order is null ? NotFound() : Ok(order);
    }
}
