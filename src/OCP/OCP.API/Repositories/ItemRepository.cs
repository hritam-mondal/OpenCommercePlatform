using System.Data;
using Npgsql;
using NpgsqlTypes;
using OCP.API.Data;
using OCP.API.DTOs;
using OCP.API.Models;

namespace OCP.API.Repositories;

public class ItemRepository(ApplicationDbContext context) : IItemRepository
{
    public async Task<int> CreateAsync(CreateItemDto dto, CancellationToken ct)
    {
        var item = new Item
        {
            SubCategoryId = dto.SubCategoryId,
            Name = dto.Name,
            Description = dto.Description,
            ShortDescription = dto.ShortDescription,
            ImageUrl = dto.ImageUrl,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = dto.CreatedBy,
            IsActive = dto.IsActive
        };

        context.Items.Add(item);
        await context.SaveChangesAsync(ct);
        return item.Id;
    }

    public async Task<bool> UpdateAsync(int id, UpdateItemDto dto, CancellationToken ct)
    {
        var item = await context.Items.FindAsync([id], ct);
        if (item == null)
        {
            return false;
        }

        if (dto.SubCategoryId.HasValue)
        {
            item.SubCategoryId = dto.SubCategoryId.Value;
        }

        if (dto.Name != null)
        {
            item.Name = dto.Name;
        }

        if (dto.Description != null)
        {
            item.Description = dto.Description;
        }

        if (dto.ShortDescription != null)
        {
            item.ShortDescription = dto.ShortDescription;
        }

        if (dto.ImageUrl != null)
        {
            item.ImageUrl = dto.ImageUrl;
        }

        if (dto.CreatedBy != null)
        {
            item.CreatedBy = dto.CreatedBy;
        }

        if (dto.IsActive.HasValue)
        {
            item.IsActive = dto.IsActive.Value;
        }

        var affected = await context.SaveChangesAsync(ct);
        return affected > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var item = await context.Items.FindAsync([id], ct);
        if (item == null)
        {
            return false;
        }

        context.Items.Remove(item);
        var affected = await context.SaveChangesAsync(ct);
        return affected > 0;
    }

    public async Task<ItemDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Items
            .Where(i => i.Id == id)
            .Select(i => new ItemDto
            {
                Id = i.Id,
                SubCategoryId = i.SubCategoryId,
                Name = i.Name,
                Description = i.Description,
                ShortDescription = i.ShortDescription,
                ImageUrl = i.ImageUrl,
                CreatedOn = i.CreatedOn,
                CreatedBy = i.CreatedBy,
                IsActive = i.IsActive
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<ItemDto>> GetAllAsync(CancellationToken ct)
    {
        return await context.Items
            .OrderByDescending(i => i.Id)
            .Select(i => new ItemDto
            {
                Id = i.Id,
                SubCategoryId = i.SubCategoryId,
                Name = i.Name,
                Description = i.Description,
                ShortDescription = i.ShortDescription,
                ImageUrl = i.ImageUrl,
                CreatedOn = i.CreatedOn,
                CreatedBy = i.CreatedBy,
                IsActive = i.IsActive
            })
            .ToListAsync(ct);
    }
}
