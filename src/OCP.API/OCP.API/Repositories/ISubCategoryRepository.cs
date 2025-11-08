using OCP.API.DTOs;

namespace OCP.API.Repositories;

public interface ISubCategoryRepository
{
    Task<int> CreateAsync(CreateSubCategoryDto dto, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateSubCategoryDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<SubCategoryDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<SubCategoryDto>> GetAllAsync(CancellationToken ct);
}
