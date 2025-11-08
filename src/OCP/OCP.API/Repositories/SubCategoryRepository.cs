using OCP.API.Data;
using OCP.API.DTOs;
using OCP.API.Models;

namespace OCP.API.Repositories;

public class SubCategoryRepository(
    ApplicationDbContext context,
    ILogger<SubCategoryRepository> logger) : ISubCategoryRepository
{
    public async Task<int> CreateAsync(CreateSubCategoryDto dto, CancellationToken ct)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var entity = new SubCategory
        {
            Name = dto.Name, CategoryId = dto.CategoryId, IsActive = dto.IsActive, StoreId = dto.StoreId
        };

        await using var tx = await context.Database.BeginTransactionAsync(ct);
        try
        {
            context.SubCategories.Add(entity);
            await context.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return entity.Id;
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(ct);
            logger.LogError(ex, "Failed to create sub-category");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(int id, UpdateSubCategoryDto dto, CancellationToken ct)
    {
        var entity = await context.SubCategories.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (entity is null)
        {
            return false;
        }

        // COALESCE semantics: only set when dto provides non-null value
        if (dto.Name is not null)
        {
            entity.Name = dto.Name;
        }

        if (dto.CategoryId.HasValue)
        {
            entity.CategoryId = dto.CategoryId.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        if (dto.StoreId.HasValue)
        {
            entity.StoreId = dto.StoreId.Value;
        }

        try
        {
            var affected = await context.SaveChangesAsync(ct);
            return affected > 0;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to update sub-category {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await context.SubCategories.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (entity is null)
        {
            return false;
        }

        context.SubCategories.Remove(entity);
        var affected = await context.SaveChangesAsync(ct);
        return affected > 0;
    }

    public async Task<SubCategoryDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.SubCategories
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new SubCategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.CategoryId,
                IsActive = x.IsActive,
                StoreId = x.StoreId
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<SubCategoryDto>> GetAllAsync(CancellationToken ct)
    {
        return await context.SubCategories
            .AsNoTracking()
            .OrderByDescending(s => s.Id)
            .Select(x => new SubCategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.CategoryId,
                IsActive = x.IsActive,
                StoreId = x.StoreId
            })
            .ToListAsync(ct);
    }
}
