using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCP.API.DTOs;
using OCP.API.Services.Catalog;

namespace OCP.API.Controllers.Catalog;

[Route("api/v1/catalog/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController(ICategoriesService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var list = await service.GetAllAsync(ct);
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var cat = await service.GetByIdAsync(id, ct);
        return cat is null ? NotFound() : Ok(cat);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var id = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto, CancellationToken ct)
    {
        var ok = await service.UpdateAsync(id, dto, ct);
        return ok ? Ok(new { message = "Updated" }) : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await service.DeleteAsync(id, ct);
        return ok ? Ok(new { message = "Deleted" }) : NotFound();
    }
}
