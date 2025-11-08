using OCP.API.DTOs;

namespace OCP.API.Repositories;

public interface IItemRepository
{
    Task<int> CreateAsync(CreateItemDto dto, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateItemDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<ItemDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<ItemDto>> GetAllAsync(CancellationToken ct);
}
