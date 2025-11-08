using OCP.API.DTOs;
using OCP.API.Repositories;

namespace OCP.API.Services.Catalog;

public class CategoriesService(ICategoryRepository repo, ILogger<CategoriesService> logger) : ICategoriesService
{
    public Task<int> CreateAsync(CreateCategoryDto dto, CancellationToken ct)
    {
        return repo.CreateAsync(dto, ct);
    }

    public Task<bool> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct)
    {
        return repo.UpdateAsync(id, dto, ct);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        return repo.DeleteAsync(id, ct);
    }

    public Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return repo.GetByIdAsync(id, ct);
    }

    public Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken ct)
    {
        return repo.GetAllAsync(ct);
    }
}
