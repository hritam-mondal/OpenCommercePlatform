using OCP.API.DTOs;

namespace OCP.API.Repositories;

public interface IUnitRepository
{
    Task<int> CreateAsync(CreateUnitDto dto, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateUnitDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<UnitDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<UnitDto>> GetAllAsync(CancellationToken ct);
}
