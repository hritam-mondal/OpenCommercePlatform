using OCP.API.DTOs;
using OCP.API.Repositories;

namespace OCP.API.Services.Pricing;

public class DiscountsService(IDiscountRepository repo) : IDiscountsService
{
    public Task<int> CreateAsync(CreateDiscountDto dto, CancellationToken ct)
    {
        return repo.CreateAsync(dto, ct);
    }

    public Task<bool> UpdateAsync(int id, UpdateDiscountDto dto, CancellationToken ct)
    {
        return repo.UpdateAsync(id, dto, ct);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        return repo.DeleteAsync(id, ct);
    }

    public Task<DiscountDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return repo.GetByIdAsync(id, ct);
    }

    public Task<IReadOnlyList<DiscountDto>> GetAllAsync(CancellationToken ct)
    {
        return repo.GetAllAsync(ct);
    }
}
