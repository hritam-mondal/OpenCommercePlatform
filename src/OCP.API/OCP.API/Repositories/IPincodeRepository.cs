using OCP.API.DTOs;

namespace OCP.API.Repositories;

public interface IPincodeRepository
{
    Task<IReadOnlyList<PincodeDto>> GetByPinAsync(string pin, CancellationToken ct);
    Task<IReadOnlyList<PincodeDto>> GetByStoreAsync(int storeId, CancellationToken ct);
}
