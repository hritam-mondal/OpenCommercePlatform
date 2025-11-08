using OCP.API.Data;
using OCP.API.DTOs;
using OCP.API.Models;

namespace OCP.API.Repositories;

public class CartRepository(ApplicationDbContext context) : ICartRepository
{
    public async Task<CartDto?> GetCartAsync(int userId, CancellationToken cancellationToken)
    {
        var cartEntity = await context.Carts
            .AsNoTracking()
            .Include(c => c.CartDetails)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

        if (cartEntity == null)
        {
            return null;
        }

        var cart = new CartDto
        {
            Id = cartEntity.Id,
            UserId = cartEntity.UserId,
            TotalItems = cartEntity.TotalItems,
            Items = cartEntity.CartDetails
                .OrderByDescending(d => d.Id)
                .Select(d => new CartItemDto
                {
                    Id = d.Id,
                    CartId = d.CartId,
                    ItemId = d.ItemId,
                    UnitId = d.UnitId,
                    ItemQuantity = d.ItemQuantity,
                    Rate = d.Rate,
                    ItemPrice = d.ItemPrice
                })
                .ToList()
        };

        return cart;
    }

    public async Task<int> EnsureCartAsync(int userId, CancellationToken ct)
    {
        var existing = await context.Carts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);

        if (existing != null)
        {
            return existing.Id;
        }

        var newCart = new Cart { UserId = userId, TotalItems = 0 };

        context.Carts.Add(newCart);
        await context.SaveChangesAsync(ct);

        return newCart.Id;
    }

    public async Task<int> AddItemAsync(AddCartItemDto dto, CancellationToken ct)
    {
        var detail = new CartDetail
        {
            CartId = dto.CartId,
            ItemId = dto.ItemId,
            UnitId = dto.UnitId,
            ItemQuantity = dto.Quantity,
            Rate = dto.Rate,
            ItemPrice = dto.Rate * dto.Quantity
        };

        context.CartDetails.Add(detail);
        await context.SaveChangesAsync(ct);

        return detail.Id;
    }

    public async Task<bool> UpdateItemAsync(int itemId, UpdateCartItemDto dto, CancellationToken ct)
    {
        var detail = await context.CartDetails.FindAsync([itemId], ct);
        if (detail == null)
        {
            return false;
        }

        if (dto.Quantity.HasValue)
        {
            detail.ItemQuantity = dto.Quantity.Value;
        }

        if (dto.Rate.HasValue)
        {
            detail.Rate = dto.Rate.Value;
        }

        context.CartDetails.Update(detail);
        var affected = await context.SaveChangesAsync(ct);
        return affected > 0;
    }

    public async Task<bool> RemoveItemAsync(int itemId, CancellationToken ct)
    {
        var detail = await context.CartDetails.FindAsync([itemId], ct);
        if (detail == null)
        {
            return false;
        }

        context.CartDetails.Remove(detail);
        var affected = await context.SaveChangesAsync(ct);
        return affected > 0;
    }
}
