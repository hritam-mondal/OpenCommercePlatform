using OCP.API.DTOs;
using OCP.API.Repositories;

namespace OCP.API.Services.Cart;

public class CartService(ICartRepository repo) : ICartService
{
    public Task<CartDto?> GetCartAsync(int userId, CancellationToken ct)
    {
        return repo.GetCartAsync(userId, ct);
    }

    public Task<int> EnsureCartAsync(int userId, CancellationToken ct)
    {
        return repo.EnsureCartAsync(userId, ct);
    }

    public Task<int> AddItemAsync(AddCartItemDto dto, CancellationToken ct)
    {
        return repo.AddItemAsync(dto, ct);
    }

    public Task<bool> UpdateItemAsync(int cartItemId, UpdateCartItemDto dto, CancellationToken ct)
    {
        return repo.UpdateItemAsync(cartItemId, dto, ct);
    }

    public Task<bool> RemoveItemAsync(int cartItemId, CancellationToken ct)
    {
        return repo.RemoveItemAsync(cartItemId, ct);
    }
}
