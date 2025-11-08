using OCP.API.DTOs;

namespace OCP.API.Repositories;

public interface ICategoryRepository
{
    Task<int> CreateAsync(CreateCategoryDto dto, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken ct);
}
