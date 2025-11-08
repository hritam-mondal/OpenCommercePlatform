using OCP.API.Data;
using OCP.API.DTOs;
using OCP.API.Models;

namespace OCP.API.Repositories;

public class OrderRepository(ApplicationDbContext db, ILogger<OrderRepository> logger) : IOrderRepository
{
    private readonly ApplicationDbContext _db = db ?? throw new ArgumentNullException(nameof(db));
    private readonly ILogger<OrderRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<string> CreateAsync(CreateOrderDto dto, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var orderId = string.IsNullOrWhiteSpace(dto.OrderId)
            ? $"ORD-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}"
            : dto.OrderId!;

        // calculate totals preserving original logic
        var total = dto.Items.Sum(i => i.ItemPrice);
        var final = total * (1 - ((dto.OverallDiscountPer ?? 0) / 100m));

        // create entities
        var order = new Order
        {
            OrderId = orderId,
            OrderName = dto.OrderName,
            TotalPrice = total,
            UserId = dto.UserId,
            OrderNote = dto.OrderNote,
            OrderStatus = dto.OrderStatus ?? "CREATED",
            PaymentStatus = dto.PaymentStatus ?? "UNPAID",
            IsActive = true,
            PaymentMode = dto.PaymentMode,
            StoreId = dto.StoreId,
            FinalPrice = final,
            OverallDiscountPer = dto.OverallDiscountPer
        };

        // add order details
        foreach (var l in dto.Items)
        {
            var detail = new OrderDetail
            {
                OrderId = orderId,
                ItemId = l.ItemId,
                ItemName = l.ItemName,
                ItemQuantity = l.Quantity,
                UnitId = l.UnitId,
                Rate = l.Rate,
                ItemPrice = l.ItemPrice,
                IsDeleted = false,
                StoreId = l.StoreId,
                DiscountPer = l.DiscountPer,
                ActualRate = l.ActualRate
            };

            order.OrderDetails.Add(detail);
        }

        // add address if present
        if (dto.Address is not null)
        {
            var addr = new Address
            {
                OrderId = orderId,
                UserId = dto.UserId,
                FullName = dto.Address.FullName,
                UserAddress = dto.Address.Address,
                City = dto.Address.City,
                Pin = dto.Address.Pin,
                Phone = dto.Address.Phone,
                Email = dto.Address.Email,
                AddressType = dto.Address.AddressType,
                State = dto.Address.State
            };

            order.Addresses.Add(addr);
        }

        // use explicit transaction to match previous behavior
        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return orderId;
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(ct);
            _logger.LogError(ex, "Failed to create order for user {UserId}", dto.UserId);
            throw;
        }
    }

    public async Task<IReadOnlyList<OrderSummaryDto>> ListAsync(int userId, int? storeId, int limit, int offset,
        CancellationToken ct)
    {
        var q = _db.Orders
            .AsNoTracking()
            .Where(o => o.UserId == userId && (storeId == null || o.StoreId == storeId))
            .OrderByDescending(o => o.OrderDate)
            .Skip(offset)
            .Take(limit)
            .Select(o => new OrderSummaryDto
            {
                OrderId = o.OrderId,
                OrderName = o.OrderName,
                OrderDate =
                    o.OrderDate.HasValue
                        ? DateTime.SpecifyKind(o.OrderDate.Value, DateTimeKind.Utc)
                        : DateTime.UtcNow,
                TotalPrice = o.TotalPrice ?? 0m,
                FinalPrice = o.FinalPrice,
                UserId = (int)o.UserId,
                StoreId = o.StoreId,
                OrderStatus = o.OrderStatus,
                PaymentStatus = o.PaymentStatus
            });

        var list = await q.ToListAsync(ct);
        return list;
    }

    public async Task<OrderDto?> GetByIdAsync(string orderId, CancellationToken ct)
    {
        var order = await _db.Orders
            .AsNoTracking()
            .Include(o => o.OrderDetails)
            .Include(o => o.Addresses)
            .FirstOrDefaultAsync(o => o.OrderId == orderId, ct);

        if (order is null)
        {
            return null;
        }

        var dto = new OrderDto
        {
            OrderId = order.OrderId,
            OrderName = order.OrderName,
            OrderDate =
                order.OrderDate.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(order.OrderDate.Value, DateTimeKind.Utc))
                    : DateTimeOffset.UtcNow,
            TotalPrice = order.TotalPrice ?? 0m,
            UserId = (int)order.UserId,
            OrderNote = order.OrderNote,
            OrderStatus = order.OrderStatus,
            PaymentStatus = order.PaymentStatus,
            IsActive = order.IsActive,
            PaymentMode = order.PaymentMode,
            StoreId = order.StoreId,
            FinalPrice = order.FinalPrice,
            OverallDiscountPer = order.OverallDiscountPer
        };

        foreach (var od in order.OrderDetails.OrderBy(d => d.Id))
        {
            dto.Lines.Add(new OrderDetailLineDto
            {
                Id = od.Id,
                ItemId = od.ItemId ?? 0,
                ItemName = od.ItemName ?? string.Empty,
                Quantity = od.ItemQuantity ?? 0m,
                UnitId = od.UnitId ?? 0,
                Rate = od.Rate ?? 0m,
                ItemPrice = od.ItemPrice ?? 0m,
                IsDeleted = od.IsDeleted ?? false,
                StoreId = od.StoreId ?? 0,
                DiscountPer = od.DiscountPer ?? 0m,
                ActualRate = od.ActualRate ?? 0m
            });
        }

        var addr = order.Addresses.OrderByDescending(a => a.Id).FirstOrDefault();
        if (addr is not null)
        {
            dto.Address = new OrderAddressDto
            {
                Id = addr.Id,
                FullName = addr.FullName ?? string.Empty,
                Address = addr.UserAddress ?? string.Empty,
                City = addr.City ?? string.Empty,
                Pin = addr.Pin ?? string.Empty,
                Phone = addr.Phone ?? string.Empty,
                Email = addr.Email ?? string.Empty,
                AddressType = addr.AddressType,
                State = addr.State ?? string.Empty
            };
        }

        return dto;
    }
}
