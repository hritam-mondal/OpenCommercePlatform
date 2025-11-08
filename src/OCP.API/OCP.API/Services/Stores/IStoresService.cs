using OCP.API.DTOs;

namespace OCP.API.Services.Stores;

public interface IStoresService
{
    Task<int> CreateAsync(CreateStoreDto dto, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateStoreDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<StoreDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<StoreDto>> GetAllAsync(CancellationToken ct);
}
