using OCP.API.DTOs;

namespace OCP.API.Services.Catalog;

public interface IItemsService
{
    Task<int> CreateAsync(CreateItemDto dto, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateItemDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<ItemDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<ItemDto>> GetAllAsync(CancellationToken ct);
}
