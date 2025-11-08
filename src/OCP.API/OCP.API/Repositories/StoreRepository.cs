using Microsoft.EntityFrameworkCore;
using OCP.API.Data;
using OCP.API.DTOs;
using OCP.API.Models;

namespace OCP.API.Repositories;

public class StoreRepository(ApplicationDbContext context, ILogger<StoreRepository> logger) : IStoreRepository
{
    public async Task<int> CreateAsync(CreateStoreDto dto, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var entity = new Store
        {
            Name = dto.Name,
            IsActive = dto.IsActive,
            ImageUrl = dto.ImageUrl,
            CompanyName = dto.CompanyName,
            CompanyAddress = dto.CompanyAddress,
            Gstn = dto.Gstn
        };

        await using var tx = await context.Database.BeginTransactionAsync(ct);
        try
        {
            context.Stores.Add(entity);
            await context.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return entity.Id;
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(ct);
            logger.LogError(ex, "Failed to create store");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(int id, UpdateStoreDto dto, CancellationToken ct)
    {
        var entity = await context.Stores.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (entity is null)
        {
            return false;
        }

        // COALESCE-like updates: only update if dto property is not null
        if (dto.Name is not null)
        {
            entity.Name = dto.Name;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        if (dto.ImageUrl is not null)
        {
            entity.ImageUrl = dto.ImageUrl;
        }

        if (dto.CompanyName is not null)
        {
            entity.CompanyName = dto.CompanyName;
        }

        if (dto.CompanyAddress is not null)
        {
            entity.CompanyAddress = dto.CompanyAddress;
        }

        if (dto.Gstn is not null)
        {
            entity.Gstn = dto.Gstn;
        }

        try
        {
            var affected = await context.SaveChangesAsync(ct);
            return affected > 0;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to update store {StoreId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await context.Stores.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (entity is null)
        {
            return false;
        }

        context.Stores.Remove(entity);
        var affected = await context.SaveChangesAsync(ct);
        return affected > 0;
    }

    public async Task<StoreDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var s = await context.Stores
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new StoreDto
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive,
                ImageUrl = x.ImageUrl,
                CompanyName = x.CompanyName,
                CompanyAddress = x.CompanyAddress,
                Gstn = x.Gstn
            })
            .FirstOrDefaultAsync(ct);

        return s;
    }

    public async Task<IReadOnlyList<StoreDto>> GetAllAsync(CancellationToken ct)
    {
        return await context.Stores
            .AsNoTracking()
            .OrderByDescending(s => s.Id)
            .Select(s => new StoreDto
            {
                Id = s.Id,
                Name = s.Name,
                IsActive = s.IsActive,
                ImageUrl = s.ImageUrl,
                CompanyName = s.CompanyName,
                CompanyAddress = s.CompanyAddress,
                Gstn = s.Gstn
            })
            .ToListAsync(ct);
    }
}
