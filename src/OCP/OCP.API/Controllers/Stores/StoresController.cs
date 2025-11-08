using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCP.API.DTOs;
using OCP.API.Services.Stores;

namespace OCP.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class StoresController : ControllerBase
{
    private readonly IStoresService _service;
    private readonly ILogger<StoresController> _logger;

    public StoresController(IStoresService service, ILogger<StoresController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var stores = await _service.GetAllAsync(ct);
        return Ok(stores);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var store = await _service.GetByIdAsync(id, ct);
        return store is null ? NotFound() : Ok(store);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStoreDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var id = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStoreDto dto, CancellationToken ct)
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
