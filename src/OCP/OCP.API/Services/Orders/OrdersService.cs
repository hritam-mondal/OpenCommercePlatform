using OCP.API.DTOs;
using OCP.API.Repositories;

namespace OCP.API.Services.Orders;

public class OrdersService(IOrderRepository repo) : IOrdersService
{
    public Task<string> CreateAsync(CreateOrderDto dto, CancellationToken ct)
    {
        return repo.CreateAsync(dto, ct);
    }

    public Task<IReadOnlyList<OrderSummaryDto>> ListAsync(int userId, int? storeId, int limit, int offset,
        CancellationToken ct)
    {
        return repo.ListAsync(userId, storeId, limit, offset, ct);
    }

    public Task<OrderDto?> GetByIdAsync(string orderId, CancellationToken ct)
    {
        return repo.GetByIdAsync(orderId, ct);
    }
}
