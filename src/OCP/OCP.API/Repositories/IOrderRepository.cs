using OCP.API.DTOs;

namespace OCP.API.Repositories;

public interface IOrderRepository
{
    Task<string> CreateAsync(CreateOrderDto dto, CancellationToken ct);

    Task<IReadOnlyList<OrderSummaryDto>> ListAsync(int userId, int? storeId, int limit, int offset,
        CancellationToken ct);

    Task<OrderDto?> GetByIdAsync(string orderId, CancellationToken ct);
}
