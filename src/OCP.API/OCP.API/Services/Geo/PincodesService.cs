using OCP.API.DTOs;
using OCP.API.Repositories;

namespace OCP.API.Services.Geo;

public class PincodesService(IPincodeRepository repo) : IPincodesService
{
    public Task<IReadOnlyList<PincodeDto>> GetByPinAsync(string pin, CancellationToken ct)
    {
        return repo.GetByPinAsync(pin, ct);
    }

    public Task<IReadOnlyList<PincodeDto>> GetByStoreAsync(int storeId, CancellationToken ct)
    {
        return repo.GetByStoreAsync(storeId, ct);
    }
}
