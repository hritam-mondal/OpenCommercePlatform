using Npgsql;
using NpgsqlTypes;
using OCP.API.Data;
using OCP.API.DTOs;
using OCP.API.Models;

namespace OCP.API.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    public async Task<int> CreateAsync(CreateCategoryDto dto, CancellationToken ct)
    {
        var category = new Category { Name = dto.Name, IsActive = dto.IsActive };

        context.Categories.Add(category);
        await context.SaveChangesAsync(ct);
        return category.Id;
    }

    public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct)
    {
        var category = await context.Categories.FindAsync([id], ct);
        if (category == null)
        {
            return false;
        }

        if (dto.Name != null)
        {
            category.Name = dto.Name;
        }

        if (dto.IsActive.HasValue)
        {
            category.IsActive = dto.IsActive.Value;
        }

        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var category = await context.Categories.FindAsync(new object[] { id }, ct);
        if (category == null)
        {
            return false;
        }

        context.Categories.Remove(category);
        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Categories
            .Where(c => c.Id == id)
            .Select(c => new CategoryDto { Id = c.Id, Name = c.Name, IsActive = c.IsActive })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken ct)
    {
        return await context.Categories
            .OrderByDescending(c => c.Id)
            .Select(c => new CategoryDto { Id = c.Id, Name = c.Name, IsActive = c.IsActive })
            .ToListAsync(ct);
    }
}
