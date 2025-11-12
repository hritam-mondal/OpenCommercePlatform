using OCP.API.DTOs;

namespace OCP.API.Services.Cart;

public interface ICartService
{
    Task<CartDto?> GetCartAsync(int userId, CancellationToken ct);
    Task<int> EnsureCartAsync(int userId, CancellationToken ct);
    Task<int> AddItemAsync(AddCartItemDto dto, CancellationToken ct);
    Task<bool> UpdateItemAsync(int cartItemId, UpdateCartItemDto dto, CancellationToken ct);
    Task<bool> RemoveItemAsync(int cartItemId, CancellationToken ct);
}
