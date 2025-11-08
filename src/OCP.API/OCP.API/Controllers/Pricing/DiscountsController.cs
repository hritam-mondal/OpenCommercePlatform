using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCP.API.DTOs;
using OCP.API.Services.Pricing;

namespace OCP.API.Controllers.Pricing;

[Route("api/v1/pricing/[controller]")]
[ApiController]
[Authorize]
public class DiscountsController : ControllerBase
{
    private readonly IDiscountsService _service;

    public DiscountsController(IDiscountsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var list = await _service.GetAllAsync(ct);
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var entity = await _service.GetByIdAsync(id, ct);
        return entity is null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDiscountDto dto, CancellationToken ct)
    {
        var id = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDiscountDto dto, CancellationToken ct)
    {
        var ok = await _service.UpdateAsync(id, dto, ct);
        return ok ? Ok(new { message = "Updated" }) : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await _service.DeleteAsync(id, ct);
        return ok ? Ok(new { message = "Deleted" }) : NotFound();
    }
}
