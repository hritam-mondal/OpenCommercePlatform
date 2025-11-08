using OCP.API.Data;
using OCP.API.DTOs;
using OCP.API.Models;

namespace OCP.API.Repositories;

public class UnitRepository(
    ApplicationDbContext context,
    ILogger<UnitRepository> logger) : IUnitRepository
{
    public async Task<int> CreateAsync(CreateUnitDto dto, CancellationToken ct)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var entity = new Unit
        {
            UnitName = dto.UnitName, UnitOrder = dto.UnitOrder, ConversionRatio = dto.ConversionRatio
        };

        await using var tx = await context.Database.BeginTransactionAsync(ct);
        try
        {
            context.Units.Add(entity);
            await context.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return entity.Id;
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(ct);
            logger.LogError(ex, "Failed to create unit");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(int id, UpdateUnitDto dto, CancellationToken ct)
    {
        var entity = await context.Units.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (entity is null)
        {
            return false;
        }

        // COALESCE-like updates: only update when dto provides a non-null value
        if (dto.UnitName is not null)
        {
            entity.UnitName = dto.UnitName;
        }

        if (dto.UnitOrder.HasValue)
        {
            entity.UnitOrder = dto.UnitOrder.Value;
        }

        if (dto.ConversionRatio.HasValue)
        {
            entity.ConversionRatio = dto.ConversionRatio.Value;
        }

        try
        {
            var affected = await context.SaveChangesAsync(ct);
            return affected > 0;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to update unit {UnitId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await context.Units.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (entity is null)
        {
            return false;
        }

        context.Units.Remove(entity);
        var affected = await context.SaveChangesAsync(ct);
        return affected > 0;
    }

    public async Task<UnitDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var u = await context.Units
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new UnitDto
            {
                Id = x.Id, UnitName = x.UnitName, UnitOrder = x.UnitOrder, ConversionRatio = x.ConversionRatio
            })
            .FirstOrDefaultAsync(ct);

        return u;
    }

    public async Task<IReadOnlyList<UnitDto>> GetAllAsync(CancellationToken ct)
    {
        return await context.Units
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Select(x => new UnitDto
            {
                Id = x.Id, UnitName = x.UnitName, UnitOrder = x.UnitOrder, ConversionRatio = x.ConversionRatio
            })
            .ToListAsync(ct);
    }
}
