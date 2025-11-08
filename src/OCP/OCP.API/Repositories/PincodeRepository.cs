using OCP.API.Data;
using OCP.API.DTOs;

namespace OCP.API.Repositories;

public class PincodeRepository(ApplicationDbContext context) : IPincodeRepository
{
    private readonly ApplicationDbContext _db = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<IReadOnlyList<PincodeDto>> GetByPinAsync(string pin, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(pin)) return [];

        var q = _db.Pincodes
            .AsNoTracking()
            .Where(p => p.Pin == pin)
            .Select(p => new PincodeDto
            {
                Pin = p.Pin,
                DeliverDay = p.DeliverDay,
                StoreId = p.StoreId
            });

        return await q.ToListAsync(ct);
    }

    public async Task<IReadOnlyList<PincodeDto>> GetByStoreAsync(int storeId, CancellationToken ct)
    {
        var q = _db.Pincodes
            .AsNoTracking()
            .Where(p => p.StoreId == storeId)
            .Select(p => new PincodeDto
            {
                Pin = p.Pin,
                DeliverDay = p.DeliverDay,
                StoreId = p.StoreId
            });

        return await q.ToListAsync(ct);
    }
}
