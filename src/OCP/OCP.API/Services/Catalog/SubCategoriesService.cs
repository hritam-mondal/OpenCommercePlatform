using OCP.API.DTOs;
using OCP.API.Repositories;

namespace OCP.API.Services.Catalog;

public interface ISubCategoriesService
{
    Task<int> CreateAsync(CreateSubCategoryDto dto, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateSubCategoryDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<SubCategoryDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<SubCategoryDto>> GetAllAsync(CancellationToken ct);
}

public class SubCategoriesService(ISubCategoryRepository repo) : ISubCategoriesService
{
    public Task<int> CreateAsync(CreateSubCategoryDto dto, CancellationToken ct)
    {
        return repo.CreateAsync(dto, ct);
    }

    public Task<bool> UpdateAsync(int id, UpdateSubCategoryDto dto, CancellationToken ct)
    {
        return repo.UpdateAsync(id, dto, ct);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        return repo.DeleteAsync(id, ct);
    }

    public Task<SubCategoryDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return repo.GetByIdAsync(id, ct);
    }

    public Task<IReadOnlyList<SubCategoryDto>> GetAllAsync(CancellationToken ct)
    {
        return repo.GetAllAsync(ct);
    }
}
