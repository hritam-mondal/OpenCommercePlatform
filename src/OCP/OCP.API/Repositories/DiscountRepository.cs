using OCP.API.Data;
using OCP.API.DTOs;
using OCP.API.Models;

namespace OCP.API.Repositories;

public class DiscountRepository(ApplicationDbContext context) : IDiscountRepository
{
    public async Task<int> CreateAsync(CreateDiscountDto dto, CancellationToken ct)
    {
        var discount = new Discount
        {
            DiscountPer = dto.DiscountPer,
            DiscountAmt = dto.DiscountAmt,
            IsApplicable = dto.IsApplicable ?? false,
            IsDeleted = false,
            Remarks = dto.Remarks,
            StoreId = dto.StoreId
        };
        context.Discounts.Add(discount);
        await context.SaveChangesAsync(ct);
        return discount.Id;
    }

    public async Task<bool> UpdateAsync(int id, UpdateDiscountDto dto, CancellationToken ct)
    {
        var entity = await context.Discounts.FindAsync([id], ct);
        if (entity == null)
        {
            return false;
        }

        if (dto.DiscountPer.HasValue)
        {
            entity.DiscountPer = dto.DiscountPer.Value;
        }

        if (dto.DiscountAmt.HasValue)
        {
            entity.DiscountAmt = dto.DiscountAmt.Value;
        }

        if (dto.IsApplicable.HasValue)
        {
            entity.IsApplicable = dto.IsApplicable.Value;
        }

        if (dto.IsDeleted.HasValue)
        {
            entity.IsDeleted = dto.IsDeleted.Value;
        }

        if (dto.Remarks is not null)
        {
            entity.Remarks = dto.Remarks;
        }

        context.Discounts.Update(entity);
        var affected = await context.SaveChangesAsync(ct);
        return affected > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await context.Discounts.FindAsync([id], ct);
        if (entity == null)
        {
            return false;
        }

        context.Discounts.Remove(entity);
        var affected = await context.SaveChangesAsync(ct);
        return affected > 0;
    }

    public async Task<DiscountDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var e = await context.Discounts
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, ct);

        if (e == null)
        {
            return null;
        }

        return new DiscountDto
        {
            Id = e.Id,
            DiscountPer = e.DiscountPer,
            DiscountAmt = e.DiscountAmt,
            IsApplicable = e.IsApplicable,
            IsDeleted = e.IsDeleted,
            Remarks = e.Remarks,
            StoreId = e.StoreId
        };
    }

    public async Task<IReadOnlyList<DiscountDto>> GetAllAsync(CancellationToken ct)
    {
        return await context.Discounts
            .AsNoTracking()
            .OrderByDescending(d => d.Id)
            .Select(e => new DiscountDto
            {
                Id = e.Id,
                DiscountPer = e.DiscountPer,
                DiscountAmt = e.DiscountAmt,
                IsApplicable = e.IsApplicable,
                IsDeleted = e.IsDeleted,
                Remarks = e.Remarks,
                StoreId = e.StoreId
            })
            .ToListAsync(ct);
    }
}
