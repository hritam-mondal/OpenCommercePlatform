namespace OCP.API.DTOs;

public sealed class CreateOrderDto
{
    public string? OrderId { get; set; }
    public string? OrderName { get; set; }
    public int UserId { get; set; }
    public string? OrderNote { get; set; }
    public string? OrderStatus { get; set; }
    public string? PaymentStatus { get; set; }
    public string? PaymentMode { get; set; }
    public int? StoreId { get; set; }
    public decimal? OverallDiscountPer { get; set; }
    public List<OrderItemInputDto> Items { get; set; } = new();
    public OrderAddressInputDto? Address { get; set; }
}
