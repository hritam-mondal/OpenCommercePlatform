using OCP.API.DTOs;

namespace OCP.API.Repositories;

public interface ICartRepository
{
    Task<CartDto?> GetCartAsync(int userId, CancellationToken cancellationToken);
    Task<int> EnsureCartAsync(int userId, CancellationToken ct);
    Task<int> AddItemAsync(AddCartItemDto dto, CancellationToken ct);
    Task<bool> UpdateItemAsync(int itemId, UpdateCartItemDto dto, CancellationToken ct);
    Task<bool> RemoveItemAsync(int itemId, CancellationToken ct);
}
