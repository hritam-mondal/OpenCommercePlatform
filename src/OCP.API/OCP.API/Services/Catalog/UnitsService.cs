using OCP.API.DTOs;
using OCP.API.Repositories;

namespace OCP.API.Services.Catalog;

public interface IUnitsService
{
    Task<int> CreateAsync(CreateUnitDto dto, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateUnitDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<UnitDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<UnitDto>> GetAllAsync(CancellationToken ct);
}

public class UnitsService(IUnitRepository repo) : IUnitsService
{
    public Task<int> CreateAsync(CreateUnitDto dto, CancellationToken ct)
    {
        return repo.CreateAsync(dto, ct);
    }

    public Task<bool> UpdateAsync(int id, UpdateUnitDto dto, CancellationToken ct)
    {
        return repo.UpdateAsync(id, dto, ct);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        return repo.DeleteAsync(id, ct);
    }

    public Task<UnitDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return repo.GetByIdAsync(id, ct);
    }

    public Task<IReadOnlyList<UnitDto>> GetAllAsync(CancellationToken ct)
    {
        return repo.GetAllAsync(ct);
    }
}
