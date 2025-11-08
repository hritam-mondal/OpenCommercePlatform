using OCP.API.DTOs;

namespace OCP.API.Repositories;

public interface IDiscountRepository
{
    Task<int> CreateAsync(CreateDiscountDto dto, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateDiscountDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<DiscountDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<DiscountDto>> GetAllAsync(CancellationToken ct);
}
