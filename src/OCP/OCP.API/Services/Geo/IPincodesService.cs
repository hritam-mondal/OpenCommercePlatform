using OCP.API.DTOs;

namespace OCP.API.Services.Geo;

public interface IPincodesService
{
    Task<IReadOnlyList<PincodeDto>> GetByPinAsync(string pin, CancellationToken ct);
    Task<IReadOnlyList<PincodeDto>> GetByStoreAsync(int storeId, CancellationToken ct);
}
